namespace MyGameSpaceInvaders
{
    public class Bullet
    {
        public Bullet(int x, int y, int speed)
        {
            Speed = speed;
            X = x;
            Y = y;
        }
        public readonly int Speed;
        public bool isFly = false;
        public int X;
        public int Y;

        public void Fly(int direction)
        {
            // игрок стреляет 1; иначе -1;
            Y += Speed * direction * -1;
        }

        public void Clear()
        {
            X = -10;
            Y = -10;
        }
    }
}
