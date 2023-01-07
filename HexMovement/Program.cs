// See https://aka.ms/new-console-template for more information
using HexGrid;
using HexGridConsole;

static void PrintHexGrid(HexGrid.HexGrid hexGrid, Player player)
{
    Console.WriteLine($"Player is at ({player.Hex!.Col}, {player.Hex!.Row})");
    Console.WriteLine(ToString(hexGrid, player));
    Console.WriteLine();
}

static string ToString(HexGrid.HexGrid hexGrid, Player player)
{
    return string.Join("\n", hexGrid.Rows.Select((r, i) => RowToString(r, i, player)));
}

static string RowToString(List<Hex> hexes, int row, Player player)
{
    var joined = string.Join(" ", hexes.Select(h => HexToString(h, player)));

    if (row % 2 != 0)
    {
        joined = " " + joined;
    }

    return joined;
}

static string HexToString(Hex hex, Player player) => IsOccupied(hex, player) ? "X" : ".";

static bool IsOccupied(Hex hex, Player player) => player.Hex == hex;

static void ToggleWrapMovement(HexGrid.HexGrid hexGrid)
{
    hexGrid.WrapMovement = !hexGrid.WrapMovement;

    Console.WriteLine($"Wrap movement is now {hexGrid.WrapMovement}");
    Console.WriteLine();
}

static void PrintHelp()
{
    Console.WriteLine("Move right: r");
    Console.WriteLine("Move down-right: dr");
    Console.WriteLine("Move down-left: dl");
    Console.WriteLine("Move left: l");
    Console.WriteLine("Move up-left: ul");
    Console.WriteLine("Move up-right: ur");
    Console.WriteLine("Toggle grid wrapping: wrap");
    Console.WriteLine("Quit: q");
    Console.WriteLine();
}

Console.WriteLine("HexGrid");
Console.WriteLine();

var hexGrid = new HexGrid.HexGrid(8, 6);

var player = new Player()
{
    Hex = hexGrid.HexAt(0, 0),
};

PrintHexGrid(hexGrid, player);

string? input;

do
{
    Console.Write("> ");

    input = Console.ReadLine()?.Trim();

    if (input == "help")
    {
        PrintHelp();
    }

    if (input == "r")
    {
        player.Hex = hexGrid.MoveRight(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "dr")
    {
        player.Hex = hexGrid.MoveDownRight(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "dl")
    {
        player.Hex = hexGrid.MoveDownLeft(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "l")
    {
        player.Hex = hexGrid.MoveLeft(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "ul")
    {
        player.Hex = hexGrid.MoveUpLeft(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "ur")
    {
        player.Hex = hexGrid.MoveUpRight(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "wrap")
    {
        ToggleWrapMovement(hexGrid);
    }

    if (input == "q")
    {
        break;
    }
} while (input != null);
