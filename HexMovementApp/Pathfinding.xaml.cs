using HexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HexMovementApp
{
    /// <summary>
    /// Interaction logic for Pathfinding.xaml
    /// </summary>
    public partial class Pathfinding : UserControl
    {
        private const int GridWidth = 14;
        private const int GridHeight = 6;

        private const int ButtonSize = 40;
        private const int MarginSize = 6;

        private readonly HexGrid.HexGrid _hexGrid;

        private readonly List<List<Button>> _tileRows;

        private Hex? _start;
        private Hex? _end;
        private bool _setEnd;

        public event Action<Hex?, Hex?> OnChangeRoute;

        public event Action<bool> OnToggleWrapMovement;

        public event Action OnReset;

        public Pathfinding()
        {
            InitializeComponent();

            _hexGrid = new(GridWidth, GridHeight);
            _tileRows = new();

            OnChangeRoute += UpdateButtonStates;
            OnChangeRoute += UpdatePath;

            OnToggleWrapMovement += shouldWrap => _hexGrid.WrapMovement = shouldWrap;
            OnToggleWrapMovement += shouldWrap => UpdateButtonStates(_start, _end);
            OnToggleWrapMovement += shouldWrap => UpdatePath(_start, _end);

            OnReset += ResetButtons;
            OnReset += () => UpdatePath(null, null);

            DrawGrid();
        }

        private void DrawGrid()
        {
            // remove the two example rows
            rowsPanel.Children.RemoveAt(0);
            rowsPanel.Children.RemoveAt(0);

            for (var y = 0; y < _hexGrid.Rows.Count; y++)
            {
                var hexes = _hexGrid.Rows[y];

                var row = new List<Button>();

                var rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(y % 2 != 0 ? (ButtonSize + MarginSize) / 2 : 0, y > 0 ? MarginSize : 0, 0, 0),
                };

                for (var x = 0; x < hexes.Count; x++)
                {
                    var hex = hexes[x];

                    var button = new Button
                    {
                        Width = ButtonSize,
                        Height = ButtonSize,
                        Margin = new Thickness(x > 0 ? MarginSize : 0, 0, 0, 0),
                        Content = $"({hex.Col}, {hex.Row})",
                        Background = new SolidColorBrush(GetColour(hex)),
                    };

                    button.Click += (sender, args) => SetRoute(hex);

                    row.Add(button);
                    rowPanel.Children.Add(button);
                }

                _tileRows.Add(row);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private static Color GetColour(Hex hex) => hex.Terrain == Terrain.Hill ? Colors.Yellow : Colors.Green;

        private void UpdateButtonStates(Hex? start, Hex? end)
        {
            for (var y = 0; y < _tileRows.Count; y++)
            {
                var row = _tileRows[y];

                for (var x = 0; x < row.Count; x++)
                {
                    var button = row[x];

                    button.IsEnabled = !((x == (start?.Col / 2) && y == start?.Row) || (x == (end?.Col / 2) && y == end?.Row));
                }
            }
        }

        private void UpdatePath(Hex? start, Hex? end)
        {
            if (start is not null && end is not null)
            {
                var path = HexGridPath.ComputeWrappedPath(_hexGrid, start, end);
                distanceText.Content = $"Distance: {path.Count} tile(s)";
                costText.Content = $"Cost: {HexGridPath.ComputeCost(path.Prepend(start).ToList())} move(s)";

                for (var y = 0; y < _tileRows.Count; y++)
                {
                    var row = _tileRows[y];

                    for (var x = 0; x < row.Count; x++)
                    {
                        var button = row[x];
                        var hex = _hexGrid.Rows[y][x];

                        // highlight hexes in the path
                        var index = path.FindIndex(0, path.Count, h => x == (h.Col / 2) && y == h.Row);
                        if (index > -1)
                        {
                            button.Background = new SolidColorBrush(Colors.AliceBlue);
                            button.Content = $"({hex.Col}, {hex.Row})\n[{index + 1}]";
                        }
                        else
                        {
                            button.Background = new SolidColorBrush(GetColour(hex));
                            button.Content = $"({hex.Col}, {hex.Row})";
                        }
                    }
                }
            }
            else
            {
                distanceText.Content = "Distance: ? tile(s)";
            }
        }

        private void ResetButtons()
        {
            for (var y = 0; y < _tileRows.Count; y++)
            {
                var row = _tileRows[y];

                for (var x = 0; x < row.Count; x++)
                {
                    var button = row[x];
                    var hex = _hexGrid.Rows[y][x];

                    button.IsEnabled = true;
                    button.Background = new SolidColorBrush(GetColour(hex));
                    button.Content = $"({hex.Col}, {hex.Row})";
                }
            }
        }

        private void SetRoute(Hex hex)
        {
            if (_setEnd)
            {
                _end = hex;
                _setEnd = false;
            }
            else
            {
                _start = hex;
                _setEnd = true;
            }

            OnChangeRoute(_start, _end);
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            _start = _end = null;
            OnReset();
        }

        private void SetWrapOn(object sender, RoutedEventArgs e) => OnToggleWrapMovement(true);

        private void SetWrapOff(object sender, RoutedEventArgs e) => OnToggleWrapMovement(false);
    }
}
