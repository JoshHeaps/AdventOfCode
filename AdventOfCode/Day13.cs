using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public static class Day13
{
    public static long GetLowestCost(this string input, bool shouldAugment = false)
    {
        var lines = input.SplitToLines().Where(x => !string.IsNullOrWhiteSpace(x));
        var machineStrings = Enumerable.Range(0, lines.Count() / 3).Select(x => lines.Skip(x * 3).Take(3).ToList()).ToList();
        List<ClawMachine> machines = [];

        foreach (var machine in machineStrings)
        {
            machines.Add(machine);
        }

        if (shouldAugment)
            machines.ForEach(x => x.AugmentPrizePosition());

        List<long> costs = [];

        foreach (var machine in machines)
        {
            var cost = machine.GetLowestCostForMachine();

            if (cost != -1)
                costs.Add(cost);
        }

        return costs.Sum();
    }

    private static long GetLowestCostForMachine(this ClawMachine machine)
    {
        decimal b = (decimal)(machine.ButtonA.Y * machine.PrizePosition.X - machine.ButtonA.X * machine.PrizePosition.Y) / (decimal)(machine.ButtonA.Y * machine.ButtonB.X - machine.ButtonA.X * machine.ButtonB.Y);
        decimal a = (decimal)(machine.PrizePosition.X - machine.ButtonB.X * b) / (decimal)machine.ButtonA.X;

        if (b % 1 == 0 && a % 1 == 0)
            return (long)(b + a * 3);

        return -1;
    }
}

public class ClawMachine
{
    public (long X, long Y) ButtonA { get; set; }
    public (long X, long Y) ButtonB { get; set; }
    public (long X, long Y) PrizePosition { get; set; }

    public void AugmentPrizePosition()
    {
        PrizePosition = new(PrizePosition.X + 10000000000000, PrizePosition.Y + 10000000000000);
    }

    public static implicit operator ClawMachine(List<string> input)
    {
        if (input.Count != 3)
            throw new ArgumentException("ClawMachine Input is three lines");

        string buttonA = input.First(x => x.StartsWith("Button A"));
        string buttonB = input.First(x => x.StartsWith("Button B"));
        string prize = input.First(x => x.StartsWith("Prize"));

        (long X, long Y) a = new(long.Parse(Regex.Match(buttonA, @"(?<=X\+)[0-9]+").Value), long.Parse(Regex.Match(buttonA, @"(?<=Y\+)[0-9]+").Value));
        (long X, long Y) b = new(long.Parse(Regex.Match(buttonB, @"(?<=X\+)[0-9]+").Value), long.Parse(Regex.Match(buttonB, @"(?<=Y\+)[0-9]+").Value));
        (long X, long Y) p = new(long.Parse(Regex.Match(prize, @"(?<=X=)[0-9]+").Value), long.Parse(Regex.Match(prize, @"(?<=Y=)[0-9]+").Value));

        return new() { ButtonA = a, ButtonB = b, PrizePosition = p };
    }
}
