namespace AdventOfCode.Year2024.Day2;

public static class Solver {
    private readonly static StreamReader reader = new("./2024/2/input.txt");

    public static List<int> Solution() {
        List<int> result = [];
        string? report;
        int safe = 0;
        int safeRemoved = 0;

        while ((report = reader.ReadLine()) != null) {
            // Parse
            string[] levels = report.Split(" ");
            List<int> levelsNumeric = Array.ConvertAll(levels, int.Parse).ToList();
            List<List<int>> subreports = [levelsNumeric];

            // Create subreports for part 2
            // TODO: Write faster algorithm
            for (int j = 0; j < levelsNumeric.Count; j++) {
                List<int> subreport = levelsNumeric.ConvertAll(report => report);
                subreport.RemoveAt(j);
                subreports.Add(subreport);
            }

            // Check for normal reports
            bool isSafe = CheckConditions(levelsNumeric);
            if (isSafe) safe++;

            // Check for all subreports
            foreach (List<int> rep in subreports) {
                bool isSafeWhenRemoved = CheckConditions(rep);
                if (isSafeWhenRemoved) {
                    safeRemoved++;
                    break;
                }
            }
        }

        result.Add(safe);
        result.Add(safeRemoved);
        return result;
    }
    private static bool CheckConditions(List<int> report) {
        bool isSafe = false;

        // Create list of differences between reports
        var subtractions = report.Zip(
                report.Skip(1),
                (a, b) => a - b);

        var validSub = subtractions.Where(x => x == 0 || x > 3 || x < -3).Any();
        if (validSub) return false;

        // divide differences to check if they flip sign
        var signChecks = subtractions.Zip(
                subtractions.Skip(1),
                (a, b) => (double)a / b)
                .Where(x => x < 0.0)
                .Any();

        if (!signChecks) {
            return true;
        }

        return isSafe;
    }
}