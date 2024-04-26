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

        // переменные, которые отвечают за позицию платформы
        public int platformX = 0;
        public int platformY = 0;

        private Ball ball = new Ball();

        // переменные, которые отвечают за координаты мячика на карте
        public int BallX;
        public int BallY;

        // переменные, которые отвечают за координаты при движении шарика
        public int dirX = 0;
        public int dirY = 0;
        // добавление сложности == смещаем все линии вниз и добавляем одну лининю
        public int GetBallCode()
        {
            return ball.GetCode();
        }
        public void AddLine()
        {
            for (int i = this.Height - 2; i > 0; i--)
            {
                for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                {
                    field[i, j] = field[i - 1, j];
                }
            }

            //GeneratePlatform();
        }

        // генерация препятствий для мячика
        public void GeneratePlatform()
        {
            Random r = new Random();

            // заполняем карту на треть платформами для игры
            for (int i = 0; i < this.Height / 3; i++)
            {
                for (int j = 0; j < this.Width; j += 2) // так препятствие состоит из 2 частей
                {
                    int currPlatform = r.Next(1, 3);
                    field[i, j] = currPlatform;
                    field[i, j + 1] = currPlatform + currPlatform * 10; // для определения коллизий с мячом
                }
            }

        }
        // метод для инициализации
        public GameField()
        {
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

            // расположение платформы относительно карты --- по центру нужно

            platformX = (this.Width - 1) / 2;
            platformY = this.Height - 1;

            // место размещения платформы на карте

            field[platformY, platformX] = 9; // левый конец платформы
            field[platformY, platformX + 1] = 99;// средина
            field[platformY, platformX + 2] = 999;// правый конец платформы

            // задаем расположение мячика

            BallY = platformY - 1; // на строчку выше платформы расположен мяч
            BallX = platformX + 1; // мяч размещен по середине платформы

            // реализация движения мячика
            dirX = 1;
            dirY = -1;

            GeneratePlatform(); 
        }

        // Метод DrawMap() отрисовывает элементы игры на холсте. Берем из набор спрайтов
        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    if (field[i, j] == 9)
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(90, 40)), new Rectangle(73, 236, 307, 123), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == 8)
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(35, 35)), new Rectangle(419, 195, 125, 125), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == 1)
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(82, 420, 92, 37), GraphicsUnit.Pixel);
                    }

                    if (field[i, j] == 2)
                    {
                        g.DrawImage(TextureSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(229, 420, 92, 37), GraphicsUnit.Pixel);
                    }
                }
            }
            
        }

        // здесь основная территория геймплея
        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width * 20, this.Height * 20));
        }
    }
}
