// See https://aka.ms/new-console-template for more information
using HexMovement;

static void PrintHexGrid(HexGrid hexGrid)
{
    Console.WriteLine($"Player is at ({hexGrid.Player.PosX}, {hexGrid.Player.PosY})");
    Console.WriteLine(hexGrid.ToString());
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
    Console.WriteLine("Quit: q");
    Console.WriteLine();
}

Console.WriteLine("HexGrid");
Console.WriteLine();

var hexGrid = new HexGrid(8, 6);
PrintHexGrid(hexGrid);

hexGrid.OnMove += player => PrintHexGrid(hexGrid);

hexGrid.OnMoveFail += player =>
{
    Console.WriteLine("Player cannot move there!");
    Console.WriteLine();

    PrintHexGrid(hexGrid);
};

string? input;

do
{
    input = Console.ReadLine()?.Trim();

    if (input == "help")
    {
        PrintHelp();
    }

    if (input == "r")
    {
        hexGrid.MoveRight();
    }

    if (input == "dr")
    {
        hexGrid.MoveDownRight();
    }

    if (input == "dl")
    {
        hexGrid.MoveDownLeft();
    }

    if (input == "l")
    {
        hexGrid.MoveLeft();
    }

    if (input == "ul")
    {
        hexGrid.MoveUpLeft();
    }

    if (input == "ur")
    {
        hexGrid.MoveUpRight();
    }

    if (input == "q")
    {
        break;
    }
} while (input != null);
