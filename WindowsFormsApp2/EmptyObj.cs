using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class EmptyObj : GameObj
    {
        public EmptyObj() { }
        public override int GetCode() 
        {
            return 0;
        }

        public override void Draw(Graphics g, Image TextureSet, int i, int j)
        {
           
        }
    }
}
