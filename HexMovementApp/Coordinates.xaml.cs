using HexGrid;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HexMovementApp
{
    /// <summary>
    /// Interaction logic for Coordinates.xaml
    /// </summary>
    public partial class Coordinates : UserControl
    {
        private const int GridWidth = 14;
        private const int GridHeight = 6;

        private const int CellSize = 40;
        private const int MarginSize = 6;

        private readonly IHexGrid _hexGrid;

        private readonly List<List<Button>> _tileRows;

        private IHex? _hex;
        private CoordinateSystem _coordinateSystem;

        public event Action<CoordinateSystem> OnSetCoordinateSystem;

        public Coordinates()
        {
            InitializeComponent();

            _hexGrid = new DoubleWidthHexGrid(GridWidth, GridHeight);
            _tileRows = new();

            OnSetCoordinateSystem += system =>
            {
                UpdateSystem(system);
                UpdateButtons(_hex);
            };

            DrawGrid();
        }

        private void DrawGrid()
        {
            // remove the two example rows
            rowsPanel.Children.RemoveAt(rowsPanel.Children.Count - 1);
            rowsPanel.Children.RemoveAt(rowsPanel.Children.Count - 1);

            for (var y = 0; y < _hexGrid.Rows.Count; y++)
            {
                var hexes = _hexGrid.Rows[y];

                var row = new List<Button>();

                var rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(y % 2 != 0 ? (CellSize + MarginSize) / 2 : 0, y > 0 ? MarginSize : 0, 0, 0),
                };

                for (var x = 0; x < hexes.Count; x++)
                {
                    var hex = hexes[x];

                    var button = new Button
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Margin = new Thickness(x > 0 ? MarginSize : 0, 0, 0, 0),
                    };

                    button.Click += (sender, args) =>
                    {
                        _hex = hex;

                        UpdateCoordinates(hex);
                        UpdateButtons(hex);
                    };

                    row.Add(button);
                    rowPanel.Children.Add(button);
                }

                _tileRows.Add(row);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private void UpdateSystem(CoordinateSystem system)
        {
            _coordinateSystem = system;

            doubleWidthText.Visibility = system == CoordinateSystem.DoubleWidth ? Visibility.Visible : Visibility.Collapsed;
            axialText.Visibility = system == CoordinateSystem.AxialCube ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateCoordinates(IHex? hex)
        {
            if (hex is not null)
            {
                doubleWidthText.Content = $"Double-width: ({hex.Col}, {hex.Row})";
                axialText.Content = $"Axial/cube: ({hex.Q}, {hex.R}, {hex.S})";
            }
            else
            {
                doubleWidthText.Content = $"Double-width: (?, ?)";
                axialText.Content = $"Axial/cube: (?, ?, ?)";
            }
        }

        private void UpdateButtons(IHex? hex)
        {
            for (var y = 0; y < _tileRows.Count; y++)
            {
                var row = _tileRows[y];

                for (var x = 0; x < row.Count; x++)
                {
                    var button = row[x];

                    button.IsEnabled = !(x == (hex?.Col / 2) && y == hex?.Row);

                    // highlight rows and columns
                    if (ShouldHighlight(hex, y, x))
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

        private bool ShouldHighlight(IHex? hex, int row, int col) => _coordinateSystem switch
        {
            CoordinateSystem.DoubleWidth => IsSameRowOrCol(hex, row, col),
            CoordinateSystem.AxialCube => IsSameAxis(hex, row, col),
            _ => false,
        };

        private bool IsSameRowOrCol(IHex? hex, int row, int col)
        {
            var other = _hexGrid.Rows[row][col];
            return other.Col == hex?.Col || other.Row == hex?.Row;
        }

        private bool IsSameAxis(IHex? hex, int row, int col)
        {
            var other = _hexGrid.Rows[row][col];
            return other.Q == hex?.Q || other.R == hex?.R || other.S == hex?.S;
        }

        private void DoubleWidthButton_Click(object sender, RoutedEventArgs e) => OnSetCoordinateSystem(CoordinateSystem.DoubleWidth);

        private void AxialButton_Click(object sender, RoutedEventArgs e) => OnSetCoordinateSystem(CoordinateSystem.AxialCube);
    }

    public enum CoordinateSystem
    {
        DoubleWidth,
        AxialCube,
    }
}
