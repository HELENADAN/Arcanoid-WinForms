using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    abstract class GameObj
    {
        protected int Code;

        public int GetCode()
        {
            return Code;
        }
    }
}
