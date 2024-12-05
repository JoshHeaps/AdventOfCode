
namespace AdventOfCode2024;

public static class Day5
{
    private static Dictionary<int, List<int>> PrecedingUpdateMap = [];

    public static int CorrectlyOrderedMiddleNumberSum(this string input)
    {
        var lines = input.SplitToLines();
        var ruleLines = lines.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
        var updateLines = lines.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1).ToList();

        SetUpRuleDictionaries(ruleLines);

        return updateLines.Select(x => x.IsOrderedCorrectlyOrEmpty()).Where(x => x.Count > 0).Select(x => x[x.Count / 2]).Sum();
    }

    public static int OrderedMiddleNumberSum(this string input)
    {
        var lines = input.SplitToLines();
        var ruleLines = lines.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
        var updateLines = lines.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1).ToList();

        SetUpRuleDictionaries(ruleLines);

        return updateLines.Where(x => x.IsOrderedCorrectlyOrEmpty().Count == 0).Select(OrderUpdatesCorrectly).Select(x => x[x.Count / 2]).Sum();
    }

    private static void SetUpRuleDictionaries(List<string> ruleLines)
    {
        PrecedingUpdateMap = [];

        foreach (var line in ruleLines)
        {
            var split = line.Split('|');
            var first = int.Parse(split[0]);
            var second = int.Parse(split[1]);

            if (!PrecedingUpdateMap.TryAdd(first, [second]))
                PrecedingUpdateMap[first].Add(second);
        }
    }

    private static List<int> IsOrderedCorrectlyOrEmpty(this string updateString)
    {
        List<int> updates = updateString.Split(',').Select(int.Parse).ToList();

        return IsOrderedCorrectly(updates) ? updates : [];
    }

    private static bool IsOrderedCorrectly(List<int> updates)
    {
        List<int> updated = [];

        foreach (var update in updates)
        {
            if (!PrecedingUpdateMap.TryGetValue(update, out var comesAfterUpdate))
            {
                updated.Add(update);

                continue;
            }

            if (updated.Any(comesAfterUpdate.Contains))
                return false;

            updated.Add(update);
        }

        return true;
    }

    private static List<int> OrderUpdatesCorrectly(string updateString)
    {
        List<int> updates = updateString.Split(',').Select(int.Parse).ToList();

        return OrderCorrectly(updates);
    }

    private static List<int> OrderCorrectly(List<int> updates)
    {
        List<int> updated = [];

        foreach (var update in updates)
        {
            if (!PrecedingUpdateMap.TryGetValue(update, out var comesAfterUpdate))
            {
                updated.Add(update);

                continue;
            }

            if (updated.Any(comesAfterUpdate.Contains))
            {
                var index = updated.FindIndex(comesAfterUpdate.Contains);
                var temp = updated[index];
                updated[index] = update;
                updated.Add(temp);

                continue;
            }

            updated.Add(update);
        }

        if (!IsOrderedCorrectly(updated))
            return OrderCorrectly(updated);

        return updated;
    }
}
