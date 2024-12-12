using System.Drawing;
using System.Runtime.InteropServices;

namespace AdventOfCode2024;

public static class Day12
{
    public static int GetFencingPrice(this string input, bool bulkDiscount = false)
    {
        var plainMap = input.To2DCharArray();
        Console.BufferHeight = plainMap.GetLength(0) * 5;
        Utils.SetUpColorPicking();

        Dictionary<GardenPlot, bool> map = [];

        for (int i = 0; i < plainMap.GetLength(0); i++)
        {
            for (int j = 0; j < plainMap.GetLength(1); j++)
            {
                GardenPlot plot = new(new(j, i), plainMap[j, i]);
                map.Add(plot, false);
                Console.Write(plainMap[j, i]);
            }
            Console.WriteLine();
        }

        List<GardenRegion> regions = [];

        foreach (var (plot, visited) in map)
        {
            if (visited)
                continue;

            regions.Add(new GardenRegion(ref map, plot, $"\x1b[48;2;{new Random().Next(256)};{new Random().Next(256)};{new Random().Next(256)}m"));
        }

        Console.ResetColor();
        Console.SetCursorPosition(0, plainMap.GetLength(0));
        Console.Write($"\x1b[48;2;{0};{0};{0}m");

        return regions.Sum(x => x.Area * (bulkDiscount ? x.Sides : x.Perimeter));
    }
}

public class GardenRegion
{
    public List<GardenPlot> GardenPlots { get; set; } = [];
    public int Area => GardenPlots.Count;
    public int Perimeter => CalculatePerimeter();
    public int Sides => CalculateSides();
    private string _color;

    public GardenRegion(ref Dictionary<GardenPlot, bool> map, GardenPlot startingPlot, string color)
    {
        _color = color;
        GardenPlots = [startingPlot];
        map[startingPlot] = true;
        SetUpPlots(ref map, startingPlot);
    }

    private void SetUpPlots(ref Dictionary<GardenPlot, bool> map, GardenPlot currentPlot)
    {
        Thread.Sleep(100);
        Console.SetCursorPosition(currentPlot.PlotPoint.X, currentPlot.PlotPoint.Y);
        Console.Write(_color + " ");
        List<GardenPlot> nextSteps = GetPossiblePlots(ref map, currentPlot);

        if (nextSteps.Count == 0)
            return;

        GardenPlots.AddRange(nextSteps);

        foreach (var step in nextSteps)
        {
            SetUpPlots(ref map, step);
        }
    }

    private int CalculatePerimeter()
    {
        int result = 0;

        foreach (var plot in GardenPlots)
        {
            (int x, int y) = (plot.PlotPoint.X, plot.PlotPoint.Y);

            if (!GardenPlots.Any(z => z.PlotPoint == new Point(x - 1, y)))
                result++;
            if (!GardenPlots.Any(z => z.PlotPoint == new Point(x + 1, y)))
                result++;
            if (!GardenPlots.Any(z => z.PlotPoint == new Point(x, y - 1)))
                result++;
            if (!GardenPlots.Any(z => z.PlotPoint == new Point(x, y + 1)))
                result++;
        }

        return result;
    }

    private int CalculateSides()
    {
        List<(List<Point> points, Directions d)> sides = [];

        foreach(var plot in GardenPlots)
        {
            (int x, int y) = (plot.PlotPoint.X, plot.PlotPoint.Y);
            List<Directions> openDirections = GetOpenDirections(plot);

            foreach(var direction in openDirections)
            {
                if (sides.Any(z => z.d == direction && z.points.Contains(plot.PlotPoint)))
                    continue;

                sides.Add((GetWall(direction, plot), direction));
            }
        }

        return sides.Count;
    }

