using UnityEngine.Localization;

public struct LocaleInfo : ILocaleInfo
{
    private static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
    public readonly string Name { get; }

    public readonly string Identifier { get; }
    public LocaleInfo(string name, string identifier)
    {
        Name = name;
        Identifier = identifier;
    }
    public LocaleInfo(Locale locale)
    {
        // this is SUS because it might be different for other platforms but WHO CARES :P
        Name = CapitalizeFirstLetter(locale.Identifier.CultureInfo.NativeName);
        Identifier = locale.Identifier.Code;
    }
}
