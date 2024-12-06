using System.Drawing;

namespace AdventOfCode2024;

public static class Day6
{
    public static int GetVisitedPositionsCount(this string input)
    {
        var map = input.To2DCharArray();
        var guardPosition = map.GetGuardPosition();
        var (movement, isLoop) = SimulateGuardMovement(map, guardPosition);

        return movement.Count;
    }

    public static int GetGuardLoopingCount(this string input)
    {
        var map = input.To2DCharArray();
        var guardPosition = map.GetGuardPosition();

        return map.SearchForObstructionLoops(guardPosition).Count();
    }

    private static Point GetGuardPosition(this char[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[j, i] == '^')
                    return new(j, i);
            }
        }

        throw new Exception();
    }

    private static IEnumerable<Point> SearchForObstructionLoops(this char[,] map, Point guardPosition)
    {
        // get squares guard will go to
        var (movement, _) = SimulateGuardMovement(map, guardPosition);

        foreach (var move in movement)
        {
            (Point square, _) = move;

            if (!map.IsPointOnMap(square))
                continue;

            if (map.ObstructionAtPointForcesGuardLoop(guardPosition, square))
                yield return square;
        }
    }

    private static bool ObstructionAtPointForcesGuardLoop(this char[,] map, Point guardPosition, Point obstructionPoint)
    {
        // change the map to have an obstruction at point.
        var temp = map[obstructionPoint.X, obstructionPoint.Y];
        map[obstructionPoint.X, obstructionPoint.Y] = '#';

        // simulate movement
        var (_, isLoop) = SimulateGuardMovement(map, guardPosition);

        // revert map
        map[obstructionPoint.X, obstructionPoint.Y] = temp;

        return isLoop;
    }

    private static (Dictionary<Point, Direction> movement, bool isLoop) SimulateGuardMovement(char[,] map, Point guardPosition)
    {
        Dictionary<Point, Direction> visitedSquares = [];
        int xDirection = 0;
        int yDirection = -1;
        Direction direction = Direction.North;

        while (map.IsPointOnMap(guardPosition))
        {
            // if obstacle is in the way
            while (map.IsPointOnMap(new(guardPosition.X + xDirection, guardPosition.Y + yDirection))
             && map[guardPosition.X + xDirection, guardPosition.Y + yDirection] == '#')
                (direction, xDirection, yDirection) = GetNextDirection(direction);

            guardPosition = new(guardPosition.X + xDirection, guardPosition.Y + yDirection);

            // if square has been visited previously
            if (visitedSquares.TryAdd(guardPosition, direction))
                continue;

            // if this is true, the guard is going in a loop.
            if (visitedSquares[guardPosition] == direction)
                return (visitedSquares, true);

            visitedSquares[guardPosition] = direction;
        }

        return (visitedSquares, false);
    }

    private static (Direction direction, int x, int y) GetNextDirection(Direction direction)
    {
        if (direction == Direction.West)
            direction = Direction.North;
        else
            direction++;

        (int x, int y) = DirectionValues[direction];

        return (direction, x, y);
    }

    private static bool IsPointOnMap(this char[,] map, Point guardPosition) =>
        guardPosition.X >= 0 && guardPosition.Y >= 0 && guardPosition.X < map.GetLength(0) && guardPosition.Y < map.GetLength(1);

    private static Dictionary<Direction, (int x, int y)> DirectionValues = new()
    {
        { Direction.North, (0, -1) },
        { Direction.East, (1, 0) },
        { Direction.South, (0, 1) },
        { Direction.West, (-1, 0) },
    };

    private enum Direction
    {
        North,
        East,
        South,
        West,
    }
}