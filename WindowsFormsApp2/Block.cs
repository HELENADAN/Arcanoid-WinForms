using System.Drawing;

namespace WindowsFormsApp2
{
    internal class Block : GameObj
    {

        int Code = 0;

        protected Block(int Code)
        {
            this.Code = Code;
        }

        public override int GetCode()
        {
            return Code;
        }
        
        public static Block Leftpartblock() // получим объект из класса поэтому static
        {
            return new Block(1);
        }

        public static Block Rightpartblock() 
        {
            return new Block(11);
        }

        public static Block Leftpartdoubleblock() 
        {
            return new Block(2);
        }

        public static Block Rightpartdoubleblock() 
        {
            return new Block(22);
        }

        public override void Draw(Graphics g, Image TextureSet, int i, int j)
        {
            if (Code == 1)
                g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(229, 420, 92, 37), GraphicsUnit.Pixel);
            else if (Code == 2)
                g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(82, 420, 92, 37), GraphicsUnit.Pixel);
        }

    }
}
