namespace Eternet.Accounting.Api.Tests.Common;

public static class EternetFirebirdHelper
{
    public static string DbZipFile => "ETERNET.zip";
    public static string DbTestName => "ETERNET.FDB";

    public static string GetOutputDataPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var path = Path.Combine(baseDirectory, "Data");
        return path;
    }
}
