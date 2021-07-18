namespace MyGameSpaceInvaders
{
    class Player
    {
        public Player(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int Health = 3;
        public int X;
        public int Y;

        public void Move(int direction)
        {
            X += 5 * direction;
        }
    }
}
