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

    public static char[,] To2DCharArray(this string input)
    {
        var lines = input.SplitToLines().ToList();
        char[,] result = new char[lines[0].Length, lines.Count];

        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                result[j, i] = lines[i][j];
            }
        }

        return result;
    }
}
