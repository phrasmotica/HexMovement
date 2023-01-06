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
            _player = new();
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
                    var cell = new Ellipse
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = new SolidColorBrush(IsOccupied(x, y) ? Colors.Crimson : Colors.AliceBlue),
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

        private bool IsOccupied(int row, int col)
        {
            return _player.PosX == row && _player.PosY == col;
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveLeft(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ButtonUpLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveUpLeft(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ButtonUpRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveUpRight(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveRight(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ButtonDownRight_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveDownRight(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ButtonDownLeft_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentCell();
            (_player.PosX, _player.PosY) = _hexGrid.MoveDownLeft(_player.PosX, _player.PosY);
            OnMove(_player);
        }

        private void ClearCurrentCell()
        {
            var currentCell = _tileRows[_player.PosY][_player.PosX / 2];
            currentCell.Fill = new SolidColorBrush(Colors.AliceBlue);
        }

        private void FillCurrentCell()
        {
            var currentCell = _tileRows[_player.PosY][_player.PosX / 2];
            currentCell.Fill = new SolidColorBrush(Colors.Crimson);
        }

        private void UpdatePlayerPositionLabel()
        {
            positionText.Content = $"Player position: ({_player.PosX}, {_player.PosY})";
        }

        private void UpdateButtonStates()
        {
            buttonRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveRight(_player.PosX, true);
            buttonDownRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveDownRight(_player.PosX, _player.PosY);
            buttonDownLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveDownLeft(_player.PosX, _player.PosY);
            buttonLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveLeft(_player.PosX, true);
            buttonUpLeft.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveUpLeft(_player.PosX, _player.PosY);
            buttonUpRight.IsEnabled = _hexGrid.WrapMovement || _hexGrid.CanMoveUpRight(_player.PosX, _player.PosY);
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
