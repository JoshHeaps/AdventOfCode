using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public static partial class Day3
{
    public static int GetNonCorruptedMultiplyOnly(string input) =>
        input.RemoveCorruptedNonMultiplyInstructions().Select(GetNumbers).Sum(x => x.left * x.right);

    public static int GetNonCorruptedInstructionsResult(string input) =>
        input.RemoveCorruptedUnusedInstructions().Select(GetNumbers).Sum(x => x.left * x.right);

    private static List<string> RemoveCorruptedNonMultiplyInstructions(this string input)
    {
        var matches = MatchMultiplyInstructions().Matches(input);

        List<string> result = [];

        foreach (Match match in matches)
        {
            if (!match.Success)
                continue;

            result.Add(match.Value);
        }

        return result;
    }

    private static List<string> RemoveCorruptedUnusedInstructions(this string input)
    {
        var matches = MatchAllInstructions().Matches(input);

        List<string> result = [];
        bool shouldAdd = true;

        foreach (Match match in matches)
        {
            if (!match.Success)
                continue;

            if (match.Value.StartsWith("do"))
            {
                shouldAdd = match.Value == @"do()";

                continue;
            }

            if (!shouldAdd)
                continue;

            result.Add(match.Value);
        }

        return result;
    }

    private static (int left, int right) GetNumbers(string input)
    {
        var splits = input.Split(',');

        var left = int.Parse(MatchNumber().Match(splits[0]).Value);
        var right = int.Parse(MatchNumber().Match(splits[1]).Value);

        return (left, right);
    }

    [GeneratedRegex(@"[0-9]+")]
    public static partial Regex MatchNumber();
    [GeneratedRegex(@"mul\([0-9]+,[0-9]+\)")]
    public static partial Regex MatchMultiplyInstructions();
    [GeneratedRegex(@"mul\([0-9]+,[0-9]+\)|do\(\)|don't\(\)")]
    public static partial Regex MatchAllInstructions();
}