namespace Logic
{
    public class Ball
    {
        private Random random = new Random();
        private int x;
        private int y;

        public  Ball(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int GetX() { return x; }
        public int GetY() { return y; }

        private void Move()
        {
            x += random.Next(21) - 10;
            y += random.Next(21) - 10;
        }
    }
}