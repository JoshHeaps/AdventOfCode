namespace AdventOfCode2024;

public static class Day11
{
    private static Dictionary<long, Dictionary<long, long>> stoneCounts = [];

    public static long GetStoneCount(this string input, int blinks = 6)
    {
        stoneCounts = [];
        List<long> stones = input.Split(' ').Select(long.Parse).ToList();

        long count = 0;

        foreach (var x in stones)
        {
            count += GetStoneCountFromBlinks(x, blinks);
        }

        return count;
    }

    private static long GetStoneCountFromBlinks(long stone, int remainingBlinks)
    {
        if (remainingBlinks == 0)
            return 1;

        if (stone == 0)
        {
            if (stoneCounts.TryGetValue(stone, out var dict) && (dict?.TryGetValue(remainingBlinks, out var count) ?? false))
                return count;

            var result = GetStoneCountFromBlinks(1, remainingBlinks - 1);

            stoneCounts.TryAdd(stone, []);
            stoneCounts[stone].TryAdd(remainingBlinks, result);

            return result;
        }

        if (stone.ToString().Length % 2 == 0)
        {
            if (stoneCounts.TryGetValue(stone, out var dict) && dict.TryGetValue(remainingBlinks, out var count))
                return count;

            string s = stone.ToString();

            long first = long.Parse(s[..(s.Length / 2)]);
            long second = long.Parse(s[(s.Length / 2)..]);

            var result = GetStoneCountFromBlinks(first, remainingBlinks - 1) + GetStoneCountFromBlinks(second, remainingBlinks - 1);

            stoneCounts.TryAdd(stone, []);
            stoneCounts[stone].TryAdd(remainingBlinks, result);

            return result;
        }

        return GetStoneCountFromBlinks(stone * 2024, remainingBlinks - 1);
    }
}
