
using System.Drawing;

namespace WindowsFormsApp2
{
    abstract class GameObj
    {   
        private int x = 0;
        private int y = 0;
        public int GetX()
        {
            return x;
        }

        public void SetX(int value)
        {
            x = value;
        }

        public int GetY()
        {
            return y;
        }

        public void SetY(int value)
        {
            y = value;
        }

        public abstract int GetCode();

        public abstract void Draw(Graphics g, Image TextureSet, int i, int j);
    }
}
