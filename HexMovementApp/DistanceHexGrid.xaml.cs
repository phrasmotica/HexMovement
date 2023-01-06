using HexGrid;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HexMovementApp
{
    /// <summary>
    /// Interaction logic for DistanceHexGrid.xaml
    /// </summary>
    public partial class DistanceHexGrid : UserControl
    {
        private readonly HexGrid.HexGrid _hexGrid;

        private readonly List<List<Button>> _tiles;

        private int? _startX;
        private int? _startY;
        private int? _endX;
        private int? _endY;

        public event Action<int?, int?, int?, int?> OnChangeRoute;

        public event Action OnReset;

        public DistanceHexGrid()
        {
            InitializeComponent();

            var width = 14;
            var height = 6;

            _hexGrid = new(width, height);
            _tiles = new();

            OnChangeRoute += UpdateButtonStates;
            OnChangeRoute += UpdateDistance;

            OnReset += ResetButtons;
            OnReset += () => UpdateDistance(null, null, null, null);

            DrawGrid();
        }

        private void DrawGrid()
        {
            // remove the two example rows
            rowsPanel.Children.RemoveAt(0);
            rowsPanel.Children.RemoveAt(0);

            for (var y = 0; y < _hexGrid.Height; y++)
            {
                var rowTiles = new List<Button>();

                var rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(y % 2 != 0 ? 23 : 0, y > 0 ? 6 : 0, 0, 0),
                };

                // start rendering odd rows at X = 1
                var startX = y % 2 != 0 ? 1 : 0;

                for (var x = startX; x < _hexGrid.Width; x += 2)
                {
                    var button = new Button
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(x > startX ? 6 : 0, 0, 0, 0),
                        Content = $"({x}, {y})",
                    };

                    // capture values of x and y
                    var col = x;
                    var row = y;

                    button.Click += (sender, args) => SetRoute(col, row);

                    rowTiles.Add(button);
                    rowPanel.Children.Add(button);
                }

                _tiles.Add(rowTiles);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private void UpdateButtonStates(int? startX, int? startY, int? endX, int? endY)
        {
            for (var y = 0; y < _tiles.Count; y++)
            {
                var row = _tiles[y];

                for (var x = 0; x < row.Count; x++)
                {
                    var button = row[x];

                    button.IsEnabled = !((x == (startX / 2) && y == startY) || (x == (endX / 2) && y == endY));
                }
            }
        }

        private void UpdateDistance(int? startX, int? startY, int? endX, int? endY)
        {
            if (startX.HasValue && startY.HasValue && endX.HasValue && endY.HasValue)
            {
                var distance = HexGridDistance.ComputeDistanceDoubleWidth(startX.Value, startY.Value, endX.Value, endY.Value);
                distanceText.Content = $"Distance: {distance} tile(s)";
            }
            else
            {
                distanceText.Content = "Distance: ?";
            }
        }

        private void ResetButtons()
        {
            foreach (var row in _tiles)
            {
                foreach (var button in row)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private void SetRoute(int x, int y)
        {
            if (_startX is not null)
            {
                _endX = x;
                _endY = y;
            }
            else
            {
                _startX = x;
                _startY = y;
            }

            OnChangeRoute(_startX, _startY, _endX, _endY);
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            _startX = _startY = _endX = _endY = null;
            OnReset();
        }
    }
}
