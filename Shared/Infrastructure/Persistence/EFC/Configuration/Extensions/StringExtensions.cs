namespace prueba.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
///     String extensions
/// </summary>
/// <remarks>
///     This class contains extension methods for strings.
///     It includes methods to convert strings to snake case and pluralize them.
/// </remarks>
public static class StringExtensions
{
    /// <summary>
    ///     Convert a string to snake case
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>The string converted to snake case</returns>
    public static string ToSnakeCase(this string text)
    {
        return new string(Convert(text.GetEnumerator()).ToArray());

        static IEnumerable<char> Convert(CharEnumerator e)
        {
            if (!e.MoveNext()) yield break;

            yield return char.ToLower(e.Current);

            while (e.MoveNext())
                if (char.IsUpper(e.Current))
                {
                    yield return '_';
                    yield return char.ToLower(e.Current);
                }
                else
                {
                    yield return e.Current;
                }
        }
    }

    /// <summary>
    ///     Pluralize a string (minimal implementation)
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>The string converted to plural</returns>
    public static string ToPlural(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Simple pluralization rules
        var lowerText = text.ToLowerInvariant();
        
        // Words ending in 's', 'x', 'z', 'ch', 'sh' -> add 'es'
        if (lowerText.EndsWith("s") || lowerText.EndsWith("x") || lowerText.EndsWith("z") ||
            lowerText.EndsWith("ch") || lowerText.EndsWith("sh"))
        {
            return text + "es";
        }
        
        // Words ending in 'y' preceded by a consonant -> replace 'y' with 'ies'
        if (lowerText.EndsWith("y") && text.Length > 1)
        {
            var charBeforeY = lowerText[text.Length - 2];
            if (!IsVowel(charBeforeY))
            {
                return text.Substring(0, text.Length - 1) + "ies";
            }
        }
        
        // Default: add 's'
        return text + "s";
    }

    private static bool IsVowel(char c)
    {
        return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }
}