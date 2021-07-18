namespace MyGameSpaceInvaders
{
    class Wall
    {
        public Wall(int x, int y)
        {
            X = x;
            Y = y;
            rX = x + 40;
            dY = y + 8;
        }
        public int health = 10;
        public int X;
        public int Y;
        public int rX;
        public int dY;
    }
}
