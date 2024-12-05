namespace AdventOfCode2024;

public static class Day1
{
    public static int GetTotalDistance(string list)
    {
        var values = SplitStringToList(list);

        var leftValues = values.Select(x => x.left).ToList();
        leftValues.Sort();
        var rightValues = values.Select(values => values.right).ToList();
        rightValues.Sort();

        int result = 0;

        for (int i = 0; i < leftValues.Count; i++)
        {
            result += Math.Abs(leftValues[i] - rightValues[i]);
        }

        return result;
    }

    public static int GetSimilarityScore(string list)
    {
        var values = SplitStringToList(list);

        var leftValues = values.Select(x => x.left).ToList();
        var rightValues = values.Select(values => values.right).ToList();

        var max = values.Max(x => Math.Max(x.left, x.right));

        int[] array = new int[max];

        var uniqueLefts = leftValues.Distinct().ToList();

        foreach (var left in uniqueLefts)
        {
            array[left] = rightValues.Count(x => x == left);
        }

        return array.Select((x, i) => x * i).Sum();
    }

    private static List<(int left, int right)> SplitStringToList(string list)
    {
        List<(int left, int right)> result = [];

        foreach (var item in list.SplitToLines())
        {
            var split = item.Split(' ');
            int left = int.Parse(split.First());
            int right = int.Parse(split.Last());

            result.Add((left, right));
        }

        return result;
    }
}