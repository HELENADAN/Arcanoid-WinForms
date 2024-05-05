using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{   
    // находится ли мячик вне границ массива + столкновение с чем-либо
    class GameLogic
    {
        readonly Player player = new Player();
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

        public int RefreshPlayerScore()
        {
            return player.RefreshScore();
        }

        public int RefreshPlayerLife()
        {
            return player.RefreshLife();
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

           
            // если значение элемента массива field (представляющего игровое поле) в позиции, куда направлено движение мяча, не равно 0
            // значит произошло столкновение с блоками сверху (1 11 или 2 22)

            if (field.field[field.BallY + field.dirY, field.BallX] != 0)
            {
                bool addScore = false;
                isColliding = true;

                // Если столкнулся с одинарным блоком (число которого == 11 == правая часть), то блок уничтожается, а игроку начисляются очки

                if (field.field[field.BallY + field.dirY, field.BallX] == field.GetBlockCode() * 10 + field.GetBlockCode())
                {   
                    // уничтожаем блок
                    field.field[field.BallY + field.dirY, field.BallX] = 0;
                    field.field[field.BallY + field.dirY, field.BallX - 1] = 0;
                    addScore = true;//добавить очки

                }

                // Если столкнулся с двойным блоком (число - 22 - право), то двойной блок заменяется на одинарный (число - 11 - право), а соседний блок (число 2 - лево) также заменяется на одинарный (число - 1 - лево)
                
                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetDoubleBlockCode() * 10 + field.GetDoubleBlockCode())
                {
                    field.field[field.BallY + field.dirY, field.BallX] = field.GetBlockCode() * 10 + field.GetBlockCode();
                    field.field[field.BallY + field.dirY, field.BallX - 1] = field.GetBlockCode();
                }

                // Если столкнулся с одинарным блоком (число которого == 1 == лево), то блок уничтожается, а игроку начисляются очки

                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetBlockCode())
                {   // уничтожаем блок
                    field.field[field.BallY + field.dirY, field.BallX] = 0;
                    field.field[field.BallY + field.dirY, field.BallX + 1] = 0;
                    addScore = true;//добавить очки
                }

                // Если столкнулся с двойным блоком (число - 2 - лево), то двойной блок заменяется на одинарный (число - 1 - лево), а соседний блок (число - 22 - право) также заменяется на одинарный (число - 11 - право)
                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetDoubleBlockCode())
                {
                    field.field[field.BallY + field.dirY, field.BallX] = field.GetBlockCode();
                    field.field[field.BallY + field.dirY, field.BallX + 1] = field.GetBlockCode() * 10 + field.GetBlockCode();
                }

                if (addScore)
                {
                    player.AddScore(50);

                    if (player.GetScore() % 500 == 0 && player.GetScore() > 0)
                    {
                        field.AddLine();
                    }

                }

                field.dirY *= -1; // мяч полетит в другую сторону
            }

            // если значение элемента массива field (представляющего игровое поле) в позиции, куда направлено движение мяча, не равно 0
            // значит произошло столкновение с блоками сверху

            if (field.field[field.BallY, field.BallX + field.dirX] != 0)
            {

                isColliding = true;
                bool addScore = false;

                // Если столкнулся с одинарным блоком (число которого == 11 == правая часть блока), то блок уничтожается, а игроку начисляются очки

                if (field.field[field.BallY + field.dirY, field.BallX] == field.GetBlockCode() * 10 + field.GetBlockCode())
                {
                    // уничтожаем блок
                    field.field[field.BallY, field.BallX + field.dirX] = 0; // зануляем правую часть
                    field.field[field.BallY, field.BallX + field.dirX - 1] = 0; // зануляем левую часть
                    addScore = true;//добавить очки
                }
                // если происходит колизия с элементом число которого == 22 == правая часть блока
                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetDoubleBlockCode() * 10 + field.GetDoubleBlockCode())
                {
                    // уменьшаем прочность на один у правой и левой части
                    field.field[field.BallY, field.BallX + field.dirX] = field.GetBlockCode() * 10 + field.GetBlockCode(); 
                    field.field[field.BallY, field.BallX + field.dirX - 1] = field.GetBlockCode(); 
                    
                }

                // Если столкнулся с одинарным блоком (число которого == 1 == левая часть), то блок уничтожается, а игроку начисляются очки
                
                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetBlockCode())
                {   // уничтожаем блок
                    field.field[field.BallY, field.BallX + field.dirX] = 0; // зануляем левую часть
                    field.field[field.BallY, field.BallX + field.dirX + 1] = 0; // зануляем правую часть
                    addScore = true;//добавить очки
                }
                // если происходит колизия с элементом число которого == 2 == левая часть

                else if (field.field[field.BallY + field.dirY, field.BallX] == field.GetDoubleBlockCode())
                {   // уменьшаем прочность на один у правой и левой части
                    field.field[field.BallY, field.BallX + field.dirX] = field.GetBlockCode(); 
                    field.field[field.BallY, field.BallX + field.dirX + 1] = field.GetBlockCode() * 10 + field.GetBlockCode();
                    
                }

                if (addScore)
                {
                    player.AddScore(50);
                    if (player.GetScore() % 500 == 0 && player.GetScore() > 0)
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