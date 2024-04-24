using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{   // находится ли мячик вне границ массива + столкновение с чем-либо

    class Physics2DController
    {
        Player player = new Player();
        public int GetPlayerScore()
        {
            return player.GetScore();
        }

        public int GetPlayerLives()
        {
            return player.GetLives();
        }

        public int DamagePlayer()
        {
            return player.SubtructLife();
        }
        public bool IsCollade(MapController map, Label ScoreLabel, Label ScoreNubmerLabel)
        {

            // столкновение с границей по х
            bool isColliding = false;


            if (map.BallX + map.dirX > map.Width - 1 || map.BallX + map.dirX < 0)
            {
                map.dirX *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }
            // столкновение с границей по у
            if (map.BallY + map.dirY > map.Height - 1 || map.BallY + map.dirY < 0)
            {
                map.dirY *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }

            // столкновение с объектом
            if (map.map[map.BallY + map.dirY, map.BallX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map.map[map.BallY + map.dirY, map.BallX] > 10 && map.map[map.BallY + map.dirY, map.BallX] < 99)
                {
                    map.map[map.BallY + map.dirY, map.BallX] = 0;
                    map.map[map.BallY + map.dirY, map.BallX - 1] = 0;
                    addScore = true;

                }
                else if (map.map[map.BallY + map.dirY, map.BallX] < 9)
                {
                    map.map[map.BallY + map.dirY, map.BallX] = 0;
                    map.map[map.BallY + map.dirY, map.BallX + 1] = 0;
                    addScore = true;
                }

                if (addScore)
                {
                    player.AddScore(50);

                    if (player.GetScore() % 200 == 0 && player.GetScore() > 0)
                    {
                        map.AddLine();
                    }

                }

                map.dirY *= -1; // мяч полетит в другю сторону
            }

            // если слева или справа не 0, есть какой то объект, то нужно сменить направление
            // реализация исчезновения платформ сверху
            if (map.map[map.BallY, map.BallX + map.dirX] != 0)
            {

                isColliding = true;
                bool addScore = false;
                // если происходит колизия с элементом число которого больше 10, то есть это часть платформы == правая
                if (map.map[map.BallY + map.dirY, map.BallX] > 10 && map.map[map.BallY + map.dirY, map.BallX] < 99)
                {
                    // обнуляем то с чем столкнулись и то что было до 

                    map.map[map.BallY, map.BallX + map.dirX] = 0; // зануляем правую часть
                    map.map[map.BallY, map.BallX + map.dirX - 1] = 0; // зануляем левую часть
                    addScore = true;
                }
                else if (map.map[map.BallY + map.dirY, map.BallX] < 9) // значит столкнулись с первой частью препятствия == левая
                {
                    map.map[map.BallY, map.BallX + map.dirX] = 0; // зануляем левую часть
                    map.map[map.BallY, map.BallX + map.dirX + 1] = 0; // зануляем правую часть
                    addScore = true;
                }
                if (addScore)
                {
                    player.AddScore(50);
                    if (player.GetScore() % 200 == 0 && player.GetScore() > 0)
                    {
                        map.AddLine();
                    }
                }
                map.dirX *= -1; // мяч полетит в другю сторону
            }
            ScoreLabel.Text = "SCORES"; // обновим счетчик
            ScoreNubmerLabel.Text = player.GetScore().ToString();
            return isColliding;

        }      
       
    }
}
