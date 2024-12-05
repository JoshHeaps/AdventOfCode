namespace AdventOfCode2024;

public static class Utils
{
    public static IEnumerable<string> SplitToLines(this string input)
    {
        if (input == null)
        {
            yield break;
        }

        using StringReader reader = new(input);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}
