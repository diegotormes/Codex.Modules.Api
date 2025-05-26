namespace Eternet.Web.UI.Extensions;
public static class StringExtensions
{
    public static string? GetNumericValue(this string? value)
    {
        var newValue = new string(value?.Where(char.IsDigit).ToArray());
        return string.IsNullOrEmpty(newValue) ? null : newValue;
    }
}
