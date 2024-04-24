using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Player
    {
        private int score; // очки
        private int lives; // жизни

        public Player() 
        {
            score = 0;
            lives = 5;
        }

        public int GetScore()
        {
            return score;
        }

        public int GetLives()
        {
            return lives;
        }

        public int AddScore(int score)
        {
            this.score += score;
            return score;
        }

        public int SubtructLife()
        {   
            if (lives>0) lives--;
            return lives;
        }
    }
}
