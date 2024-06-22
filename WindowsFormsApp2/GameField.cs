using System;
using System.Drawing;

namespace WindowsFormsApp2
{
    internal class GameField
    {       
        public Image TextureSet; // картинка для игры

        // карта, ее размер, двумерный массив = игровое поле
        public readonly int Width;  
        public readonly int Height;
        public GameObj[,] field;

        public EmptyObj emptyObj = new EmptyObj(); // объект для заполнения пустых клеток поля
        
        public EmptyObj GetEmptyObj()
        {
            return emptyObj;
        }

        private Platform leftplatformpart;
        private Platform centerplatformpart;
        private Platform rightplatformpart;
      
        public Platform GetLeftPlatform()
        {
            return leftplatformpart;
        }
        public Platform GetCenterPlatform()
        {
            return centerplatformpart;
        }
        public Platform GetRightPlatform()
        {
            return rightplatformpart;
        }

        private Ball ball;
        
   
        // переменные, которые отвечают за направление движения мячика 
        public int dirX = 0;
        public int dirY = 0;

        public Ball GetBall()
        {
            return ball;
        }

        private Block Leftpartblock = Block.Leftpartblock();
        private Block Leftpartdoubleblock = Block.Leftpartdoubleblock();
        private Block Rightpartblock = Block.Rightpartblock();
        private Block Rightpartdoubleblock = Block.Rightpartdoubleblock();
        public Block GetLeftpartblock()
        {
            return Leftpartblock;
        }
        public Block GetLeftpartdoubleblock()
        {
            return Leftpartdoubleblock;
        }
        public Block GetRightpartblock()
        {
            return Rightpartblock;
        }
        public Block GetRightpartdoubleblock()
        {
            return Rightpartdoubleblock;
        }
        public void SetStartBallPosition() 
        {
            ball.SetY(leftplatformpart.GetY() - 2); // на строчку выше платформы расположен мяч
            ball.SetX(leftplatformpart.GetX() + 1); // мяч размещен по середине платформы
            field[ball.GetY(),ball.GetX()] = ball;//
        }

        public void HideBall()
        {
            field[ball.GetY(),ball.GetX()] = emptyObj;
        }

