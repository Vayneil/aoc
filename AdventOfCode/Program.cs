using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode.lib;

Action? ParseAction(string[] args, string[] regexes, Func<string[], Action> parse) {
    // Check if regexes match
    if (args.Length != regexes.Length) {
        return null;
    }

    var matches = Enumerable.Zip(args, regexes, (arg, regex) => 
            new Regex("^" + regex + "$").Match(arg));
    
    if (!matches.All(match => match.Success)) {
        return null;
    }

    // parsing matches values to array of matches
    try {
        return parse(matches.SelectMany(m =>
                m.Groups.Count > 1 ? m.Groups
                        .Cast<Group>()
                        // m.Groups[0] same as m.Value
                        .Skip(1)
                        .Select(g => g.Value)
                                   : [m.Value]
            ).ToArray());
    } catch {
        return null;
    }
}

void DisplayResults(List<int>? results) {
    if (results is null) {
        throw new Exception(message: "Results are null");
    }

    if (results.Count > 2) {
        throw new Exception(message: "Too many elements in result list");
    }

    Console.WriteLine("Results are:");
    Console.WriteLine("Part 1: " + results[0].ToString());

    if (results.Count == 2) {
        Console.WriteLine("Part 2: " + results[1].ToString());
    }
}

void RunSolver(int year, int day) {
    // Find solver for the date
    string typeName = $"AdventOfCode.Year{year}.Day{day}.Solver";
    Type? type = Type.GetType(typeName);

    // Invoke Solution method and display results
    if (type is not null) {
        MethodInfo? method = type.GetMethod("Solution") ?? 
                throw new Exception(message: "No Solution method found");
        List<int>? results = method.Invoke(null, null) as List<int>;
        DisplayResults(results);
        return;
    }
    throw new Exception(message: "Class name not found");
}

string ShowHelp() {
    return $"""
            How to use: dotnet run [args]
            Available arguments:
            - To run solutions
            solve [year]/[day] (WIP)                Solves the problem if year and day is valid,
                                                    and appropriate solver is found.
            solve today (WIP)                       Same as above, but for this day

            - To fetch inputs: You need to be logged in to AoC site and set cookie in .env file
            as follows:
            session=XXXXXXXXXXXXXXXXXXXXXXXXXXX
            update [year]/[day] (WIP)               Fetches the problem and creates necessary
                                                    directories/classes
            update today (WIP)                      Same as above, but for this day
            """;
}

var parseParameters =
    ParseAction(args, ["update", "([0-9]+)/([0-9]+)"], m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return () => new Updater().Update(year, day).Wait();
    }) ??
    ParseAction(args, ["solve", "([0-9]+)/([0-9]+)"], m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return () => RunSolver(year, day);
    }) ??
    new Action(() => {
        Console.WriteLine("Couldn't recognize arguments.\n");
        Console.Write(ShowHelp());
    });

parseParameters();

