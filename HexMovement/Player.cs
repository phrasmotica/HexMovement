namespace HexMovement
{
    public class Player
    {
        public int PosX { get; set; }

        public int PosY { get; set; }

        public void MoveUp() => PosY -= 1;

        public void MoveRight() => PosX += 1;

        public void MoveDown() => PosY += 1;

        public void MoveLeft() => PosX -= 1;
    }
}
