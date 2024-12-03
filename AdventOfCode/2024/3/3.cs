using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day3;

public static class Solver {
    private readonly static string path = "./2024/3/input.txt";

    public static List<int> Solution() {
        List<int> result = [];
        int mulResult = 0;
        string mulPattern = @"(mul\(\d{1,3},\d{1,3}\))|(do\(\))|(don\'t\(\))";
        string numberPattern = @"\d{1,3}";
        string input;

        if (File.Exists(path)) {
            input = File.ReadAllText(path);
        } else throw new Exception(message: "Input file does not exist");

        bool enabled = true;

        foreach (Match match in Regex.Matches(input, mulPattern)) {
            int mulBase = 1;
            switch (match.Value) {
                case "do()":
                    enabled = true;
                    break;
                case "don't()":
                    enabled = false;
                    break;
                default:
                    if (enabled) {
                        MatchCollection numbers = Regex.Matches(match.Value, numberPattern);
                        foreach (Match number in numbers) {
                            mulBase *= int.Parse(number.Value);
                        }
                        mulResult += mulBase;
                    }
                    break;
            }
        }

        result.Add(mulResult);
        return result;
    }
}