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
    /// Interaction logic for NeighboursHexGrid.xaml
    /// </summary>
    public partial class NeighboursHexGrid : UserControl
    {
        private const int GridWidth = 14;
        private const int GridHeight = 6;

        private const int ButtonSize = 40;
        private const int MarginSize = 6;

        private readonly HexGrid.HexGrid _hexGrid;

        private readonly List<List<Button>> _tileRows;

        private Hex? _hex;

        public event Action<Hex?> OnChangeHex;

        public event Action<bool> OnToggleWrapMovement;

        public NeighboursHexGrid()
        {
            InitializeComponent();

            _hexGrid = new(GridWidth, GridHeight);
            _tileRows = new();

            OnChangeHex += hex => _hex = hex;
            OnChangeHex += UpdateButtons;
            OnChangeHex += UpdateNeighbours;

            OnToggleWrapMovement += shouldWrap => _hexGrid.WrapMovement = shouldWrap;
            OnToggleWrapMovement += shouldWrap => UpdateButtons(_hex);
            OnToggleWrapMovement += shouldWrap => UpdateNeighbours(_hex);

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

                    button.Click += (sender, args) => OnChangeHex(hex);

                    row.Add(button);
                    rowPanel.Children.Add(button);
                }

                _tileRows.Add(row);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private void UpdateButtons(Hex? hex)
        {
            var neighbours = new List<Hex>();

            if (hex is not null)
            {
                neighbours = _hexGrid.GetNeighbours(hex);
            }

            for (var y = 0; y < _tileRows.Count; y++)
            {
                var row = _tileRows[y];

                for (var x = 0; x < row.Count; x++)
                {
                    var button = row[x];

                    button.IsEnabled = !(x == (hex?.Col / 2) && y == hex?.Row);

                    // highlight neighbours
                    if (neighbours.Any(h => x == (h.Col / 2) && y == h.Row))
                    {
                        button.Background = new SolidColorBrush(Colors.AliceBlue);
                    }
                    else
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                    }
                }
            }
        }

        private void UpdateNeighbours(Hex? hex)
        {
            if (hex is not null)
            {
                var neighbours = _hexGrid.GetNeighbours(hex);
                neighboursText.Content = $"# of neighbours: {neighbours.Count}";
            }
            else
            {
                neighboursText.Content = "# of neighbours: ?";
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e) => OnChangeHex(null);

        private void SetWrapOn(object sender, RoutedEventArgs e) => OnToggleWrapMovement(true);

        private void SetWrapOff(object sender, RoutedEventArgs e) => OnToggleWrapMovement(false);
    }
}
