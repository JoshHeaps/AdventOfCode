namespace AdventOfCode2024;

public static class Day7
{
    public static long GetTotalCalibrationResult(this string input)
    {
        return input.SplitToLines()
            .Select(x => x.Split(' ')
                .Select(y => long.Parse(y.Replace(":",""))).ToList())
                .Where(IsCalibrationPossible)
            .Select(x => x.First())
            .Sum();
    }

    private static bool IsCalibrationPossible(List<long> calibration)
    {
        List<Func<long, long>> funcs = [];

        return IsCalibrationPossible(calibration[1..], ref funcs, calibration[0]);
    }

    private static bool IsCalibrationPossible(List<long> calibration, ref List<Func<long, long>> funcs, long answer, int index = 0)
    {
        if (index == calibration.Count)
            return answer == GetFuncsResult(funcs);

        if (funcs.Count == 0)
        {
            funcs.Add(x => calibration[index]);

            return IsCalibrationPossible(calibration, ref funcs, answer, index + 1);
        }

        funcs.Add(x => x + calibration[index]);

        if (IsCalibrationPossible(calibration, ref funcs, answer, index + 1))
            return true;

        funcs = funcs[..^1];
        funcs.Add(x => x * calibration[index]);

        if (IsCalibrationPossible(calibration, ref funcs, answer, index + 1))
            return true;

        funcs = funcs[..^1];

        funcs.Add(x => long.Parse(x.ToString() + calibration[index].ToString()));

        if (IsCalibrationPossible(calibration, ref funcs, answer, index + 1))
            return true;

        funcs = funcs[..^1];

        return false;
    }

    private static long GetFuncsResult(List<Func<long, long>> funcs)
    {
        long result = 0;
        foreach (var func in funcs)
        {
            result = func(result);
        }
        return result;
    }
}