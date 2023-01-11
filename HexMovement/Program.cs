// See https://aka.ms/new-console-template for more information
using HexGrid;
using HexGridConsole;

static void PrintHexGrid(IHexGrid hexGrid, Player player)
{
    Console.WriteLine($"Player is at ({player.Hex!.Col}, {player.Hex!.Row})");
    Console.WriteLine(ToString(hexGrid, player));
    Console.WriteLine();
}

static string ToString(IHexGrid hexGrid, Player player)
{
    return string.Join("\n", hexGrid.Rows.Select((r, i) => RowToString(r, i, player)));
}

static string RowToString(List<IHex> hexes, int row, Player player)
{
    var joined = string.Join(" ", hexes.Select(h => HexToString(h, player)));

    if (row % 2 != 0)
    {
        joined = " " + joined;
    }

    return joined;
}

static string HexToString(IHex hex, Player player) => IsOccupied(hex, player) ? "X" : ".";

static bool IsOccupied(IHex hex, Player player) => player.Hex == hex;

static void ToggleWrapMovement(IHexGrid hexGrid)
{
    hexGrid.WrapMovement = !hexGrid.WrapMovement;

    Console.WriteLine($"Wrap movement is now {hexGrid.WrapMovement}");
    Console.WriteLine();
}

static void PrintHelp()
{
    Console.WriteLine("Move east: e");
    Console.WriteLine("Move south-east: se");
    Console.WriteLine("Move south-west: sw");
    Console.WriteLine("Move west: w");
    Console.WriteLine("Move north-west: nw");
    Console.WriteLine("Move north-east: ne");
    Console.WriteLine("Toggle grid wrapping: wrap");
    Console.WriteLine("Quit: q");
    Console.WriteLine();
}

Console.WriteLine("HexGrid");
Console.WriteLine();

var hexGrid = new DoubleWidthHexGrid(8, 6);

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

    if (input == "e")
    {
        player.Hex = hexGrid.MoveEast(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "se")
    {
        player.Hex = hexGrid.MoveSouthEast(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "sw")
    {
        player.Hex = hexGrid.MoveSouthWest(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "w")
    {
        player.Hex = hexGrid.MoveWest(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "nw")
    {
        player.Hex = hexGrid.MoveNorthWest(player.Hex!);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "ne")
    {
        player.Hex = hexGrid.MoveNorthEast(player.Hex!);
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
