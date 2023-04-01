namespace Logic
{
    public class Ball
    {
        private int x;
        private int y;
        private int velocityX;
        private int velocityY;

        public static readonly int radius = 5;

        public  Ball(int x, int y, int velocityX, int velocityY)
        {
            this.x = x;
            this.y = y;
            this.velocityX = velocityX;
            this.velocityY = velocityY;
        }

        public int getX() { return x; }
        public int getY() { return y; }
        public int getVelocityX() { return velocityX; }
        public int getVelocityY() { return velocityY; }

        public void setVelocityX(int v) { velocityX = v; }
        public void setVelocityY(int v) { velocityY = v; }

        public void move(int borderX, int borderY)
        {
            if ( (borderX >= (x + radius)) && (borderY >= (y + radius)) )
            {
                if ( (x + velocityX + radius) >= 0 && ((x + velocityX + radius) <= borderX) )
                {
                    x += velocityX;
                }
                else 
                {
                    if ( (x + velocityX + radius) >= 0 )
                    {
                        x = borderX - radius;
                    }
                    else
                    {
                        x = 0 + radius;
                    }
                        
                    velocityX *= -1;
                }

                if ( (y + velocityY + radius) >= 0 && ((y + velocityY + radius) <= borderY) )
                {
                    y += velocityY;
                }
                else
                {
                    if ( (y + velocityY + radius) >= 0 )
                    {
                        y = borderY - radius;
                    }
                    else
                    {
                        y = 0 + radius;
                    }

                    velocityY *= -1;
                }

            }
        }
    }
}