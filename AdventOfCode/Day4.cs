using System.Drawing;

namespace AdventOfCode2024;

public static class Day4
{
    public static int GetTotalXMasCount(this string input)
    {
        var xmasArray = input.To2DCharArray();
        int result = 0;

        for (int i = 0; i < xmasArray.GetLength(1); i++)
        {
            for (int j = 0; j < xmasArray.GetLength(0); j++)
            {
                if (xmasArray[j, i] != 'X')
                    continue;

                result += xmasArray.GetPositionXMasCount(new(j, i));
            }
        }

        return result;
    }

    public static int GetTotalCrossMasCount(this string input)
    {
        var xmasArray = input.To2DCharArray();
        int result = 0;

        for (int i = 0; i < xmasArray.GetLength(1); i++)
        {
            for (int j = 0; j < xmasArray.GetLength(0); j++)
            {
                if (xmasArray[j, i] != 'A')
                    continue;

                result += xmasArray.IsCrossMas(new(j, i)) ? 1 : 0;
            }
        }

        return result;
    }

    private static char[,] To2DCharArray(this string input)
    {
        var lines = input.SplitToLines().ToList();
        char[,] result = new char[lines[0].Length, lines.Count];

        for (int i = 0;  i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                result[j, i] = lines[i][j];
            }
        }

        return result;
    }

    #region XMAS

    private static readonly char[] XMAS = { 'X', 'M', 'A', 'S' };

    private static int GetPositionXMasCount(this char[,] array, Point startingPoint, int depth = 3)
    {
        int result = 0;

        for (int i = -1; i <= 1; i++)
        {
            if (startingPoint.Y + (i * depth) < 0 || startingPoint.Y + (i * depth) >= array.GetLength(1))
                continue;

            for (int j = -1; j <= 1; j++)
            {
                if (startingPoint.X + (j * depth) < 0 || startingPoint.X + (j * depth) >= array.GetLength(0))
                    continue;

                if (array.IsDirectionXMas(startingPoint, j, i, 0))
                    result++;
            }
        }

        return result;
    }

    private static bool IsDirectionXMas(this char[,] array, Point currentPoint, int xChange, int yChange, int depth)
    {
        if (array[currentPoint.X, currentPoint.Y] != XMAS[depth])
            return false;

        if (depth == 3)
            return true;

        depth++;

        return array.IsDirectionXMas(new(currentPoint.X + xChange, currentPoint.Y + yChange), xChange, yChange, depth);
    }
    #endregion XMAS

    #region CrossMAS

    private static readonly char[] CrossMAS = { 'M', 'A', 'S' };

    private static bool IsCrossMas(this char[,] array, Point startingPoint)
    {
        (int x, int y) = (startingPoint.X, startingPoint.Y);

        if (x == 0 || y == 0 || x == array.GetLength(0) - 1 || y == array.GetLength(1) - 1)
            return false;

        bool isFlippedVertically = array[x + 1, y + 1] == array[x + 1, y - 1];
        bool isFlippedHorizontally = array[x + 1, y + 1] == array[x - 1, y + 1];

        if (!isFlippedHorizontally && !isFlippedVertically)
            return false;

        if (isFlippedHorizontally ==  isFlippedVertically)
            return false;

        Dictionary<char, int> map = new()
        {
            { 'X', 0 },
            { 'M', 0 },
            { 'A', 1 },
            { 'S', 0 },
        };

        for (int i = -1; i <= 1; i += 2)
            for (int j = -1; j <= 1; j += 2)
                map[array[x + i, y + j]]++;

        if (map['M'] != 2 || map['A'] != 1 || map['S'] != 2)
            return false;

        return true;
    }
    #endregion CrossMAS
}