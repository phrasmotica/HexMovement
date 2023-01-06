// See https://aka.ms/new-console-template for more information
using HexGrid;
using HexGridConsole;

static void PrintHexGrid(HexGrid.HexGrid hexGrid, Player player)
{
    Console.WriteLine($"Player is at ({player.PosX}, {player.PosY})");
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

static string HexToString(HexGrid.Hex hex, Player player)
{
    return IsOccupied(hex.Row, hex.Col, player) ? "X" : ".";
}

static bool IsOccupied(int row, int col, Player player)
{
    return player.PosX == col && player.PosY == row;
}

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
var player = new Player();
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
        (player.PosX, player.PosY) = hexGrid.MoveRight(player.PosX, player.PosY);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "dr")
    {
        (player.PosX, player.PosY) = hexGrid.MoveDownRight(player.PosX, player.PosY);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "dl")
    {
        (player.PosX, player.PosY) = hexGrid.MoveDownLeft(player.PosX, player.PosY);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "l")
    {
        (player.PosX, player.PosY) = hexGrid.MoveLeft(player.PosX, player.PosY);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "ul")
    {
        (player.PosX, player.PosY) = hexGrid.MoveUpLeft(player.PosX, player.PosY);
        PrintHexGrid(hexGrid, player);
    }

    if (input == "ur")
    {
        (player.PosX, player.PosY) = hexGrid.MoveUpRight(player.PosX, player.PosY);
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
