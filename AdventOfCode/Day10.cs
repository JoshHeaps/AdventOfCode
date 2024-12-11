using System.Drawing;
using AdventOfCode2024;

namespace AdventOfCode2024;

public static class Day10
{
    public static int GetTrailheadCount(this string input)
    {
        Dictionary<Point, bool> pointsToTrailhead = [];
        int[,] map = input.To2DIntArray();
        var mappedPaths = map.SearchForPaths();

        return mappedPaths.Where(x => map[x.Key.X, x.Key.Y] == 0).Sum(x => x.Value.Count);
    }

    public static int GetDistinctTrailsCount(this string input)
    {
        Dictionary<Point, bool> pointsToTrailhead = [];
        int[,] map = input.To2DIntArray();
        var mappedPaths = map.SearchForUniquePaths();

        return mappedPaths.Where(x => map[x.Key.X, x.Key.Y] == 0).Sum(x => x.Value);
    }

    private static List<Point> GetTrailHeads(this int[,] map)
    {
        List<Point> result = [];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[j, i] == 0)
                    result.Add(new Point(j, i));
            }
        }

        return result;
    }

    private static List<Point> GetTrailScores(this int[,] map)
    {
        List<Point> result = [];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[j, i] == 9)
                    result.Add(new Point(j, i));
            }
        }

        return result;
    }

    private static Dictionary<Point, int> SearchForUniquePaths(this int[,] map)
    {
        var trailHeads = map.GetTrailHeads();

        Dictionary<Point, int> result = [];

        foreach (var point in trailHeads)
        {
            result = result.AddRange(map.UniquePathSearch(point, result));
        }

        return result;
    }

    private static Dictionary<Point, int> UniquePathSearch(this int[,] map, Point currentPoint, Dictionary<Point, int> checkedPaths)
    {
        if (checkedPaths.TryGetValue(currentPoint, out _))
            return checkedPaths;

        Dictionary<Point, int> copy = new(checkedPaths);

        if (map[currentPoint.X, currentPoint.Y] == 9)
        {
            copy.TryAdd(currentPoint, 1);
            return copy;
        }

        List<Point> possibleSteps = [];
        int value = map[currentPoint.X, currentPoint.Y];

        if (currentPoint.X > 0 && map[currentPoint.X - 1, currentPoint.Y] - value == 1)
            possibleSteps.Add(new(currentPoint.X - 1, currentPoint.Y));

        if (currentPoint.X < map.GetLength(1) - 1 && map[currentPoint.X + 1, currentPoint.Y] - value == 1)
            possibleSteps.Add(new(currentPoint.X + 1, currentPoint.Y));

        if (currentPoint.Y > 0 && map[currentPoint.X, currentPoint.Y - 1] - value == 1)
            possibleSteps.Add(new(currentPoint.X, currentPoint.Y - 1));

        if (currentPoint.Y < map.GetLength(0) - 1 && map[currentPoint.X, currentPoint.Y + 1] - value == 1)
            possibleSteps.Add(new(currentPoint.X, currentPoint.Y + 1));

        if (possibleSteps.Count == 0)
        {
            copy.Add(currentPoint, 0);
            return copy;
        }

        int count = 0;

        foreach (var step in possibleSteps)
        {
            copy = copy.AddRange(map.UniquePathSearch(step, copy));
            count += copy[step];
        }

        copy.Add(currentPoint, count);

        return copy;
    }

    private static Dictionary<Point, List<Point>> SearchForPaths(this int[,] map)
    {
        var trailHeads = map.GetTrailHeads();

        Dictionary<Point, List<Point>> result = [];

        foreach (var point in trailHeads)
        {
            result = result.AddRange(map.PathSearch(point, result));
        }

        return result;
    }

    private static Dictionary<Point, List<Point>> PathSearch(this int[,] map, Point currentPoint, Dictionary<Point, List<Point>> checkedPaths)
    {
        if (checkedPaths.TryGetValue(currentPoint, out _))
            return checkedPaths;

        Dictionary<Point, List<Point>> copy = new(checkedPaths);

        if (map[currentPoint.X, currentPoint.Y] == 9)
        {
            copy.TryAdd(currentPoint, [currentPoint]);
            return copy;
        }

        List<Point> possibleSteps = [];
        int value = map[currentPoint.X, currentPoint.Y];

        if (currentPoint.X > 0 && map[currentPoint.X - 1, currentPoint.Y] - value == 1)
            possibleSteps.Add(new(currentPoint.X - 1, currentPoint.Y));

        if (currentPoint.X < map.GetLength(1) - 1 && map[currentPoint.X + 1, currentPoint.Y] - value == 1)
            possibleSteps.Add(new(currentPoint.X + 1, currentPoint.Y));

        if (currentPoint.Y > 0 && map[currentPoint.X, currentPoint.Y - 1] - value == 1)
            possibleSteps.Add(new(currentPoint.X, currentPoint.Y - 1));

        if (currentPoint.Y < map.GetLength(0) - 1 && map[currentPoint.X, currentPoint.Y + 1] - value == 1)
            possibleSteps.Add(new(currentPoint.X, currentPoint.Y + 1));

        if (possibleSteps.Count == 0)
        {
            copy.Add(currentPoint, []);
            return copy;
        }

        List<Point> reachableScores = [];

        foreach (var step in possibleSteps)
        {
            copy = copy.AddRange(map.PathSearch(step, copy));
            reachableScores.AddRange(copy[step]);
        }

        copy.Add(currentPoint, reachableScores.Distinct().ToList());

        return copy;
    }

    private static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> current, Dictionary<TKey, TValue> range) where TKey : notnull
    {
        Dictionary<TKey, TValue> result = new(current);

        foreach (var pair in range)
            result.TryAdd(pair.Key, pair.Value);

        return result;
    }
}
