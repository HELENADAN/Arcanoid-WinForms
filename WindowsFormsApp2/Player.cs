using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class Player
    {

        // переменные, которые отвечают за координаты мячика на карте
        public int BallX;
        public int BallY;

        // переменные, которые отвечают за позицию платформы
        public int platformX = 0;
        public int platformY = 0;

        // переменные, которые отвечают за координаты при движении шарика
        public int dirX = 0;
        public int dirY = 0;

        public int score; // очки
        public int lives; // жизни


    }
}
