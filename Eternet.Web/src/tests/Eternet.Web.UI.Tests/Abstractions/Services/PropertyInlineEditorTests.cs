using Eternet.Web.UI.Abstractions.Services;
using FluentAssertions;

namespace Eternet.Web.UI.Tests.Abstractions.Services;

public class PropertyInlineEditorTests
{
    private class View
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void Constructor_ValidExpressions_SetsMetadata()
    {
        var editor = new PropertyInlineEditor<View, Entity, int, string>(v => v.Name, v => v.Id, e => e.Name, e => e.Id);

        editor.ColumnName.Should().Be(nameof(View.Name));
        editor.ViewId.Should().Be(nameof(View.Id));
        editor.PropertyName.Should().Be(nameof(Entity.Name));
        editor.EntityId.Should().Be(nameof(Entity.Id));
        editor.EntityType.Should().Be(typeof(Entity));
        editor.ViewType.Should().Be(typeof(View));

        var view = new View { Id = 5 };
        editor.ViewIdSelector(view).Should().Be(5);
    }

    [Fact]
    public void Constructor_InvalidExpression_Throws()
    {
        var action = () => new PropertyInlineEditor<View, Entity, int, string>(v => v.Name.ToUpper(), v => v.Id, e => e.Name, e => e.Id);

        action.Should().Throw<InvalidOperationException>();
    }
}
