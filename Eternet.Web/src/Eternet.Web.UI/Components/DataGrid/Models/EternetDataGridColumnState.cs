namespace Eternet.Web.UI.Components.DataGrid.Models;

/// <summary>
/// Represents the state of a single column in the EternetDataGrid.
/// Used for managing visibility and persisting configurations.
/// </summary>
public record EternetDataGridColumnState
{
    /// <summary>
    /// The programmatic name of the column (usually the property name).
    /// </summary>
    public required string ColumnName { get; init; }

    /// <summary>
    /// The display title of the column.
    /// </summary>
    public required string Title { get; set; } // Allow title updates if metadata changes

    /// <summary>
    /// Whether the column is currently visible in the grid.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Persisted width of the column as a CSS value (e.g. "120px").
    /// </summary>
    public string? Width { get; set; }
}