        public void HidePlatform()
        {
            // очистим предыдущее место размещение платформы
            field[leftplatformpart.GetY(),leftplatformpart.GetX()] = emptyObj;
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+1] = emptyObj;
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+2] = emptyObj;
        }
 
        public void PlatformMoveRight()
        {
            if (leftplatformpart.GetX() + 4 < Width - 1)
                // сдвинем координату платформы на единичку по оси x 
                leftplatformpart.SetX(leftplatformpart.GetX() +2);
        }

        public void PlatformMoveLeft()
        {
            if (leftplatformpart.GetX() > 1)
                // сдвинем координату платформы на единичку по оси x
                leftplatformpart.SetX(leftplatformpart.GetX()- 2);
        }

        public void UpdatePlatformCoordinates()
        {
            // Размещает платформу на карте с помощью специальных маркеров для разных частей платформы
            field[leftplatformpart.GetY(),leftplatformpart.GetX()] = leftplatformpart; // левый конец платформы
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+1] = centerplatformpart;// середина
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+2] = rightplatformpart;// правый конец платформы
        }

        public void PlaceBall()
        {
            field[ball.GetY(),ball.GetX()] = ball;
        }

        public void ResetBallDirectory()
        {
            dirX = 1;
            dirY = -1;
        }

        public void MoveBall()
        {
            ball.SetX(ball.GetX()+dirX);
            ball.SetY(ball.GetY() + dirY);
        }

        public int getballx()
        {
            return ball.GetX();
        }

        public int getbally()
        {
            return ball.GetY();
        }

        DifficultyLevel selectedLevel = DifficultyLevel.Medium;


        // добавление сложности, смещаем все линии вниз и добавляем одну линию, добавляем блоки в зависимости от уровня сложности!
        public void AddLine()
        {
            for (int i = this.Height - 2; i > 0; i--)
            {
                for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                {
                    field[i, j] = field[i - 1, j];
                }
            }

            switch (selectedLevel)
            {
                case DifficultyLevel.Easy:
                    for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                        {
                            field[0, j] = Leftpartblock;
                            field[0, j + 1] = Rightpartblock;// для определения коллизий с мячом
                        }                  
                    break;
                case DifficultyLevel.Medium:
                    Random r = new Random();
                    // заполняем карту на треть платформами для игры
                    for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                    {
                        if (r.Next(1, 3) == 1)
                        {
                            field[0, j] = Leftpartblock;
                            field[0, j + 1] = Rightpartblock;
                        }
                        else
                        { 
                           field[0, j] = Leftpartdoubleblock;
                           field[0, j + 1] = Rightpartdoubleblock;// для определения коллизий с мячо
                        } 
                    }
                    break;
                case DifficultyLevel.Hard:
                    for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                    {
                        field[0, j] = Leftpartdoubleblock;
                        field[0, j + 1] = Rightpartdoubleblock;// для определения коллизий с мячом
                    }
                    break;
            }
        }

        // генерация препятствий для мячика в зависимости от уровня сложности
        public void GenerateBlocks(DifficultyLevel level)
        {
            selectedLevel = level;
            switch (level) 
            {
                case DifficultyLevel.Easy:
                    for (int i = 0; i < this.Height / 3; i++)
                    {
                        for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                        {                           
                           field[i, j] = Leftpartblock;
                           field[i, j + 1] = Rightpartblock;// для определения коллизий с мячом
                        }
                    }
                    break;
                case DifficultyLevel.Medium:
                    Random r = new Random();
                    // заполняем карту на треть платформами для игры
                    for (int i = 0; i < this.Height / 3; i++)
                    {
                        for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                        {
                            if (r.Next(1, 3) == 1)
                            {
                                field[i, j] = Leftpartblock;
                                field[i, j + 1] = Rightpartblock;
                            }
                            else
                            {
                                field[i, j] = Leftpartdoubleblock;
                                field[i, j + 1] = Rightpartdoubleblock;// для определения коллизий с мячом
                            }                          
                        }
                    }
                    break;
                case DifficultyLevel.Hard:
                    for (int i = 0; i < this.Height / 3; i++)
                    {
                        for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                        {
                            field[i, j] = Leftpartdoubleblock;
                            field[i, j + 1] = Rightpartdoubleblock;// для определения коллизий с мячом
                        }
                    }
                    break;
            }
        }

        // конструктор игрового поля
        public GameField()
        {
            ball = Ball.BallObj();

            leftplatformpart = Platform.Leftpart();
            centerplatformpart = Platform.Centerpart();
            rightplatformpart = Platform.Rightpart();

            // подгрузка картинки
            TextureSet = new Bitmap("C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\img_for_w2\\arca.jpg");

            this.Width = 20;
            this.Height = 30;

            field = new GameObj[Height, Width];

            // заполняем массив карты нулями
            for (int i = 0; i < this.Height; i++)// двигается по х
            {
                for (int j = 0; j < this.Width; j++)// двигается по у
                {
                    field[i, j] = emptyObj;// заполняем двумерный массив нулями
                }
            }

            // расположение платформы относительно карты - по центру

            leftplatformpart.SetX((this.Width - 1) / 2);
            leftplatformpart.SetY(this.Height - 1); 

            // место размещения платформы на карте

            field[leftplatformpart.GetY(),leftplatformpart.GetX()] = leftplatformpart;
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+1] = centerplatformpart;
            field[leftplatformpart.GetY(),leftplatformpart.GetX()+2] = rightplatformpart;

            // задаем расположение мячика

            // на строчку выше платформы расположен мяч
            ball.SetY(leftplatformpart.GetY() - 1);
;           // мяч размещен по середине платформы
            ball.SetX(leftplatformpart.GetX() + 1);

            // реализация движения мячика
            dirX = 1;
            dirY = -1;
        }

        // Метод DrawMap() рисует элементы игры на холсте. Берем из спрайта
        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    field[i, j].Draw(g, TextureSet, i, j);                   
                }
            }
            
        }

        // здесь основная территория геймплея - прямоугольник
        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width * 20, this.Height * 20));
        }
    }
}
