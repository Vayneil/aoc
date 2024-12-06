using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;

namespace AdventOfCode.lib;

class Updater {
    public async Task Update(int year, int day) {
        Console.WriteLine("Starting update...");
        
        // creating context for scrape
        string session = getSession();
        Uri address = new("https://adventofcode.com");
        IBrowsingContext context = BrowsingContext.New(Configuration.Default
                .WithDefaultLoader()
                .WithDefaultCookies()
                .WithCss());
        context.SetCookie(new Url(address.ToString()), "session=" + session);
        
        // Fetching and saving
        IResponse input = await DownloadInput(context, address, year, day);
        CreateDirectory(year, day);
        SaveInput(input, year, day);
        CreateSolverTemplate(year, day);
    }

    private string getSession() {
        Env.Load(Path.Combine(Environment.CurrentDirectory, ".env"));
        return Env.GetVariable("session");
    }

    private async Task<IResponse> DownloadInput(IBrowsingContext context, Uri address, int year, int day) {
        Console.WriteLine("Starting download of problem input:");
        var loader = context.GetService<IDocumentLoader>();
        var input =  await loader!.FetchAsync(
            new DocumentRequest(new Url(address + $"{year}/day/{day}/input"))
        ).Task;

        if (input.StatusCode != HttpStatusCode.OK) {
            Console.WriteLine("Fetch failed");
            Console.Write("Status Code: ");
            Console.WriteLine(input.StatusCode);
            throw new Exception(message: "Failed to fetch input");
        }

        return input;
    }

    private void SaveInput(IResponse input, int year, int day) {
        string file = Path.Combine(Dir(year, day), "input.txt");
        File.WriteAllText(file, new StreamReader(input.Content).ReadToEnd());
    }

    private string Dir(int year, int day) {
        return Path.Combine(Environment.CurrentDirectory, $"{year}", $"{day}");
    }
    
    private void CreateDirectory(int year, int day) {
        string dir = Dir(year, day);
        Console.WriteLine("Creating directory at: " + dir);
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
    }

    private void CreateSolverTemplate(int year, int day) {
        string template = $@"namespace AdventOfCode.Year{year}.Day{day};

public static class Solver {{
    private readonly static string path = ""./{year}/{day}/input.txt"";

    public static List<int> Solution(){{
        List<int> results = [];

        return results;
    }}
}} 
";
        
        string file = Path.Combine(Dir(year, day), $"{day}.cs");
        File.WriteAllText(file, template);

    }
}