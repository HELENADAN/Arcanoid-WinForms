using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{   // находится ли мячик вне границ массива + столкновение с чем-либо

    class GameLogic
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
        public bool IsCollade(GameField field, Label ScoreLabel, Label ScoreNubmerLabel)
        {

            // столкновение с границей по х
            bool isColliding = false;


            if (field.BallX + field.dirX > field.Width - 1 || field.BallX + field.dirX < 0)
            {
                field.dirX *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }
            // столкновение с границей по у
            if (field.BallY + field.dirY > field.Height - 1 || field.BallY + field.dirY < 0)
            {
                field.dirY *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }

            // столкновение с объектом
            if (field.field[field.BallY + field.dirY, field.BallX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                if (field.field[field.BallY + field.dirY, field.BallX] > 10 && field.field[field.BallY + field.dirY, field.BallX] < 99)
                {
                    field.field[field.BallY + field.dirY, field.BallX] = 0;
                    field.field[field.BallY + field.dirY, field.BallX - 1] = 0;
                    addScore = true;

                }
                else if (field.field[field.BallY + field.dirY, field.BallX] < 9)
                {
                    field.field[field.BallY + field.dirY, field.BallX] = 0;
                    field.field[field.BallY + field.dirY, field.BallX + 1] = 0;
                    addScore = true;
                }

                if (addScore)
                {
                    player.AddScore(50);

                    if (player.GetScore() % 200 == 0 && player.GetScore() > 0)
                    {
                        field.AddLine();
                    }

                }

                field.dirY *= -1; // мяч полетит в другю сторону
            }

            // если слева или справа не 0, есть какой то объект, то нужно сменить направление
            // реализация исчезновения платформ сверху
            if (field.field[field.BallY, field.BallX + field.dirX] != 0)
            {

                isColliding = true;
                bool addScore = false;
                // если происходит колизия с элементом число которого больше 10, то есть это часть платформы == правая
                if (field.field[field.BallY + field.dirY, field.BallX] > 10 && field.field[field.BallY + field.dirY, field.BallX] < 99)
                {
                    // обнуляем то с чем столкнулись и то что было до 

                    field.field[field.BallY, field.BallX + field.dirX] = 0; // зануляем правую часть
                    field.field[field.BallY, field.BallX + field.dirX - 1] = 0; // зануляем левую часть
                    addScore = true;
                }
                else if (field.field[field.BallY + field.dirY, field.BallX] < 9) // значит столкнулись с первой частью препятствия == левая
                {
                    field.field[field.BallY, field.BallX + field.dirX] = 0; // зануляем левую часть
                    field.field[field.BallY, field.BallX + field.dirX + 1] = 0; // зануляем правую часть
                    addScore = true;
                }
                if (addScore)
                {
                    player.AddScore(50);
                    if (player.GetScore() % 200 == 0 && player.GetScore() > 0)
                    {
                        field.AddLine();
                    }
                }
                field.dirX *= -1; // мяч полетит в другю сторону
            }
            ScoreLabel.Text = "SCORES"; // обновим счетчик
            ScoreNubmerLabel.Text = player.GetScore().ToString();
            return isColliding;

        }      
       

    }
}
