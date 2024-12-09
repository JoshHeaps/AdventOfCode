namespace AdventOfCode2024;

public static class Day8
{
    public static int GetRequiredAntinodeCount(this string input)
    {
        var map = input.To2DCharArray();

        var towers = GetTowerLocationsByType(map);
        var antiNodes = GetAntinodeLocations(map, towers).Distinct().ToList();

        foreach ( var antiNode in antiNodes )
            Console.WriteLine($"{antiNode.x} {antiNode.y}");

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[j, i] == '.' && antiNodes.Any(x => x.x == j && x.y == i))
                    Console.Write("#");
                else
                    Console.Write(map[j, i].ToString());
            }

            Console.WriteLine();
        }

        return antiNodes.Count;
    }

    private static Dictionary<char, List<(int x, int y)>> GetTowerLocationsByType(char[,] towerMap)
    {
        Dictionary<char, List<(int x, int y)>> result = [];

        for (int i = 0; i < towerMap.GetLength(0); i++)
        {
            for (int j = 0; j < towerMap.GetLength(1); j++)
            {
                if (towerMap[j, i] == '.')
                    continue;

                if (!result.TryAdd(towerMap[j, i], [(j, i)]))
                    result[towerMap[j, i]].Add((j, i));
            }
        }

        return result;
    }

    private static List<(int x, int y)> GetAntinodeLocations(char[,] towerMap, Dictionary<char, List<(int x, int y)>> towers)
    {
        List<(int x, int y)> result = [];

        foreach (var (key, locations) in towers)
        {
            if (locations.Count < 2) continue;

            for (int i = 0; i < locations.Count; i++)
            {
                result.AddRange(GetAntinodeLocationsByPairs(towerMap, towers, key, i));
            }
        }

        return result;
    }

    private static List<(int x, int y)> GetAntinodeLocationsByPairs(char[,] towerMap, Dictionary<char, List<(int x, int y)>> towers, char key, int index)
    {
        List<(int x, int y)> result = [];
        var locations = towers[key];

        for (int i = 0; i < towers[key].Count; i++)
        {
            if (i == index) continue;

            int xDist = locations[i].x - locations[index].x;
            int yDist = locations[i].y - locations[index].y;

            for (int j = 0; locations[i].x + xDist * j >= 0 && locations[i].x + xDist * j < towerMap.GetLength(1)
                         && locations[i].y + yDist * j >= 0 && locations[i].y + yDist * j < towerMap.GetLength(0); j++)
            {
                result.Add((locations[i].x + xDist * j, locations[i].y + yDist * j));
            }

            for (int j = 0; locations[index].x - xDist * j >= 0 && locations[index].x - xDist * j < towerMap.GetLength(1)
                         && locations[index].y - yDist * j >= 0 && locations[index].y - yDist * j < towerMap.GetLength(0); j++)
            {
                result.Add((locations[index].x - xDist * j, locations[index].y - yDist * j));
            }
        }

        return result;
    }
}
