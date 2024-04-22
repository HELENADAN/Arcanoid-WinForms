using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class MapController
    {
        public Image arcanoidSet; // картинка для игры

        // карта, ее размер, двумерный массив = игровое поле
        public const int mapWidth = 20; // ширина карты
        public const int mapHeight = 30;
        public int[,] map = new int[mapHeight, mapWidth];

        // переменные, которые отвечают за позицию платформы
        public int platformX = 0;
        public int platformY = 0;

        // переменные, которые отвечают за координаты мячика на карте
        public int BallX;
        public int BallY;

        // переменные, которые отвечают за координаты при движении шарика
        public int dirX = 0;
        public int dirY = 0;
        public MapController()
        {
            // подгрузка картинки

            arcanoidSet = new Bitmap("C:\\Users\\Елена\\Desktop\\Arc\\WindowsFormsApp2\\img_for_w2\\arca.jpg");

        }

        // добавление сложности == смещаем все линии вниз и добавляем одну лининю

        public void AddLine()
        {
            for (int i = MapController.mapHeight - 2; i > 0; i--)
            {
                for (int j = 0; j < MapController.mapWidth; j += 2) // так препятствие состоит из 2 частей
                {
                    map[i, j] = map[i - 1, j];
                }
            }

            Random r = new Random();

            // заполняем карту на треть платформами для игры
            for (int i = 0; i < MapController.mapHeight / 3; i++)
            {
                for (int j = 0; j < MapController.mapWidth; j += 2) // так препятствие состоит из 2 частей
                {
                    int currPlatform = r.Next(1, 3);// От 1 включительно до 3 невключительно
                    map[i, j] = currPlatform;
                    map[i, j + 1] = currPlatform + currPlatform * 10; // для определения коллизий с мячом
                }
            }

        }

        // Метод DrawMap() отрисовывает элементы игры на холсте. Берем из набор спрайтов
        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < MapController.mapHeight; i++)
            {
                for (int j = 0; j < MapController.mapWidth; j++)
                {
                    if (map[i, j] == 9)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(90, 40)), new Rectangle(73, 236, 307, 123), GraphicsUnit.Pixel);
                    }

                    if (map[i, j] == 8)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(30, 30)), new Rectangle(419, 221, 100, 100), GraphicsUnit.Pixel);
                    }

                    if (map[i, j] == 1)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(82, 420, 92, 37), GraphicsUnit.Pixel);
                    }

                    if (map[i, j] == 2)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(229, 420, 92, 37), GraphicsUnit.Pixel);
                    }
                }
            }
        }

        // здесь основная территория геймплея

        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, MapController.mapWidth * 20, MapController.mapHeight * 30));

        }
    }
}
