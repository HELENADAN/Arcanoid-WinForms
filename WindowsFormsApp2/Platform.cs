using System.Drawing;

namespace WindowsFormsApp2
{
    internal class Platform:GameObj
    {
        // частям платформ присваиваются необходимые коды
        int Code = 0;

        private Platform(int Code) 
        {
            this.Code = Code;
        }

        public override int GetCode()
        {
            return Code;
        }
       
        public static Platform Leftpart() // получим объект из класса поэтому static
        { 
            return new Platform(9);
        }

        public static Platform Centerpart()
        {
            return new Platform(99);
        }

        public static Platform Rightpart() 
        {
            return new Platform(999);
        }

        public override void Draw(Graphics g, Image TextureSet, int i, int j)
        {   
            if (Code == 9)
                g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(90, 40)), new Rectangle(73, 236, 307, 123), GraphicsUnit.Pixel);
        }

    }
}
