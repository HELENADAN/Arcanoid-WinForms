using System.Windows.Forms;

namespace WindowsFormsApp2
{   
    // находится ли мячик вне границ массива + столкновение с чем-либо
    class GameLogic
    {
        readonly Player player = new Player();
        readonly GameField field;
        public GameLogic(GameField field)
        {
            this.field = field;     
        }
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

        public bool NeedHandleBottomCollision() 
        {
            return field.getbally() + field.dirY > field.Height - 1;
        }

        public void HandleBallCollision()
        {
            if (!IsCollade())
            {
                // меняем координаты
                field.MoveBall();
            }
            
        }
        public bool IsCollade()
        {

            // столкновение с границей по х
            bool isColliding = false;


            if (field.getballx() + field.dirX > field.Width - 1 || field.getballx() + field.dirX < 0)
            {
                field.dirX *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }
            // столкновение с границей по у
            if (field.getbally() + field.dirY > field.Height - 1 || field.getbally() + field.dirY < 0)
            {
                field.dirY *= -1; // мяч полетит в другю сторону
                isColliding = true;
            }

           
            // если значение элемента массива field (представляющего игровое поле) в позиции, куда направлено движение мяча, не равно 0
            // значит произошло столкновение с блоками сверху (1 11 или 2 22)

            if (!field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetEmptyObj()))
            {
                bool addScore = false;
                isColliding = true;

                // Если столкнулся с одинарным блоком (число которого == 11 == правая часть), то блок уничтожается, а игроку начисляются очки

                if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetRightpartblock()))
                {   
                    // уничтожаем блок
                    field.field[field.getbally() + field.dirY, field.getballx()] = field.GetEmptyObj();
                    field.field[field.getbally() + field.dirY, field.getballx() - 1] = field.GetEmptyObj();
                    addScore = true;//добавить очки

                }

                // Если столкнулся с двойным блоком (число - 22 - право), то двойной блок заменяется на одинарный (число - 11 - право), а соседний блок (число 2 - лево) также заменяется на одинарный (число - 1 - лево)
                
                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetRightpartdoubleblock()))
                {
                    field.field[field.getbally() + field.dirY, field.getballx()] = field.GetRightpartblock();
                    field.field[field.getbally() + field.dirY, field.getballx() - 1] = field.GetLeftpartblock();
                }

                // Если столкнулся с одинарным блоком (число которого == 1 == лево), то блок уничтожается, а игроку начисляются очки

                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetLeftpartblock()))
                {   // уничтожаем блок
                    field.field[field.getbally() + field.dirY, field.getballx()] = field.GetEmptyObj(); 
                    field.field[field.getbally() + field.dirY, field.getballx() + 1] = field.GetEmptyObj();
                    addScore = true;//добавить очки
                }

                // Если столкнулся с двойным блоком (число - 2 - лево), то двойной блок заменяется на одинарный (число - 1 - лево), а соседний блок (число - 22 - право) также заменяется на одинарный (число - 11 - право)
                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetLeftpartdoubleblock()))
                {
                    field.field[field.getbally() + field.dirY, field.getballx()] = field.GetLeftpartblock();
                    field.field[field.getbally() + field.dirY, field.getballx() + 1] = field.GetRightpartblock();
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

            if (!field.field[field.getbally(), field.getballx()+ field.dirX].Equals(field.GetEmptyObj()))
            {

                isColliding = true;
                bool addScore = false;

                // Если столкнулся с одинарным блоком (число которого == 11 == правая часть блока), то блок уничтожается, а игроку начисляются очки

                if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetRightpartblock()))
                {
                    // уничтожаем блок
                    field.field[field.getbally(), field.getballx()+ field.dirX] = field.GetEmptyObj(); // зануляем правую часть
                    field.field[field.getbally(), field.getballx()+ field.dirX - 1] = field.GetEmptyObj(); // зануляем левую часть
                    addScore = true;//добавить очки
                }
                // если происходит колизия с элементом число которого == 22 == правая часть блока
                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetRightpartdoubleblock()))
                {
                    // уменьшаем прочность на один у правой и левой части
                    field.field[field.getbally(), field.getballx()+ field.dirX] = field.GetRightpartblock(); 
                    field.field[field.getbally(), field.getballx()+ field.dirX - 1] = field.GetLeftpartblock(); 
                    
                }

                // Если столкнулся с одинарным блоком (число которого == 1 == левая часть), то блок уничтожается, а игроку начисляются очки
                
                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetLeftpartblock()))
                {   // уничтожаем блок
                    field.field[field.getbally(), field.getballx()+ field.dirX] = field.GetEmptyObj(); // зануляем левую часть
                    field.field[field.getbally(), field.getballx()+ field.dirX + 1] = field.GetEmptyObj(); // зануляем правую часть
                    addScore = true;//добавить очки
                }
                // если происходит колизия с элементом число которого == 2 == левая часть

                else if (field.field[field.getbally() + field.dirY, field.getballx()].Equals(field.GetLeftpartdoubleblock()))
                {   // уменьшаем прочность на один у правой и левой части
                    field.field[field.getbally(), field.getballx()+ field.dirX] = field.GetLeftpartblock(); 
                    field.field[field.getbally(), field.getballx()+ field.dirX + 1] = field.GetRightpartblock();
                    
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
            
            return isColliding;
        }
    }
}