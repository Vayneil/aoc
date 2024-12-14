namespace AdventOfCode.Year2024.Day6;

public static class Solver {
    private readonly static string path = "./2024/6/input.txt";

    public static List<int> Solution(){
        List<int> results = [];
        List<string> input;
        List<List<char>> grid = [];

        char empty = '.';
        char visited = 'X';
        char wall = '#';

        List<(int Y, int X)> directions =   [(-1, -0), // up
                                            (0, 1), // right
                                            (1, 0), // down
                                            (0, -1)]; // left
        (int Y, int X) guardPos = (-1, -1);
        List<(int Y, int X)> potentialWalls = []; 
        
        // starting grid state
        input = [.. File.ReadAllLines(path)];
        int height = input.Count;
        int width = input[0].Length;

        for (int row = 0; row < width; row++) {
            grid.Add([]);
            foreach (char location in input[row]) {
                grid[row].Add(location);

                // find guard
                if (location == '^') {
                    guardPos.Y = row;
                    guardPos.X = grid[row].Count - 1;
                } 
            }
        }

        // save initial guard position for part 2
        (int Y, int X) initGuardPos = guardPos;
        
        // initial direction is up
        (int Y, int X) currentDir = directions[0];
        int dirIndex = 0;

        // visit initial location
        int visitedCount = 0;
        grid[guardPos.Y][guardPos.X] = visited;
        visitedCount++;

        // check next location
        (int Y, int X) next = (guardPos.Y + currentDir.Y, guardPos.X + currentDir.X);

        // if next not in bounds, end
        while (next.X >= 0 && next.X < width && next.Y >= 0 && next.Y < height) {
            // if wall, turn
            if (grid[next.Y][next.X] == '#') {
                dirIndex++;
                // if gone through all directions, repeat
                if (dirIndex >= directions.Count) dirIndex = 0;
                currentDir = directions[dirIndex];
            } else {
                // if not wall, go next
                guardPos = next;
                // if location not visited, visit
                if (grid[guardPos.Y][guardPos.X] == empty) {
                    grid[guardPos.Y][guardPos.X] = visited;
                    visitedCount++;
                    // walls can be put only on initial path guard takes
                    potentialWalls.Add(guardPos);
                }
            }
            next = (guardPos.Y + currentDir.Y, guardPos.X + currentDir.X);            
        }

        // loops counter
        int loopsCounter = 0;

        // check each potenial wall location
        foreach ((int Y, int X) in potentialWalls) {
            // reset simulation
            guardPos = initGuardPos;
            currentDir = directions[0];
            dirIndex = 0;
            next = (guardPos.Y + currentDir.Y, guardPos.X + currentDir.X);
            
            /* create a dictionary to check if location next to wall
            has been visited already in that direction
            if it was, its a loop */
            Dictionary<(int Y, int X, int dir), bool> wasVisited = [];
            bool canGuardLeave = true;

            // put wall
            grid[Y][X] = wall;

            // run simulation
            while (next.X >= 0 && next.X < width && next.Y >= 0 && next.Y < height) {
                if (grid[next.Y][next.X] == '#') {
                    dirIndex++;
                    if (dirIndex >= directions.Count) dirIndex = 0;
                    currentDir = directions[dirIndex];
                    
                    if (wasVisited.ContainsKey((guardPos.Y, guardPos.X, dirIndex))) {
                        canGuardLeave = false;
                        break;
                    }
                    wasVisited.Add((guardPos.Y, guardPos.X, dirIndex), true);
                } else {
                    guardPos = next;
                }
                next = (guardPos.Y + currentDir.Y, guardPos.X + currentDir.X);
            }
            grid[Y][X] = empty;
            if (!canGuardLeave) loopsCounter++; 
        }
        return [visitedCount, loopsCounter];
    }
}


