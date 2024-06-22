using System.Drawing;

namespace WindowsFormsApp2
{
    internal class Ball : GameObj
    {

        int Code = 0;

        private Ball(int Code)
        {
            this.Code = Code;
        }

        public override int GetCode()
        {
            return Code;
        }

        public static Ball BallObj() // получим объект из класса поэтому static
        {
            return new Ball(8);
        }

        public override void Draw(Graphics g, Image TextureSet, int i, int j)
        {
            g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(35, 35)), new Rectangle(419, 195, 125, 125), GraphicsUnit.Pixel);
        }

    }
}
