using System.Linq.Expressions;
using System.Reflection;
using Bunit;
using FluentAssertions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;
using Eternet.Web.UI.Components.DataGrid.Models;
using Microsoft.JSInterop;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class PersistConfigurationsAsyncTests : Bunit.TestContext
{
    private class Item
    {
        public string Name { get; set; } = string.Empty;
    }

    private class DummyViewsService : IViewsService
    {
        public IQueryable<T> GetViewAsQuery<T>() where T : class => Enumerable.Empty<T>().AsQueryable();
        public Task<bool> UpdateSourcePropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(true);
        public Task<bool> UpdateViewPropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(true);
    }

    private class DummyBuilder : IBuildStringLikeExpression
    {
        public Expression Build(Expression columnBody, string value) => columnBody;
    }

    private class DummyGridColumnService : IGridColumnService
    {
        public Dictionary<string, GridColumnMetadata<TGridItem>> GetColumnsFor<TGridItem>()
        {
            var prop = typeof(Item).GetProperty(nameof(Item.Name))!;
            var param = Expression.Parameter(typeof(TGridItem), "x");
            var body = Expression.Property(param, prop);
            var lambda = Expression.Lambda(body, param);
            var lambdaObj = Expression.Lambda<Func<TGridItem, object>>(Expression.Convert(body, typeof(object)), param);

            var meta = new GridColumnMetadata<TGridItem>
            {
                ColumnName = prop.Name,
                Title = prop.Name,
                AllowsSorting = false,
                AllowsSearching = true,
                AllowsFiltering = true,
                AllowsIntegerFiltering = false,
                LambdaExpression = lambda,
                PropertyExpression = lambdaObj,
                FilterableAttribute = null
            };
            return new Dictionary<string, GridColumnMetadata<TGridItem>> { [prop.Name] = meta };
        }
    }

    private class ThrowingJsRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new InvalidOperationException();
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new InvalidOperationException();
    }

    private class RecordingToastService : IToastService
    {
        public bool Called { get; private set; }
        public string? Message { get; private set; }

        public event Action<bool>? OnClearAll;
        public event Action<ToastIntent, bool>? OnClearIntent;
        public event Action? OnClearQueue;
        public event Action<ToastIntent>? OnClearQueueIntent;
        public event Action<Type?, ToastParameters, object>? OnShow;
        public event Action<string?, ToastParameters>? OnUpdate;
        public event Action<string>? OnClose;

        public void ClearAll(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearCustomIntentToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearDownloadToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearErrorToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearEventToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearInfoToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearIntent(ToastIntent intent, bool includeQueue = true) => throw new NotImplementedException();
        public void ClearMentionToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearProgressToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearQueue() => throw new NotImplementedException();
        public void ClearQueueCustomIntentToasts() => throw new NotImplementedException();
        public void ClearQueueDownloadToasts() => throw new NotImplementedException();
        public void ClearQueueErrorToasts() => throw new NotImplementedException();
        public void ClearQueueEventToasts() => throw new NotImplementedException();
        public void ClearQueueInfoToasts() => throw new NotImplementedException();
        public void ClearQueueMentionToasts() => throw new NotImplementedException();
        public void ClearQueueProgressToasts() => throw new NotImplementedException();
        public void ClearQueueSuccessToasts() => throw new NotImplementedException();
        public void ClearQueueToasts(ToastIntent intent) => throw new NotImplementedException();
        public void ClearQueueUploadToasts() => throw new NotImplementedException();
        public void ClearQueueWarningToasts() => throw new NotImplementedException();
        public void ClearSuccessToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearUploadToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void ClearWarningToasts(bool includeQueue = true) => throw new NotImplementedException();
        public void CloseToast(string id) => throw new NotImplementedException();
        public void ShowCommunicationToast(ToastParameters<CommunicationToastContent> parameters) => throw new NotImplementedException();
        public void ShowConfirmationToast(ToastParameters<ConfirmationToastContent> parameters) => throw new NotImplementedException();
        public void ShowCustom(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null, (Icon Value, Color Color)? icon = null) => throw new NotImplementedException();
        public void ShowDownload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowError(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null)
        {
            Called = true;
            Message = title;
        }
        public void ShowEvent(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowInfo(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowMention(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowProgress(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowProgressToast(ToastParameters<ProgressToastContent> parameters) => throw new NotImplementedException();
        public void ShowSuccess(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowToast(ToastIntent intent, string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowToast<TContent>(Type? toastComponent, ToastParameters parameters, TContent content) where TContent : class => throw new NotImplementedException();
        public void ShowToast<T, TContent>(ToastParameters<TContent> parameters) where T : IToastContentComponent<TContent> where TContent : class => throw new NotImplementedException();
        public void ShowUpload(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void ShowWarning(string title, int? timeout = null, string? topAction = null, EventCallback<ToastResult>? callback = null) => throw new NotImplementedException();
        public void UpdateToast<TContent>(string id, ToastParameters<TContent> parameters) where TContent : class => throw new NotImplementedException();
    }

    [Fact]
    public async Task PersistConfigurationsAsync_ShowsError_WhenJsFails()
    {
        Services.AddFluentUIComponents();
        var js = new ThrowingJsRuntime();
        var toast = new RecordingToastService();
        var grid = new EternetDataGrid<Item>(new DummyViewsService(), new DummyBuilder(), new DummyGridColumnService(), js, toast);

        var savedField = typeof(EternetDataGrid<Item>).GetField("_savedConfigurations", BindingFlags.NonPublic | BindingFlags.Instance)!;
        savedField.SetValue(grid, new List<EternetDataGridConfiguration>
        {
            new()
            {
                Name = "Cfg",
                ColumnStates = new List<EternetDataGridColumnState>(),
                ColumnFilters = new Dictionary<string, object?>(),
                Default = true
            }
        });

        var filterField = typeof(EternetDataGrid<Item>).GetField("_filterableColumns", BindingFlags.NonPublic | BindingFlags.Instance)!;
        filterField.SetValue(grid, ((DummyGridColumnService)new DummyGridColumnService()).GetColumnsFor<Item>());

        var method = typeof(EternetDataGrid<Item>).GetMethod("PersistConfigurationsAsync", BindingFlags.NonPublic | BindingFlags.Instance)!;
        await ((Task)method.Invoke(grid, null)!).ConfigureAwait(true);

        toast.Called.Should().BeTrue();
        toast.Message.Should().Be("No se pudieron guardar las configuraciones");
    }
}
