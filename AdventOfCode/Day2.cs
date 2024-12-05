namespace AdventOfCode2024;

public static class Day2
{
    public static int SafeReportCount(string list) =>
        SplitStringToLists(list).Count(IsReportSafe);

    public static int SafeReportCountWithDampener(string list) =>
        SplitStringToLists(list).Count(IsDampenedReportSafe);

    private static List<List<int>> SplitStringToLists(string list)
    {
        var lines = list.SplitToLines();

        var result = new List<List<int>>();
        int i = 0;

        foreach (var line in lines)
        {
            var splits = line.Split(' ');

            result.Add([]);

            foreach (var split in splits)
            {
                result[i].Add(int.Parse(split));
            }

            i++;
        }

        return result;
    }

    private static bool IsDampenedReportSafe(List<int> report)
    {
        var isSafe = IsReportSafe(report);

        if (isSafe)
            return true;

        for (int i = 0; i < report.Count; i++)
        {
            var copy = new List<int>(report);
            copy.RemoveAt(i);

            if (IsReportSafe(copy))
                return true;
        }

        return false;
    }

    private static bool IsReportSafe(List<int> report)
    {
        if (report.Count <= 1)
            return true;

        if (report.Count == 2)
            return Math.Abs(report[0] - report[1]) <= 3 && report[0] != report[1];

        if (report[0] == report[1])
            return false;

        bool isIncreasing = report[0] - report[1] < 0;

        for (int i = 0; i < report.Count - 1; i++)
        {
            int difference = report[i] - report[i + 1];

            bool unsafeSizeChange = Math.Abs(difference) > 3 || difference == 0;
            bool isChangeInWrongDirection = (difference > 0 && isIncreasing) || (difference < 0 && !isIncreasing);

            if (unsafeSizeChange || isChangeInWrongDirection)
                return false;
        }

        return true;
    }
}