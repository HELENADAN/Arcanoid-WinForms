using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal class Physics2DController
    {   // находится ли мячик вне границ массива + столкновение с чем-либо

        public bool IsCollade(Player player, MapController map, Label ScoreLabel, Label score_label)
        {

            // столкновение с границей по х
            bool isColliding = false;


            if (player.BallX + player.dirX > MapController.mapWidth - 1 || player.BallX + player.dirX < 0)
            {
                player.dirX *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }
            // столкновение с границей по у
            if (player.BallY + player.dirY > MapController.mapHeight - 1 || player.BallY + player.dirY < 0)
            {
                player.dirY *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }

            // столкновение с объектом
            if (map.map[player.BallY + player.dirY, player.BallX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (map.map[player.BallY + player.dirY, player.BallX] > 10 && map.map[player.BallY + player.dirY, player.BallX] < 99)
                {
                    map.map[player.BallY + player.dirY, player.BallX] = 0;
                    map.map[player.BallY + player.dirY, player.BallX - 1] = 0;
                    addScore = true;

                }
                else if (map.map[player.BallY + player.dirY, player.BallX] < 9)
                {
                    map.map[player.BallY + player.dirY, player.BallX] = 0;
                    map.map[player.BallY + player.dirY, player.BallX + 1] = 0;
                    addScore = true;
                }

                if (addScore)
                {
                    player.score += 50;

                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine();
                    }

                }

                player.dirY *= -1; // мяч полетит в другю сторону
            }

            // если слева или справа не 0, есть какой то объект, то нужно сменить направление
            // реализация исчезновения платформ сверху
            if (map.map[player.BallY, player.BallX + player.dirX] != 0)
            {

                isColliding = true;
                bool addScore = false;
                // если происходит колизия с элементом число которого больше 10, то есть это часть платформы == правая
                if (map.map[player.BallY + player.dirY, player.BallX] > 10 && map.map[player.BallY + player.dirY, player.BallX] < 99)
                {
                    // обнуляем то с чем столкнулись и то что было до 

                    map.map[player.BallY, player.BallX + player.dirX] = 0; // зануляем правую часть
                    map.map[player.BallY, player.BallX + player.dirX - 1] = 0; // зануляем левую часть
                    addScore = true;
                }
                else if (map.map[player.BallY + player.dirY, player.BallX] < 9) // значит столкнулись с первой частью препятствия == левая
                {
                    map.map[player.BallY, player.BallX + player.dirX] = 0; // зануляем левую часть
                    map.map[player.BallY, player.BallX + player.dirX + 1] = 0; // зануляем правую часть
                    addScore = true;
                }
                if (addScore)
                {
                    player.score += 50;
                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine();
                    }
                }
                player.dirX *= -1; // мяч полетит в другю сторону
            }
            ScoreLabel.Text = "Score"; // обновим счетчик
            score_label.Text = player.score.ToString();
            return isColliding;

        }

    }
}
