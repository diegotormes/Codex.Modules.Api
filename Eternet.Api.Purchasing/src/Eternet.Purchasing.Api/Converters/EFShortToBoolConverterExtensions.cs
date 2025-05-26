namespace Eternet.Purchasing.Api.Converters;

public static class EFShortToBoolConverterExtensions
{
    public static short ToLocalDbModel(bool local) =>
        local ? (short)1 : (short)0;

    public static bool ToLocalDomainModel(short value) =>
        value == 1;
}
