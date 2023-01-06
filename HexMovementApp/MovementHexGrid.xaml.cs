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
        private readonly HexGrid.HexGrid _hexGrid;
        private readonly Player _player;

        private readonly List<List<Rectangle>> _tiles;

        public event Action<Player> OnMove;

        public event Action<bool> OnToggleWrapMovement;

        public MovementHexGrid()
        {
            InitializeComponent();

            var width = 14;
            var height = 6;

            _hexGrid = new(width, height);
            _player = new();
            _tiles = new();

            // cannot wrap with an odd number of rows
            wrapCheckbox.IsEnabled = height % 2 == 0;

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

            for (var y = 0; y < _hexGrid.Height; y++)
            {
                var rowTiles = new List<Rectangle>();

                var rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(y % 2 != 0 ? 23 : 0, y > 0 ? 6 : 0, 0, 0),
                };

                // start rendering odd rows at X = 1
                var startX = y % 2 != 0 ? 1 : 0;

                for (var x = startX; x < _hexGrid.Width; x += 2)
                {
                    var cell = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = new SolidColorBrush(IsOccupied(x, y) ? Colors.Crimson : Colors.AliceBlue),
                        Stroke = new SolidColorBrush(Colors.Black),
                        Margin = new Thickness(x > startX ? 6 : 0, 0, 0, 0),
                    };

                    rowTiles.Add(cell);
                    rowPanel.Children.Add(cell);
                }

                _tiles.Add(rowTiles);
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
            var currentCell = _tiles[_player.PosY][_player.PosX / 2];
            currentCell.Fill = new SolidColorBrush(Colors.AliceBlue);
        }

        private void FillCurrentCell()
        {
            var currentCell = _tiles[_player.PosY][_player.PosX / 2];
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
