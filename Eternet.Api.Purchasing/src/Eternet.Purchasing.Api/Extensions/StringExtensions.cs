namespace Eternet.Purchasing.Api.Extensions;

public static class StringExtensions
{
    public static string GetFromMaxLen(this string value, int maxLen)
    {
        return value.Length > maxLen
            ? value.Substring(0, maxLen)
            : value;

    }
}
