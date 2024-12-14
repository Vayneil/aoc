using System.Text.RegularExpressions;
using System.Numerics;

namespace AdventOfCode.Year2024.Day7;

public static class Solver {
    private readonly static string path = "./2024/7/input.txt";

    public static List<object> Solution(){
        List<BigInteger> results = [];
        List<BigInteger> calibrationResults = [];
        List<List<BigInteger>> calibrationConstants = [];

        // match all numbers
        StreamReader reader = new(path);
        string? line;
        string pattern = @"(\d+)";

        while ((line = reader.ReadLine()) != null) {
            MatchCollection matches = Regex.Matches(line, pattern);
            calibrationResults.Add(BigInteger.Parse(matches[0].Value));
            calibrationConstants.Add(matches.Skip(1)
                                            .Select(x => BigInteger.Parse(x.Value))
                                            .ToList());
        }

        BigInteger validSum = 0;
        
        for (int i = 0; i < calibrationResults.Count; i++) {
            bool canCalibrate = TryOperate(calibrationResults[i], calibrationConstants[i]); 
            if (canCalibrate) {
                validSum += calibrationResults[i];
            } 
        }

        return [validSum];
    }
    private static bool TryOperate(BigInteger res, List<BigInteger> nums) { 
        return TryOperateRecursive(res, nums, 0, nums[0], []);
    } 
    
    private static bool TryOperateRecursive(BigInteger res, List<BigInteger> nums, int cur, BigInteger currentValue, Dictionary<string, bool> memo) { 
        string key = currentValue + "|" + cur; 
        if (memo.ContainsKey(key)) return memo[key]; 
        if (cur == nums.Count - 1) { 
            if (currentValue == res) return memo[key] = true; 
            return memo[key] = false; 
        }
        BigInteger nextValue = nums[cur + 1]; 
        // Multiplication
        if (TryOperateRecursive(res, nums, cur + 1, currentValue * nextValue, memo)) 
            return memo[key] = true;
        // Addition
        if (TryOperateRecursive(res, nums, cur + 1, currentValue + nextValue, memo)) 
            return memo[key] = true; 
        // Concatenation
        BigInteger concatValue = BigInteger.Parse(currentValue.ToString() + nextValue.ToString()); 
        if (TryOperateRecursive(res, nums, cur + 1, concatValue, memo))
            return memo[key] = true; 
        
        return memo[key] = false;
    }
}
