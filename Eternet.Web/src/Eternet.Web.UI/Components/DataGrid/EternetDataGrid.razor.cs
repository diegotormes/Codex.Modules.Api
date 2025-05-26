using System.Globalization;
using System.Linq.Expressions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;
using Eternet.Web.UI.Components.DataGrid.Models;
using Invoices.Incoming.WebApi.Components.DataGrid;
using Microsoft.JSInterop;

using ColumnName = string;

namespace Eternet.Web.UI;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class EternetDataGrid<TGridItem>(
    IViewsService viewsService,
    IBuildStringLikeExpression buildStringLikeExpression,
    IGridColumnService gridColumnService,
    IJSRuntime jsRuntime,
    IToastService toastService) : ComponentBase, IAsyncDisposable
    where TGridItem : class
{
    private DotNetObjectReference<EternetDataGrid<TGridItem>>? _dotNetHelper;
    private IQueryable<TGridItem> _items = Enumerable.Empty<TGridItem>().AsQueryable();

    [Parameter] public Dictionary<ColumnName, PropertyInlineEditor> InlineEditors { get; set; } = [];
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<MenuButtonItem<TGridItem>> Actions { get; set; } = [];

    private FluentDataGrid<TGridItem> _grid = null!;

    private bool _isLoading = true;
    private bool _showFilterPanel;
    private string _generalFilter = "";
    private bool _generalFilterIgnoreCase;
    private readonly PaginationState _paginationState = new() { ItemsPerPage = 25 };
    private Dictionary<ColumnName, GridColumnMetadata<TGridItem>> _allColumns = [];
    private Dictionary<ColumnName, Func<TGridItem, object>> _columnsGetFunctionCalling = [];
    private Dictionary<ColumnName, GridColumnMetadata<TGridItem>> _searchableColumns = [];

    private Dictionary<string, object?> _columnFilters = [];
    private Dictionary<string, GridColumnMetadata<TGridItem>> _filterableColumns = [];

    // --- Column Visibility State (Managed by parent, passed to dialog) ---
    private List<EternetDataGridColumnState> _currentColumnStates = [];
    private List<EternetDataGridConfiguration> _savedConfigurations = [];
    private List<(string Column, SortDirection Direction)> _sortCriteria = [];
    public List<(string Column, SortDirection Direction)> SortCriteria
    {
        get => _sortCriteria;
        set => _sortCriteria = value;
    }
    private bool _showColumnSelectorDialog;
    private static string LocalStorageKey => $"EternetDataGridConfig_{typeof(TGridItem).FullName}";
    // --- End Column Visibility State ---

    private string? _gridTemplateColumns;

    private readonly ColumnResizeLabels _resizeLabels = ColumnResizeLabels.Default with
    {
        ResizeMenu = "Redimensionar",
        DiscreteLabel = "(+/- 10px)",
        ResetAriaLabel = "Restaurar"
    };

    private const string UtilsJsFile = "./_content/Eternet.Web.UI/datagridExtensions.js";
    private IJSObjectReference? _utilsModule;

    private readonly ColumnSortLabels _sortLabels = ColumnSortLabels.Default with
    {
        SortMenu = "Ordenar",
        SortMenuAscendingLabel = "Ascendente",
        SortMenuDescendingLabel = "Descendente"
    };

    private readonly ColumnOptionsLabels _optionsLabels = ColumnOptionsLabels.Default with
    {
        OptionsMenu = "Filtrar"
    };

    private void UpdateGridTemplateColumns()
    {
        var widths = _currentColumnStates
            .Where(c => c.IsVisible)
            .Select(c => c.Width ?? "1fr");
        _gridTemplateColumns = string.Join(" ", widths);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _dotNetHelper = DotNetObjectReference.Create(this);

        _allColumns = gridColumnService.GetColumnsFor<TGridItem>();
        _columnsGetFunctionCalling = _allColumns.ToDictionary(c => c.Key, c => c.Value.PropertyExpression.Compile());
        _searchableColumns = _allColumns.Where(c => c.Value.AllowsSearching).ToDictionary();
        _filterableColumns = _allColumns.Where(c => c.Value.AllowsFiltering).ToDictionary();

        _currentColumnStates = _allColumns.Select(kvp => new EternetDataGridColumnState
        {
            ColumnName = kvp.Key,
            Title = kvp.Value.Title,
            IsVisible = true,
            Width = null
        }).ToList();

        UpdateGridTemplateColumns();

        foreach (var col in _filterableColumns)
        {
            _columnFilters[col.Key] = null;
        }

        GetItems();
        _isLoading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _utilsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", UtilsJsFile);
            await LoadConfigurationsAsync().ConfigureAwait(false);
            var defaultConfig = _savedConfigurations.FirstOrDefault(x => x.Default);
            if (defaultConfig != null)
            {
                var currentStateMatchesDefault = _currentColumnStates.Count == defaultConfig.ColumnStates.Count &&
                                                 _currentColumnStates.All(cs => defaultConfig.ColumnStates.Any(dcs => dcs.ColumnName == cs.ColumnName && dcs.IsVisible == cs.IsVisible));
                if (!currentStateMatchesDefault)
                {
                    await ApplyConfigurationAsync(defaultConfig).ConfigureAwait(false);
                }
            }
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }

    private async Task ApplyDefaultConfigurationAsync()
    {
        var defaultConfig = _savedConfigurations.FirstOrDefault(x => x.Default);
        if (defaultConfig != null)
        {
            await ApplyConfigurationAsync(defaultConfig).ConfigureAwait(false);
        }
    }

    private async Task ApplyConfigurationAsync(EternetDataGridConfiguration configToLoad)
    {
        var configuredOrder = new List<EternetDataGridColumnState>();
        var remaining = new HashSet<string>(_allColumns.Keys);

        foreach (var state in configToLoad.ColumnStates)
        {
            if (_allColumns.TryGetValue(state.ColumnName, out var meta))
            {
                configuredOrder.Add(new EternetDataGridColumnState
                {
                    ColumnName = state.ColumnName,
                    Title = meta.Title,
                    IsVisible = state.IsVisible,
                    Width = state.Width
                });
                remaining.Remove(state.ColumnName);
            }
        }

        foreach (var name in remaining)
        {
            var extraMeta = _allColumns[name];
            configuredOrder.Add(new EternetDataGridColumnState
            {
                ColumnName = name,
                Title = extraMeta.Title,
                IsVisible = true,
                Width = null
            });
        }

        _currentColumnStates = configuredOrder;
        UpdateGridTemplateColumns();
        _generalFilter = configToLoad.GeneralFilter ?? "";
        _columnFilters = configToLoad.ColumnFilters.Count != 0
            ? new Dictionary<string, object?>(configToLoad.ColumnFilters)
            : _filterableColumns.ToDictionary(fc => fc.Key, _ => (object?)null);

        _sortCriteria = configToLoad.SortCriteria ?? [];
        if (_sortCriteria.Count > 0 && _allColumns.TryGetValue(_sortCriteria[0].Column, out var sortMeta))
        {
            await _grid.SortByColumnAsync(sortMeta.Title, _sortCriteria[0].Direction).ConfigureAwait(false);
        }
        else
        {
            await _grid.RemoveSortByColumnAsync().ConfigureAwait(false);
        }
    }

    private void GetItems()
    {
        _items = viewsService.GetViewAsQuery<TGridItem>();
        ApplyFiltersInternal();
    }

    public bool ShowClearFilters => _generalFilter != "" || _columnFilters.Values.Any(f => f != null);

    public async Task ClearFiltersAsync()
    {
        _generalFilter = "";
        var keys = _columnFilters.Keys.ToList();
        foreach (var key in keys)
        {
            _columnFilters[key] = null;
        }
        await ApplyFiltersAndRefreshAsync().ConfigureAwait(false);
    }

    public void FlipShowFilterPanel()
    {
        _showFilterPanel = !_showFilterPanel;
    }

    private IQueryable<TGridItem> ApplyGeneralFilter(
        IQueryable<TGridItem> items,
        string generalFilter,
        Dictionary<ColumnName, GridColumnMetadata<TGridItem>> columns)
    {
        if (string.IsNullOrWhiteSpace(generalFilter))
        {
            return items;
        }

        var parsedFilter = generalFilter.ParseGeneralFilter();
        var allValuesAsStrings = parsedFilter
            .IntValues.Select(i => i.ToString(CultureInfo.InvariantCulture))
            .Concat(parsedFilter.LongValues.Select(i => i.ToString(CultureInfo.InvariantCulture)))
            .Concat(parsedFilter.StringValues).ToHashSet();

        if (parsedFilter.IntValues.Count == 0 &&
            parsedFilter.LongValues.Count == 0 &&
            parsedFilter.StringValues.Count == 0)
        {
            return items;
        }

        Expression? combinedExpression = null;
        var parameter = Expression.Parameter(typeof(TGridItem), "item");

        foreach (var column in columns.Values.Where(c => c.AllowsSearching))
        {
            combinedExpression = column.CreateFilterExpression(
                buildStringLikeExpression,
                parsedFilter,
                allValuesAsStrings,
                combinedExpression,
                parameter,
                _generalFilterIgnoreCase);
        }
        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<TGridItem, bool>>(combinedExpression, parameter);
            return items.Where(lambda);
        }
        else
        {
            return items;
        }
    }

    private IQueryable<TGridItem> ApplyColumnFilters(IQueryable<TGridItem> query)
    {
        var parameter = Expression.Parameter(typeof(TGridItem), "x");
        Expression? combinedFilterExpression = null;

        foreach (var filterEntry in _columnFilters)
        {
            var colKey = filterEntry.Key;
            var filterValue = filterEntry.Value;

            if (!_allColumns.TryGetValue(colKey, out var colMeta) || !colMeta.AllowsFiltering || filterValue == null)
            {
                continue;
            }

            if (filterValue is string s && string.IsNullOrWhiteSpace(s))
            {
                continue;
            }

            var propertyAccessExpression = new ParameterReplacer(colMeta.LambdaExpression.Parameters[0], parameter)
                                                        .Visit(colMeta.LambdaExpression.Body);

            Expression? currentColumnFilterExpression = null;

            // Handle StringFilters
            if (propertyAccessExpression.Type == typeof(string) && filterValue is StringFilters stringFilters && stringFilters.IsActive)
            {
                currentColumnFilterExpression = propertyAccessExpression.GenerateStringFilterExpression(stringFilters);
            }
            // Handle simple bool? filter
            else if (filterValue is bool boolValue && (propertyAccessExpression.Type == typeof(bool?) || propertyAccessExpression.Type == typeof(bool)))
            {
                currentColumnFilterExpression = propertyAccessExpression.GenerateBooleanFilterExpression(boolValue);
            }
            // Handle DateTime? filter (simple equality for now)
            else if (propertyAccessExpression.Type == typeof(DateTime?) || propertyAccessExpression.Type == typeof(DateTime))
            {
                if (filterValue is DateTime dateValue)
                {
                    Expression filterConstant;
                    // Handle nullable DateTime comparison correctly
                    if (propertyAccessExpression.Type == typeof(DateTime?))
                    {
                        filterConstant = Expression.Constant(dateValue, typeof(DateTime?));
                        // Check HasValue and Value == dateValue
                        var hasValueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<DateTime>.HasValue));
                        var valueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<DateTime>.Value));
                        var valueEqualsExpr = Expression.Equal(valueExpr, Expression.Constant(dateValue)); // Consider Date comparison if needed
                        currentColumnFilterExpression = Expression.AndAlso(hasValueExpr, valueEqualsExpr);
                    }
                    else // propertyAccessExpression.Type == typeof(DateTime)
                    {
                        filterConstant = Expression.Constant(dateValue, typeof(DateTime));
                        currentColumnFilterExpression = Expression.Equal(propertyAccessExpression, filterConstant);
                    }
                }
            }
            else if ((Nullable.GetUnderlyingType(propertyAccessExpression.Type) ?? propertyAccessExpression.Type).IsEnum)
            {
                if (filterValue != null)
                {
                    var targetType = Nullable.GetUnderlyingType(propertyAccessExpression.Type) ?? propertyAccessExpression.Type;
                    var enumValue = Enum.Parse(targetType, filterValue.ToString()!);
                    var constant = Expression.Constant(enumValue, targetType);
                    if (Nullable.GetUnderlyingType(propertyAccessExpression.Type) != null)
                    {
                        var hasValueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<int>.HasValue));
                        var valueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<int>.Value));
                        var equalExpr = Expression.Equal(valueExpr, constant);
                        currentColumnFilterExpression = Expression.AndAlso(hasValueExpr, equalExpr);
                    }
                    else
                    {
                        currentColumnFilterExpression = Expression.Equal(propertyAccessExpression, constant);
                    }
                }
            }
            else if (colMeta.LambdaExpression.IsColumnNumericType() ||
                     (Nullable.GetUnderlyingType(propertyAccessExpression.Type) is Type t && IsNumericType(t)))
            {
                if (filterValue is NumericFilter numFilter && numFilter.Number.HasValue)
                {
                    var targetType = Nullable.GetUnderlyingType(propertyAccessExpression.Type) ?? propertyAccessExpression.Type;
                    var converted = Convert.ChangeType(numFilter.Number.Value, targetType, CultureInfo.InvariantCulture);
                    var constant = Expression.Constant(converted, targetType);

                    Expression baseExpression = Nullable.GetUnderlyingType(propertyAccessExpression.Type) != null
                        ? Expression.Property(propertyAccessExpression, nameof(Nullable<int>.Value))
                        : propertyAccessExpression;

                    Expression comparison = numFilter.Operator switch
                    {
                        FilterNumericOperator.Equals => Expression.Equal(baseExpression, constant),
                        FilterNumericOperator.LessOrEqual => Expression.LessThanOrEqual(baseExpression, constant),
                        _ => Expression.GreaterThanOrEqual(baseExpression, constant)
                    };

                    if (Nullable.GetUnderlyingType(propertyAccessExpression.Type) != null)
                    {
                        var hasValueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<int>.HasValue));
                        comparison = Expression.AndAlso(hasValueExpr, comparison);
                    }
                    currentColumnFilterExpression = comparison;
                }
            }

            // Combine with overall filter expression
            if (currentColumnFilterExpression != null)
            {
                combinedFilterExpression = combinedFilterExpression == null
                    ? currentColumnFilterExpression
                    : Expression.AndAlso(combinedFilterExpression, currentColumnFilterExpression);
            }
        }

        if (combinedFilterExpression != null)
        {
            var lambdaFinal = Expression.Lambda<Func<TGridItem, bool>>(combinedFilterExpression, parameter);
            // System.Diagnostics.Debug.WriteLine($"Filter Lambda: {lambdaFinal}"); // For debugging
            try
            {
                return query.Where(lambdaFinal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying column filter expression: {ex.Message}");
                // Optionally log the expression: Console.WriteLine(lambdaFinal.ToString());
                return query; // Return unfiltered query on error
            }
        }
        else
        {
            return query;
        }
    }

    private void ApplyFiltersInternal()
    {
        var originalItems = viewsService.GetViewAsQuery<TGridItem>();
        var afterGeneral = ApplyGeneralFilter(originalItems, _generalFilter, _searchableColumns);
        _items = ApplyColumnFilters(afterGeneral);
    }

    public Task ApplyFiltersAndRefreshAsync() => ApplyFiltersAndRefreshAsync(false);

    public async Task ApplyFiltersAndRefreshAsync(bool closeColumnOptions = false)
    {
        if (closeColumnOptions && _grid is not null)
        {
            await _grid.CloseColumnOptionsAsync().ConfigureAwait(false);
        }
        ApplyFiltersInternal();
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    private GridSort<TGridItem>? GetSortBy(string columnName)
    {
        return _allColumns.TryGetValue(columnName, out var column) && column.AllowsSorting
            ? GridSort<TGridItem>.ByAscending(column.PropertyExpression)
            : null;
    }

    private async Task UpdateItemsAsync()
    {
        GetItems();
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    private string GetString(TGridItem item, string columnName)
    {
        if (_columnsGetFunctionCalling.TryGetValue(columnName, out var func))
        {
            var value = func(item);
            return value?.ToString() ?? "";
        }
        return "";
    }

    private static object GetViewId(TGridItem item, Func<object, object> func) => func(item);

    private static bool IsNumericType(Type t) =>
        t == typeof(int) || t == typeof(long) || t == typeof(short) ||
        t == typeof(double) || t == typeof(decimal) || t == typeof(float);

    private void HideColumnSelectorDialog() => _showColumnSelectorDialog = false;

    private async Task<bool> HandleSaveConfigurationAsync(string newConfigurationName)
    {
        if (string.IsNullOrWhiteSpace(newConfigurationName))
        {
            toastService.ShowError("El nombre de la configuración no puede estar vacío.");
            return false;
        }

        var existingConfig = _savedConfigurations.FirstOrDefault(c => c.Name.Equals(newConfigurationName, StringComparison.OrdinalIgnoreCase));
        if (existingConfig != null)
        {
            existingConfig.ColumnStates = _currentColumnStates.Select(cs => cs with { }).ToList();
            existingConfig.GeneralFilter = _generalFilter;
            existingConfig.ColumnFilters = new Dictionary<string, object?>(_columnFilters);
            existingConfig.SortCriteria = new List<(string Column, SortDirection Direction)>(_sortCriteria);
            return true;
        }
        else
        {
            // Add new configuration
            var newConfig = new EternetDataGridConfiguration
            {
                Name = newConfigurationName,
                ColumnStates = _currentColumnStates.Select(cs => cs with { }).ToList(), // Create copies
                Default = !_savedConfigurations.Any(c => c.Default), // Make first saved config default if none exists
                GeneralFilter = _generalFilter,
                ColumnFilters = new Dictionary<string, object?>(_columnFilters),
                SortCriteria = new List<(string Column, SortDirection Direction)>(_sortCriteria)
            };
            _savedConfigurations.Add(newConfig);
        }

        await PersistConfigurationsAsync().ConfigureAwait(false);
        HideColumnSelectorDialog(); // Close dialog after saving
        await InvokeAsync(StateHasChanged).ConfigureAwait(false); // Update grid and dialog list
        return true;
    }

    private async Task HandleLoadConfigurationAsync(string configurationNameToLoad)
    {
        if (string.IsNullOrWhiteSpace(configurationNameToLoad))
        {
            return;
        }

        var configToLoad = _savedConfigurations.FirstOrDefault(c => c.Name.Equals(configurationNameToLoad, StringComparison.OrdinalIgnoreCase));
        if (configToLoad != null)
        {
            await ApplyConfigurationAsync(configToLoad).ConfigureAwait(false);
            HideColumnSelectorDialog();
            await ApplyFiltersAndRefreshAsync().ConfigureAwait(false); // Re-apply filters and update grid items/UI
            await InvokeAsync(StateHasChanged).ConfigureAwait(false); // Ensure dialog and grid reflect loaded state
                               // Optionally persist this loaded state as the new default/current state if desired
        }
        else
        {
            Console.WriteLine($"Configuration '{configurationNameToLoad}' not found."); // User feedback needed
        }
    }

    private async Task HandleDeleteConfigurationAsync(string configurationName)
    {
        var configToRemove = _savedConfigurations.FirstOrDefault(c => c.Name.Equals(configurationName, StringComparison.OrdinalIgnoreCase));
        if (configToRemove != null)
        {
            var wasDefault = configToRemove.Default;
            _savedConfigurations.Remove(configToRemove);
            // If the deleted one was default, and others exist, make the first one default (or none if empty)
            if (wasDefault && _savedConfigurations.Count != 0 && !_savedConfigurations.Any(c => c.Default))
            {
                _savedConfigurations.First().Default = true;
            }
            await PersistConfigurationsAsync().ConfigureAwait(false);
            await InvokeAsync(StateHasChanged).ConfigureAwait(false); // Update UI (dialog list)
        }
    }

    private async Task HandleSetDefaultConfigurationAsync((string configurationName, bool isDefault) config)
    {
        var (configurationName, isDefault) = config;
        var configToModify = _savedConfigurations.FirstOrDefault(c => c.Name.Equals(configurationName, StringComparison.OrdinalIgnoreCase));
        if (configToModify == null)
        {
            return;
        }

        // Ensure only one default
        if (isDefault)
        {
            foreach (var c in _savedConfigurations)
            {
                c.Default = c.Name.Equals(configurationName, StringComparison.OrdinalIgnoreCase);
            }
        }
        else // Setting to false - ensure *some* default exists if possible
        {
            configToModify.Default = false;
            // If no other default exists, make the first one default
            if (!_savedConfigurations.Any(c => c.Default) && _savedConfigurations.Count != 0)
            {
                _savedConfigurations.First().Default = true;
            }
        }

        await PersistConfigurationsAsync().ConfigureAwait(false);
        await InvokeAsync(StateHasChanged).ConfigureAwait(false); // Update UI (dialog list)
    }

    private async Task PersistConfigurationsAsync()
    {
        try
        {
            // Ensure only one default before saving
            var defaultConfigs = _savedConfigurations.Where(c => c.Default).ToList();
            if (defaultConfigs.Count > 1)
            {
                // Keep the first one, unset others
                for (int i = 1; i < defaultConfigs.Count; i++)
                {
                    defaultConfigs[i].Default = false;
                }
            }
            else if (defaultConfigs.Count == 0 && _savedConfigurations.Count != 0)
            {
                // If none are default, make the first one default
                _savedConfigurations.First().Default = true;
            }

            foreach (var config in _savedConfigurations)
            {
                if (config.ColumnFilters.Count == 0)
                {
                    config.ColumnFilters = _filterableColumns.ToDictionary(fc => fc.Key, _ => (object?)null);
                }
                config.SortCriteria ??= [];
            }

            var json = JsonSerializer.Serialize(_savedConfigurations);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, json).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error persisting grid configurations: {ex.Message}");
            toastService.ShowError("No se pudieron guardar las configuraciones");
        }
    }

    private async Task LoadConfigurationsAsync()
    {
        try
        {
            var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(json))
            {
                var loadedConfigs = JsonSerializer.Deserialize<List<EternetDataGridConfiguration>>(json);
                _savedConfigurations = loadedConfigs ?? [];

                // Synchronize loaded configurations with current metadata
                // This ensures newly added columns appear and column titles are up-to-date
                var synchronizedStates = new List<EternetDataGridConfiguration>();
                foreach (var config in _savedConfigurations)
                {
                    var currentConfigColumns = config.ColumnStates.ToDictionary(cs => cs.ColumnName);
                    var updatedStates = new List<EternetDataGridColumnState>();

                    // Iterate over metadata columns to build the new state list
                    foreach (var metaCol in _allColumns)
                    {
                        updatedStates.Add(new EternetDataGridColumnState
                        {
                            ColumnName = metaCol.Key,
                            Title = metaCol.Value.Title, // Always take the current title
                            // Take visibility from saved state if exists, otherwise default to visible
                            IsVisible = currentConfigColumns.TryGetValue(metaCol.Key, out var savedState) ? savedState.IsVisible : true
                        });
                    }
                    // Only keep configurations that have at least one column matching current metadata? No, keep all, just sync.
                    config.ColumnStates = updatedStates; // Replace with synchronized list

                    var updatedFilters = new Dictionary<string, object?>();
                    foreach (var metaCol in _filterableColumns)
                    {
                        if (config.ColumnFilters.TryGetValue(metaCol.Key, out var val))
                        {
                            updatedFilters[metaCol.Key] = val;
                        }
                        else
                        {
                            updatedFilters[metaCol.Key] = null;
                        }
                    }
                    config.ColumnFilters = updatedFilters;

                    config.GeneralFilter ??= string.Empty;

                    synchronizedStates.Add(config);
                }
                _savedConfigurations = synchronizedStates;

                // Ensure only one default after loading and synchronizing
                var defaultConfigs = _savedConfigurations.Count(c => c.Default);
                if (defaultConfigs > 1)
                {
                    var firstDefault = _savedConfigurations.First(c => c.Default);
                    foreach (var config in _savedConfigurations.Where(c => c != firstDefault))
                    {
                        config.Default = false;
                    }
                }
                else if (defaultConfigs == 0 && _savedConfigurations.Count != 0)
                {
                    _savedConfigurations.First().Default = true; // Ensure at least one default if configs exist
                }
            }
            else
            {
                _savedConfigurations = [];
                // Initialize _currentColumnStates based on metadata if no config loaded
                _currentColumnStates = _allColumns.Select(kvp => new EternetDataGridColumnState
                {
                    ColumnName = kvp.Key,
                    Title = kvp.Value.Title,
                    IsVisible = true
                }).ToList();
                _generalFilter = string.Empty;
                _columnFilters = _filterableColumns.ToDictionary(fc => fc.Key, _ => (object?)null);
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Error deserializing grid configurations: {jsonEx.Message}. Resetting configurations.");
            _savedConfigurations = [];
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey).ConfigureAwait(false); // Remove potentially corrupted data
                                                                                                               // Initialize _currentColumnStates based on metadata after resetting
            _currentColumnStates = _allColumns.Select(kvp => new EternetDataGridColumnState
            {
                ColumnName = kvp.Key,
                Title = kvp.Value.Title,
                IsVisible = true
            }).ToList();
            _generalFilter = string.Empty;
            _columnFilters = _filterableColumns.ToDictionary(fc => fc.Key, _ => (object?)null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading grid configurations: {ex.Message}");
            _savedConfigurations = []; // Reset on other errors
                                       // Initialize _currentColumnStates based on metadata after error
            _currentColumnStates = _allColumns.Select(kvp => new EternetDataGridColumnState
            {
                ColumnName = kvp.Key,
                Title = kvp.Value.Title,
                IsVisible = true
            }).ToList();
            _generalFilter = string.Empty;
            _columnFilters = _filterableColumns.ToDictionary(fc => fc.Key, _ => (object?)null);
        }
        // Apply default configuration after loading/initialization
        await ApplyDefaultConfigurationAsync().ConfigureAwait(false);
    }

    //private async Task HandleColumnResizedAsync()
    //{
    //    if (_utilsModule is null)
    //    {
    //        return;
    //    }

    //    try
    //    {
    //        var widths = await _utilsModule.InvokeAsync<string[]>("getColumnWidths", _grid.Element);
    //        for (var i = 0; i < Math.Min(widths.Length, _currentColumnStates.Count); i++)
    //        {
    //            _currentColumnStates[i].Width = widths[i];
    //        }
    //        UpdateGridTemplateColumns();
    //        await PersistConfigurationsAsync().ConfigureAwait(false);
    //    }
    //    catch (JSException ex)
    //    {
    //        Console.WriteLine($"Error getting column widths: {ex.Message}");
    //    }
    //}

    private void OnColumnStatesChanged(List<EternetDataGridColumnState> updatedStates)
    {
        // This might not be strictly necessary if the dialog directly binds to _currentColumnStates,
        // but provides an explicit hook if needed for validation or other logic.
        _currentColumnStates = updatedStates;
        UpdateGridTemplateColumns();
        StateHasChanged(); // Update the grid rendering based on new visibility
    }

    public ValueTask DisposeAsync()
    {
        _dotNetHelper?.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
