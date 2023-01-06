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
        private const int GridWidth = 14;
        private const int GridHeight = 6;

        private const int ButtonSize = 40;
        private const int MarginSize = 6;

        private readonly HexGrid.HexGrid _hexGrid;

        private readonly List<List<Button>> _tileRows;

        private int? _startX;
        private int? _startY;
        private int? _endX;
        private int? _endY;

        public event Action<int?, int?, int?, int?> OnChangeRoute;

        public event Action OnReset;

        public DistanceHexGrid()
        {
            InitializeComponent();

            _hexGrid = new(GridWidth, GridHeight);
            _tileRows = new();

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
                    };

                    button.Click += (sender, args) => SetRoute(hex.Col, hex.Row);

                    row.Add(button);
                    rowPanel.Children.Add(button);
                }

                _tileRows.Add(row);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private void UpdateButtonStates(int? startX, int? startY, int? endX, int? endY)
        {
            for (var y = 0; y < _tileRows.Count; y++)
            {
                var row = _tileRows[y];

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
            foreach (var row in _tileRows)
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