    private List<Point> GetWall(Directions direction, GardenPlot plot)
    {
        List<Point> result = [];

        if (direction == Directions.North || direction == Directions.South)
        {
            for (int i = 0; GardenPlots.Any(x => x.PlotPoint == new Point(plot.PlotPoint.X - i, plot.PlotPoint.Y) && GetOpenDirections(x).Contains(direction)); i++)
            {
                result.Add(new Point(plot.PlotPoint.X - i, plot.PlotPoint.Y));
            }
            for (int i = 0; GardenPlots.Any(x => x.PlotPoint == new Point(plot.PlotPoint.X + i, plot.PlotPoint.Y) && GetOpenDirections(x).Contains(direction)); i++)
            {
                result.Add(new Point(plot.PlotPoint.X + i, plot.PlotPoint.Y));
            }
        }
        else if (direction == Directions.West || direction == Directions.East)
        {
            for (int i = 0; GardenPlots.Any(x => x.PlotPoint == new Point(plot.PlotPoint.X, plot.PlotPoint.Y - i) && GetOpenDirections(x).Contains(direction)); i++)
            {
                result.Add(new Point(plot.PlotPoint.X, plot.PlotPoint.Y - i));
            }
            for (int i = 0; GardenPlots.Any(x => x.PlotPoint == new Point(plot.PlotPoint.X, plot.PlotPoint.Y + i) && GetOpenDirections(x).Contains(direction)); i++)
            {
                result.Add(new Point(plot.PlotPoint.X, plot.PlotPoint.Y + i));
            }
        }

        return result.Distinct().ToList();
    }

    private List<Directions> GetOpenDirections(GardenPlot plot)
    {
        List<Directions> openDirections = [];
        (int x, int y) = (plot.PlotPoint.X, plot.PlotPoint.Y);

        if (!GardenPlots.Any(z => z.PlotPoint == new Point(x - 1, y)))
            openDirections.Add(Directions.West);
        if (!GardenPlots.Any(z => z.PlotPoint == new Point(x + 1, y)))
            openDirections.Add(Directions.East);
        if (!GardenPlots.Any(z => z.PlotPoint == new Point(x, y - 1)))
            openDirections.Add(Directions.North);
        if (!GardenPlots.Any(z => z.PlotPoint == new Point(x, y + 1)))
            openDirections.Add(Directions.South);

        return openDirections;
    }

    private enum Directions
    {
        North,South,East,West
    }

    private static List<GardenPlot> GetPossiblePlots(ref Dictionary<GardenPlot, bool> map, GardenPlot currentPlot)
    {
        List<GardenPlot> nextSteps = [];
        (int x, int y) = (currentPlot.PlotPoint.X, currentPlot.PlotPoint.Y);

        if (map.Any(plot => plot.Key.PlotPoint == new Point(x - 1, y) && plot.Key.PlotType == currentPlot.PlotType))
        {
            var result = map.First(plot => plot.Key.PlotPoint == new Point(x - 1, y) && plot.Key.PlotType == currentPlot.PlotType);

            if (!result.Value)
                nextSteps.Add(result.Key);

            map[result.Key] = true;
        }

        if (map.Any(plot => plot.Key.PlotPoint == new Point(x + 1, y) && plot.Key.PlotType == currentPlot.PlotType))
        {
            var result = map.First(plot => plot.Key.PlotPoint == new Point(x + 1, y) && plot.Key.PlotType == currentPlot.PlotType);

            if (!result.Value)
                nextSteps.Add(result.Key);

            map[result.Key] = true;
        }

        if (map.Any(plot => plot.Key.PlotPoint == new Point(x, y - 1) && plot.Key.PlotType == currentPlot.PlotType))
        {
            var result = map.First(plot => plot.Key.PlotPoint == new Point(x, y - 1) && plot.Key.PlotType == currentPlot.PlotType);

            if (!result.Value)
                nextSteps.Add(result.Key);

            map[result.Key] = true;
        }

        if (map.Any(plot => plot.Key.PlotPoint == new Point(x, y + 1) && plot.Key.PlotType == currentPlot.PlotType))
        {
            var result = map.First(plot => plot.Key.PlotPoint == new Point(x, y + 1) && plot.Key.PlotType == currentPlot.PlotType);

            if (!result.Value)
                nextSteps.Add(result.Key);

            map[result.Key] = true;
        }

        return nextSteps;
    }
}

public class GardenPlot(Point plotPoint, char plotType)
{
    public Point PlotPoint => plotPoint;
    public char PlotType => plotType;
}
