using System.Diagnostics;

namespace AdventOfCode.Year2024.Day5;

public static class Solver {
    private readonly static string path = "./2024/5/input.txt";

    public static List<int> Solution(){
        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        
        List<int> results = [];
        List<List<int>> orderList = new(100);
        foreach (int i in Enumerable.Range(0, 99)) orderList.Add([]);
        StreamReader reader = new(path);
        string? line;

        while ((line = reader.ReadLine()) != "") {
            int[] order = Array.ConvertAll(line.Split("|"), int.Parse);
            orderList[order[0]].Add(order[1]);
        }

        int middlePageSum = 0;
        int badMiddlePageSum = 0;
        ElvenMemeSorter elvenMemeSorter = new();
        elvenMemeSorter.SetArr(orderList);
        
        while((line = reader.ReadLine()) != null) {
            bool isAlreadyOrdered = true;
            List<int> update = [.. Array.ConvertAll(line.Split(","), int.Parse)];
            int middle = update.Count / 2;

            for (int page1 = 0; page1 < update.Count - 1; page1++) {
                for (int page2 = page1 + 1; page2 < update.Count; page2++) {
                    if (orderList[update[page2]].Contains(update[page1])) {
                        isAlreadyOrdered = false;
                        var sorted = update.ConvertAll(x => x);
                        sorted.Sort(0, update.Count, elvenMemeSorter);
                        badMiddlePageSum += sorted[middle];
                        break;
                    }
                }
                if (!isAlreadyOrdered) break;
            }
            if (isAlreadyOrdered) middlePageSum += update[middle];
        }

        results.Add(middlePageSum);
        results.Add(badMiddlePageSum);

        stopwatch.Stop();
        TimeSpan stopwatchElapsed = stopwatch.Elapsed;
        Console.Write("Time elapsed: ");
        Console.Write(stopwatchElapsed.TotalMilliseconds);
        Console.Write(" ms\n");

        return results;
    }
}

public class ElvenMemeSorter : IComparer<int> {
    
    public List<List<int>> arr = [];
    
    public void SetArr(List<List<int>> newArr) {
        arr = newArr;
    }

    public int Compare(int a, int b) {
        if (arr[a].Contains(b)) return -1;
        if (arr[b].Contains(a)) return 1;
        return 0;
    }
}
