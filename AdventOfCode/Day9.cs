namespace AdventOfCode2024;

public static class Day9
{
    public static long GetFileSystemCheckSum(this string input) =>
        input.GetDriveFormat().FormatDrive().Select((x, i) => x == -1 ? 0 : x * i).Sum();

    private static List<long> GetDriveFormat(this string input)
    {
        List<long> result = [];

        bool isEmptySpace = false;
        int spaceId = 0;

        foreach (var c in input)
        {
            int count = int.Parse(c.ToString());

            foreach (var item in Enumerable.Range(0, count))
            {
                result.Add(isEmptySpace ? -1 : spaceId);
            }

            if (!isEmptySpace)
                spaceId++;

            isEmptySpace = !isEmptySpace;
        }

        return result;
    }

    private static List<long> FormatDrive(this List<long> drive)
    {
        List<long> format = new(drive);
        int i = 0;
        int j = format.Count - 1;
        List<long> idsMoved = [];

        while (j > 0)
        {
            //format.ForEach(x => Console.Write(x == -1 ? "." : x));
            //Console.WriteLine();

            while (j > 0 && format[j] == -1)
                j--;

            int fileIndex = j;
            var id = format[j];

            if (idsMoved.Contains(id))
            {
                j--;
                continue;
            }

            idsMoved.Add(id);

            while (fileIndex > 0 && format[fileIndex] == id)
                fileIndex--;

            if (fileIndex == 0)
                break;

            int fileSize = j - fileIndex;

            i = 0;
            int openSlot = 0;

            while (openSlot < fileSize)
            {
                while (i < fileIndex && format[i] != -1)
                    i++;

                openSlot = 0;

                if (i >= fileIndex)
                    break;

                while (i < fileIndex && format[i] == -1)
                {
                    i++;
                    openSlot++;
                }
            }

            if (openSlot < fileSize)
            {
                j = fileIndex;
                continue;
            }

            i -= openSlot;

            for (int k = 0; k < fileSize; k++)
            {
                format[i] = format[j];
                format[j] = -1;
                j--;
                i++;
            }
        }

        return format;
    }
}
