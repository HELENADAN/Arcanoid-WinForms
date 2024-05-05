using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class GameField
    {       
        public Image TextureSet; // картинка для игры

        // карта, ее размер, двумерный массив = игровое поле
        public readonly int Width;  
        public readonly int Height;
        public int[,] field;

        private Platform platform;

        // переменные, которые отвечают за текущую позицию платформы
        public int platformX = 0;
        public int platformY = 0;
        public int GetPlatformCode()
        {
            return platform.GetCode();
        }

        private Ball ball;
        
        // переменные, которые отвечают за текущие координаты мячика на карте
        public int BallX;
        public int BallY;

        // переменные, которые отвечают за направление движения мячика 
        public int dirX = 0;
        public int dirY = 0;

        public int GetBallCode()
        {
            return ball.GetCode();
        }
        private Block block = new Block();
        private Block doubleblock = new DoubleBlock();
        public int GetBlockCode()
        {
            return block.GetCode();
        }
        public int GetDoubleBlockCode()
        {
            return doubleblock.GetCode();
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
                            field[0, j] = block.GetCode();
                            field[0, j + 1] = field[0, j] + field[0, j] * 10;// для определения коллизий с мячом
                        }                  
                    break;
                case DifficultyLevel.Medium:
                    Random r = new Random();
                    // заполняем карту на треть платформами для игры
                    for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                    {
                        if (r.Next(1, 3) == 1)
                        {
                            field[0, j] = block.GetCode();
                        }
                        else field[0, j] = doubleblock.GetCode();
                        field[0, j + 1] = field[0, j] + field[0, j] * 10;// для определения коллизий с мячом
                    }
                    break;
                case DifficultyLevel.Hard:
                    for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                    {
                        field[0, j] = doubleblock.GetCode();
                        field[0, j + 1] = field[0, j] + field[0, j] * 10;// для определения коллизий с мячом
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
                           field[i, j] = block.GetCode();
                           field[i, j + 1] = field[i, j] + field[i, j] * 10;// для определения коллизий с мячом
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
                                field[i, j] = block.GetCode();
                            }
                            else field[i, j] = doubleblock.GetCode();
                            field[i, j + 1] = field[i, j] + field[i, j] * 10;// для определения коллизий с мячом
                        }
                    }
                    break;
                case DifficultyLevel.Hard:
                    for (int i = 0; i < this.Height / 3; i++)
                    {
                        for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                        {
                            field[i, j] = doubleblock.GetCode();
                            field[i, j + 1] = field[i, j] + field[i, j] * 10;// для определения коллизий с мячом
                        }
                    }
                    break;
            }
        }

        // конструктор игрового поля
        public GameField()
        {   
            ball = new Ball();
            platform = new Platform();

            // подгрузка картинки
            TextureSet = new Bitmap("C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\img_for_w2\\arca.jpg");

            this.Width = 20;
            this.Height = 30;

            field = new int[Height, Width];
          
            // заполняем массив карты нулями
            for (int i = 0; i < this.Height; i++)// двигается по х
            {
                for (int j = 0; j < this.Width; j++)// двигается по у
                {
                    field[i, j] = 0;// заполняем двумерный массив нулями
                }
            }

            // расположение платформы относительно карты - по центру

            platformX = (this.Width - 1) / 2;
            platformY = this.Height - 1;

            // место размещения платформы на карте

            field[platformY, platformX] = platform.GetCode(); // левый конец платформы
            field[platformY, platformX + 1] = platform.GetCode() * 10 + field[platformY, platformX];
            field[platformY, platformX + 2] = platform.GetCode() * 100 + field[platformY, platformX + 1];// правый конец платформы

            // задаем расположение мячика

            BallY = platformY - 1; // на строчку выше платформы расположен мяч
            BallX = platformX + 1; // мяч размещен по середине платформы

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
                    if (field[i, j] == platform.GetCode())
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(90, 40)), new Rectangle(73, 236, 307, 123), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == ball.GetCode())
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(35, 35)), new Rectangle(419, 195, 125, 125), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == doubleblock.GetCode())
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(82, 420, 92, 37), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == block.GetCode())
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(229, 420, 92, 37), GraphicsUnit.Pixel);
                    }
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
