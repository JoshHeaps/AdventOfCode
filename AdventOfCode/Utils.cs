using System.Runtime.InteropServices;

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

    public static int[,] To2DIntArray(this string input) =>
        input.SplitToLines().Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray().To2DArray();

    public static T[,] To2DArray<T>(this T[][] array)
    {
        T[,] result = new T[array.Length,array.Length];

        for (int i = 0;i < array.Length; i++)
        {
            for (int j = 0; j < array[i].Length; j++)
            {
                result[j,i] = array[i][j];
            }
        }

        return result;
    }

    public static void SetUpColorPicking()
    {
        var handle = GetStdHandle(-11);
        GetConsoleMode(handle, out int mode);
        SetConsoleMode(handle, mode | 0x4);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr handle, out int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int handle);
}
