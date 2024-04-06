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

        public MapController() 
        {
            // подгрузка картинки

            arcanoidSet = new Bitmap("C:\\Users\\Елена\\Desktop\\2курс(2)\\c#\\WindowsFormsApp2\\WindowsFormsApp2\\img_for_w2\\arcanoid.png");

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
                    int currPlatform = r.Next(1, 5);
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
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(60, 20)), new Rectangle(398, 17, 150, 50), GraphicsUnit.Pixel);
                    }

                    if (map[i, j] == 8)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(20, 20)), new Rectangle(806, 548, 73, 73), GraphicsUnit.Pixel);
                    }

                    if (map[i, j] == 1)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), new Rectangle(20, 16, 170, 59), GraphicsUnit.Pixel);
                    }
                    // разница между текущей платформой и следующей  77 пкс
                    if (map[i, j] == 2)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 3)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 4)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 5)
                    {
                        g.DrawImage(arcanoidSet, new Rectangle(new Point(j * 20, i * 20), new Size(40, 20)), 20, 16 + 77 * (map[i, j] - 1), 170, 59, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        // здесь основная территория геймплея

        public void DrawArea(Graphics g)
        {
            g.DrawRectangle(Pens.White, new Rectangle(0, 0, MapController.mapWidth * 20, MapController.mapHeight * 20));

        }
    }
}
