using HexGrid;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HexMovementApp
{
    /// <summary>
    /// Interaction logic for MovementHexGrid.xaml
    /// </summary>
    public partial class MovementHexGrid : UserControl
    {
        private const int GridWidth = 14;
        private const int GridHeight = 6;

        private const int CellSize = 40;
        private const int MarginSize = 6;

        private readonly HexGrid.HexGrid _hexGrid;

        private readonly Player _player;

        private readonly List<List<Ellipse>> _tileRows;

        public event Action<Player> OnMove;

        public event Action<bool> OnToggleWrapMovement;

        public MovementHexGrid()
        {
            InitializeComponent();

            _hexGrid = new(GridWidth, GridHeight);
            
            _player = new()
            {
                Hex = _hexGrid.HexAt(0, 0),
            };

            _tileRows = new();

            // cannot wrap with an odd number of rows
            wrapCheckbox.IsEnabled = GridHeight % 2 == 0;

            OnMove += player =>
            {
                FillCurrentCell();
                UpdatePlayerPositionLabel();
                UpdateButtonStates();
            };

            OnToggleWrapMovement += shouldWrap => UpdateButtonStates();

            DrawGrid();
            UpdateButtonStates();
        }

        private void DrawGrid()
        {
            // remove the two example rows
            rowsPanel.Children.RemoveAt(rowsPanel.Children.Count - 1);
            rowsPanel.Children.RemoveAt(rowsPanel.Children.Count - 1);

            for (var y = 0; y < _hexGrid.Rows.Count; y++)
            {
                var hexes = _hexGrid.Rows[y];

                var row = new List<Ellipse>();

                var rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(y % 2 != 0 ? (CellSize + MarginSize) / 2 : 0, y > 0 ? MarginSize : 0, 0, 0),
                };

                for (var x = 0; x < hexes.Count; x++)
                {
                    var hex = hexes[x];

                    var cell = new Ellipse
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = new SolidColorBrush(IsOccupied(hex) ? Colors.Crimson : Colors.AliceBlue),
                        Stroke = new SolidColorBrush(Colors.Black),
                        Margin = new Thickness(x > 0 ? MarginSize : 0, 0, 0, 0),
                    };

                    row.Add(cell);
                    rowPanel.Children.Add(cell);
                }

                _tileRows.Add(row);
                rowsPanel.Children.Add(rowPanel);
            }
        }

        private bool IsOccupied(Hex hex) => _player.Hex == hex;

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveLeft(_player.Hex!);
            OnMove(_player);
        }

        private void ButtonUpLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveUpLeft(_player.Hex!);
            OnMove(_player);
        }

        private void ButtonUpRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveUpRight(_player.Hex!);
            OnMove(_player);
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveRight(_player.Hex!);
            OnMove(_player);
        }

        private void ButtonDownRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveDownRight(_player.Hex!);
            OnMove(_player);
        }

        private void ButtonDownLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            _player.Hex = _hexGrid.MoveDownLeft(_player.Hex!);
            OnMove(_player);
        }

        private void ClearCurrentCell()
        {
            var currentCell = _tileRows[_player.Hex!.Row][_player.Hex!.Col / 2];
            currentCell.Fill = new SolidColorBrush(Colors.AliceBlue);
        }

        private void FillCurrentCell()
        {
            var currentCell = _tileRows[_player.Hex!.Row][_player.Hex!.Col / 2];
            currentCell.Fill = new SolidColorBrush(Colors.Crimson);
        }

        private void UpdatePlayerPositionLabel()
        {
            positionText.Content = $"Player position: ({_player.Hex!.Col}, {_player.Hex!.Row})";
        }

        private void UpdateButtonStates()
        {
            buttonRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveRight(_player.Hex!, true);
            buttonDownRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveDownRight(_player.Hex!);
            buttonDownLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveDownLeft(_player.Hex!);
            buttonLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveLeft(_player.Hex!, true);
            buttonUpLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveUpLeft(_player.Hex!);
            buttonUpRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveUpRight(_player.Hex!);
        }

        private void SetWrapOn(object sender, RoutedEventArgs e)
        {
            _hexGrid.WrapMovement = true;
            OnToggleWrapMovement(true);
        }

        private void SetWrapOff(object sender, RoutedEventArgs e)
        {
            _hexGrid.WrapMovement = false;
            OnToggleWrapMovement(false);
        }
    }
}
