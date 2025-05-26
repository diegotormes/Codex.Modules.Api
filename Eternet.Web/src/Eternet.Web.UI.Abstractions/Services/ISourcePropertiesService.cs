namespace Eternet.Web.UI.Abstractions.Services;

public interface ISourcePropertiesService
{
    int? GetMaxLength(Type entityType, string propertyName);
}

public class SourcePropertiesServiceDefault : ISourcePropertiesService
{
    public int? GetMaxLength(Type entityType, string propertyName)
    {
        return null;
    }
}