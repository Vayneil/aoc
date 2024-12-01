namespace AdventOfCode.Year2024.Day1;

public static class Solver {
    private readonly static StreamReader reader = new("./2024/1/input.txt");
    public static List<int> Solution() {
        string? line;
        List<int> result = [];
        List<int> leftList = [];
        List<int> rightList = [];

        while ((line = reader.ReadLine()) != null) {
            string[] locations = line.Split("   ");
            leftList.Add(int.Parse(locations[0]));
            rightList.Add(int.Parse(locations[1]));
        }

        leftList.Sort();
        rightList.Sort();

        List<int> subtractedList = leftList.Zip(rightList, (a, b) => Math.Abs(a - b)).ToList();
        result.Add(subtractedList.Aggregate((acc, x) => acc + x));

        Dictionary<int, int> freq = [];
        foreach (var item in rightList) {
            if (freq.TryGetValue(item, out int value)) {
                freq[item]++;
            } else {
                freq[item] = 1;
            }
        }

        int sum = 0;
        foreach (var item in leftList) {
            if (freq.TryGetValue(item, out int value)) {
                sum += value*item;
            }
        }

        result.Add(sum);
        return result;
    }
}