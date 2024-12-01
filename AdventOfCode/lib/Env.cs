namespace AdventOfCode.lib;

public static class Env {
    
    private static Dictionary<string, string> Variables = [];
    
    public static void Load(string path) {
        if (!File.Exists(path)) {
            return;
        }
        foreach (string line in File.ReadAllLines(path)) {
            string[] pair = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
            Variables.Add(pair[0], pair[1]);
        }
    }

    public static string GetVariable(string name) {
        if (!Variables.TryGetValue(name, out string? value))
        {
            return "";
        }
        return Variables[name];
    }
}