namespace AdventOfCode.Year2024.Day4;

public static class Solver {
    private readonly static string path = "./2024/4/input.txt";

    public static List<int> Solution(){
        List<int> result = [];
        List<string> input = [];
        string? line;
        string xmas = "XMAS";
        List<int> coordsDim = [-1, 0, 1];
        List<(int, int)> coords = [];
        foreach (int coordY in coordsDim) {
            foreach (int coordX in coordsDim) {
                coords.Add((coordY, coordX));
            }
        }
        coords.Remove((0, 0));
        List<List<(int, int)>> xCoords = [[(-1, -1), (1, 1)], [(1, -1), (-1, 1)]];
        int xmasCount = 0;
        int xCount = 0;

        StreamReader reader = new(path);

        while ((line = reader.ReadLine()) != null) {
            input.Add(line);
        }

        for (int rInd = 0; rInd < input.Count; rInd++) {
            for (int cInd = 0; cInd < input[rInd].Length; cInd++) {
                bool isXmas = true;
                bool isX = true;
                if (!(input[rInd][cInd] == 'X')) {
                    isXmas = false;
                } else {
                    foreach ((int, int) coord in coords) {
                        isXmas = true;
                        for (int lInd = 1; lInd < xmas.Length; lInd++) {
                            int y = rInd + lInd*coord.Item1;
                            int x = cInd + lInd*coord.Item2;

                            if (IsInBounds(x, y, input)) {
                                if (!(input[y][x] == xmas[lInd])) {
                                    isXmas = false;
                                    break;
                                }
                            } else {
                                isXmas = false;
                                break;
                            }
                        }
                        if (isXmas) {
                            xmasCount++;
                            Console.WriteLine($"Coordinates: [{rInd}, {cInd}]");
                            Console.WriteLine($"Direction: {coord}\n");
                        }
                    }
                }
                if (!(input[rInd][cInd] == 'A')) {
                    isX = false;
                } else {
                    foreach (List<(int, int)> coord in xCoords) {
                        int y1 = rInd + coord[0].Item1;
                        int x1 = cInd + coord[0].Item2;

                        int y2 = rInd + coord[1].Item1;
                        int x2 = cInd + coord[1].Item2;

                        if (IsInBounds(y1, x1, input) && IsInBounds(y2, x2, input)) {
                            if (!((input[y1][x1] == 'M' || input[y1][x1] == 'S')
                            && (input[y2][x2] == 'M' || input[y2][x2] == 'S')
                            && (input[y1][x1] != input[y2][x2]))) {
                                isX = false;
                                break;
                            }
                        } else {
                            isX = false;
                            break;
                        }
                    }
                    if (isX) xCount++;
                }
            }
        }
        result.Add(xmasCount);
        result.Add(xCount);
        return result;
    }

    private static bool IsInBounds(int x, int y, List<string> input) {
        int dimX = input.Count;
        int dimY = input[0].Length;
        if (y >= 0 && y < dimY
                && x >= 0 && x < dimX) {
            return true;
        }
        return false;
    }
}