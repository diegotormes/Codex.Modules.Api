namespace Eternet.Web.UI.Components.DataGrid.Models;

/// <summary>
/// Represents a saved configuration for an EternetDataGrid instance,
/// including column visibility/order and potentially filter states.
/// Designed for JSON serialization.
/// </summary>
public class EternetDataGridConfiguration
{
    /// <summary>
    /// User-defined name for this configuration.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The state (visibility, potentially order) of each column for this configuration.
    /// </summary>
    public List<EternetDataGridColumnState> ColumnStates { get; set; } = [];

    /// <summary>
    /// Indicates whether this configuration is the default configuration for the grid.
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// Saved value of the general search filter applied to the grid.
    /// </summary>
    public string? GeneralFilter { get; set; }

    /// <summary>
    /// Saved column specific filters. Keys are column names.
    /// </summary>
    public Dictionary<string, object?> ColumnFilters { get; set; } = [];

    /// <summary>
    /// Saved sort order for the grid. Each tuple represents a column and
    /// its direction.
    /// </summary>
    public List<(string Column, SortDirection Direction)> SortCriteria { get; set; } = [];

}
