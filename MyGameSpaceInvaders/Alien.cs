namespace MyGameSpaceInvaders
{
    public class Alien
    {
        public Alien(int x, int y, int ind)
        {
            X = x;
            Y = y;
            index = ind;
        }
        public bool Alive = true;
        public int X;
        public int Y;
        public int index;
        public void Move(int direction)
        {
            if (direction == 2)
                Y += 10;
            X += 5 * direction;
        }
    }
}
