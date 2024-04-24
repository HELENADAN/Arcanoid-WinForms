using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class Double_Block : Block
    {
        private bool damage;

        public bool IsDamage()
        { 
            return damage;
        }

        public void SetDamage(bool value) 
        { 
            damage = value;
        }
    }
}
