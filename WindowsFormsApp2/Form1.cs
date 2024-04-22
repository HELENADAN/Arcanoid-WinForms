using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp2

{   // В этом коде создается форма с игровым полем и элементами, такими как шарик и платформа. 
    public partial class Form1 : Form
    {

        MapController map;
        Player player;
        Physics2DController physics;

        private readonly Label ScoreLabel;
        private readonly Label ScoreNumberLabel;
        private readonly Label LivesLabel;
        private readonly Label PauseMessage;
        private readonly Label Game_over;

        private Image image = Image.FromFile("C:\\Users\\Елена\\Desktop\\Arc\\WindowsFormsApp2\\img_for_w2\\arca.jpg");
        private readonly Label StartPauseButton;
        private readonly Label RestartButton;

        private readonly Bitmap CroppedStartButton1;
        private readonly Bitmap CroppedPauseButton;
        private readonly Bitmap CroppedRestartButton;
        private readonly Bitmap CroppedEasyButton;
        private readonly Bitmap CroppedInformButton;
        private readonly Bitmap CroppedResaltButton;
        private readonly Bitmap CroppedExitButton;

        private readonly Label EasyLevel;
        private readonly Label MediumLevel;
        private readonly Label HardLevel;
        private readonly Label EasyLevelText;
        private readonly Label MediumLevelText;
        private readonly Label HardLevelText;

        private readonly Label InformButton;
        private readonly Label ResaltButton;
        private readonly Label ExitButton;
        private Image Image { get => image; set => image = value; }

        // конструктор формы
        public Form1()
        {
            InitializeComponent();

            // общий фон
            this.BackColor = Color.White;

            // надпись Score
            ScoreLabel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 6, 50),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 23, FontStyle.Bold)
            };
            this.Controls.Add(ScoreLabel); // добавляем метки в форму --- в коллекцию элементов управления в форме

            // количество заработанных очков
            ScoreNumberLabel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 4, 80),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 23, FontStyle.Bold)
            };
            this.Controls.Add(ScoreNumberLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // количество жизней

            LivesLabel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 5, 4),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 30, FontStyle.Regular)
            };
            this.Controls.Add(LivesLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // кнопка смены состояний START - PAUSE
            Rectangle CroppPauseButton = new Rectangle(598, 40, 115, 115);
            Rectangle CroppStartButton = new Rectangle(220, 40, 115, 115);

            CroppedStartButton1 = CropSprite(Image, CroppStartButton);
            CroppedPauseButton = CropSprite(Image, CroppPauseButton);

            StartPauseButton = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 40, 125),
                BackgroundImage = CroppedStartButton1,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            StartPauseButton.Click += StartPauseButtonClick;
            this.Controls.Add(StartPauseButton);

            // кнопка RESTART

            Rectangle CroppRestartButton = new Rectangle(415, 42, 115, 115);

            CroppedRestartButton = CropSprite(Image, CroppRestartButton);

            RestartButton = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 40, 300),
                BackgroundImage = CroppedRestartButton,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            RestartButton.Click += RestartButtonClick;
            this.Controls.Add(RestartButton);

            // кнопки Level

            Rectangle CroppEasyButton = new Rectangle(604, 240, 35, 35);

            CroppedEasyButton = CropSprite(Image, CroppEasyButton);

            EasyLevel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 30, 215),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(EasyLevel);

            EasyLevelText = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 55, 212),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Easy"
            };
            this.Controls.Add(EasyLevelText);

            MediumLevel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 30, 235),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(MediumLevel);

            MediumLevelText = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 55, 232),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Medium"
            };
            this.Controls.Add(MediumLevelText);

            HardLevel = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 30, 255),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(HardLevel);

            HardLevelText = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 55, 253),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Hard"
            };
            this.Controls.Add(HardLevelText);

            // кнопка Справка

            Rectangle CroppInformButton = new Rectangle(632, 347, 79, 112);

            CroppedInformButton = CropSprite(Image, CroppInformButton);

            InformButton = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 95, 395),
                BackgroundImage = CroppedInformButton,
                AutoSize = false,
                Size = new Size(45, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(InformButton);

            // кнопка Результат

            Rectangle CroppResaltButton = new Rectangle(395, 358, 135, 102);

            CroppedResaltButton = CropSprite(Image, CroppResaltButton);

            ResaltButton = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 5, 395),
                BackgroundImage = CroppedResaltButton,
                AutoSize = false,
                Size = new Size(80, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(ResaltButton);

            // кнопка Выход

            Rectangle CroppExitButton = new Rectangle(818, 347, 115, 115);

            CroppedExitButton = CropSprite(Image, CroppExitButton);

            ExitButton = new Label
            {
                Location = new Point((MapController.mapWidth) * 20 + 45, 500),
                BackgroundImage = CroppedExitButton,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(ExitButton);
            // надпись при нажатии PAUSED
            PauseMessage = new Label
            {
                Location = new Point((MapController.mapWidth) * 3, (MapController.mapHeight) * 8),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 50, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            PauseMessage.Hide();
            this.Controls.Add(PauseMessage);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // надпись Game Over
            Game_over = new Label
            {
                Location = new Point((MapController.mapWidth), (MapController.mapHeight) * 8),
                Text = "GAME OVER",
                Font = new Font("Times new roman", 40, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                AutoSize = true
            };
            Game_over.Hide();
            this.Controls.Add(Game_over);  // добавляем метки в форму --- в коллекцию элементов управления в форме

            //обработчик событий для элемента timer1 
            timer1.Tick += new EventHandler(Update); // для движения мяча

            // обработчик событий для кнопок для движения платформы
            this.KeyDown += new KeyEventHandler(InputCheck);
            Init();

        }
        public void StartPauseButtonClick(object sender, EventArgs e)
        {
            if (((Label)sender).BackgroundImage == CroppedStartButton1)
            {
                Game_over.Hide();
                PauseMessage.Hide();
                ((Label)sender).BackgroundImage = CroppedPauseButton;
                map.map[map.BallY, map.BallX] = 8;
                timer1.Start();
            }
            else
            {
                ((Label)sender).BackgroundImage = CroppedStartButton1;
                Game_over.Hide();
                timer1.Stop();
                PauseMessage.Show();
            }
        }

        public void RestartButtonClick(object sender, EventArgs e)
        {

        }

        // обрезка элементов спрайта
        public Bitmap CropSprite(Image image, Rectangle cropRectangle)
        {

            Bitmap croppedImage = new Bitmap(cropRectangle.Width, cropRectangle.Height);

            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.DrawImage(image, new Rectangle(0, 0, cropRectangle.Width, cropRectangle.Height), cropRectangle, GraphicsUnit.Pixel);
            }

            return croppedImage;
        }


        // движение платформы
        private void InputCheck(object sender, KeyEventArgs e)
        {

            // какая клавиша нажата
            // очистим предыдущее место размещение платформы
            map.map[player.platformY, player.platformX] = 0;
            map.map[player.platformY, player.platformX + 1] = 0;
            map.map[player.platformY, player.platformX + 2] = 0;

            switch (e.KeyCode)
            {   // если нажали кнопку вправо
                case Keys.Right:
                        if (player.platformX + 2 < MapController.mapWidth - 1)
                        // сдвинем координату платформы на единичку по оси x 
                             player.platformX++;
                    break;                 
                // если нажали кнопку влево
                case Keys.Left:
                    
                    if (player.platformX > 0)
                        // сдвинем координату платформы на единичку по оси x
                        player.platformX--;
                    break;                                     
            }

            //разместить платформу с учетом новых координат
            map.map[player.platformY, player.platformX] = 9;
            map.map[player.platformY, player.platformX + 1] = 99;
            map.map[player.platformY, player.platformX + 2] = 999;

        }

        // движение мячика
        public void Update(object sender, EventArgs e)
        {
            // столкновение с нижней частью границы ---> Игра начинается заново, если мяч коснулся нижней границы
            if (map.BallY + map.dirY > MapController.mapHeight - 1)
            {
                player.lives--; // коснулись нижней части платформы -жизнь
                if (player.lives <= 0)
                {
                    timer1.Stop();
                    Init();
                    Game_over.Show();                                                  
                }
                //                              
                else
                    
                    Continue();
            }
            //очищаем предыдущее место
            map.map[map.BallY, map.BallX] = 0;
            // проверка на выход из карты - массива
            if (!physics.IsCollade(player,map,ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                map.BallX += map.dirX;
            }
            if (!physics.IsCollade(player, map, ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                map.BallY += map.dirY;
            }
            // задаем новое место
            map.map[map.BallY, map.BallX] = 8;

            // место размещения платформы на карте

            map.map[player.platformY, player.platformX] = 9; // левый конец платформы
            map.map[player.platformY, player.platformX + 1] = 99;// средина
            map.map[player.platformY, player.platformX + 2] = 999;// правый конец платформы

            Invalidate();//перерисовка холста
        }

        // генерация препятствий для мячика
        public void GeneratePlatform() 
        {   
            Random r = new Random();
            
            // заполняем карту на треть платформами для игры
            for (int i = 0; i < MapController.mapHeight / 3; i++)
            {
                for (int j = 0; j < MapController.mapWidth; j += 2) // так препятствие состоит из 2 частей
                {
                    int currPlatform = r.Next(1, 3);
                    map.map[i, j] = currPlatform;
                    map.map[i, j + 1] = currPlatform + currPlatform * 10; // для определения коллизий с мячом
                }
            }

        }
        public void Continue() // продолжаем игру не изменяя состояние карты
        {
            timer1.Interval = 50;// нужна для мячика
            ScoreLabel.Text = "Score";
            ScoreNumberLabel.Text = player.score.ToString();
            LivesLabel.Text = "";
            for (int i = 0; i < player.lives; i++)
                LivesLabel.Text += "♥";

            // место размещения платформы на карте

            map.map[player.platformY, player.platformX] = 9; // левый конец платформы
            map.map[player.platformY, player.platformX + 1] = 99;// средина
            map.map[player.platformY, player.platformX + 2] = 999;// правый конец платформы
            map.map[map.BallY, map.BallX] = 0;

            // задаем расположение мячика

            map.BallY = player.platformY - 1; // на строчку выше платформы расположен мяч
            map.BallX = player.platformX + 1; // мяч размещен по середине платформы

            // место размещения мяча на карте
            map.map[map.BallY, map.BallX] = 8;

            // реализация движения мячика
            map.dirX = 1;
            map.dirY = -1;

            // запуск таймера для осуществления цикла игры
            timer1.Start();
        }

        // метод для инициализации
        public void Init()
        {

            map = new MapController(); // создадим новый объект класса MapController
            player = new Player();
            physics = new Physics2DController();

            // вычисление размеров окна, которые будут подстраиваться под нашу карту

            this.Width = (MapController.mapWidth + 8) * 20;
            this.Height = (MapController.mapHeight + 2) * 20;

            timer1.Interval = 50;// нужна для мячика

            player.score = 0;
            player.lives = 5;
            PauseMessage.Text = "PAUSE";
            ScoreLabel.Text = "SCORES";
            ScoreNumberLabel.Text = player.score.ToString();

            //PauseMessage.Text = "PAUSED";

            LivesLabel.Text = "";
            for (int i = 0; i < player.lives; i++)
                LivesLabel.Text += "♥";

            // заполняем массив карты нулями
            for (int i = 0; i < MapController.mapHeight; i++)// двигается по х
            {
                for (int j = 0; j < MapController.mapWidth; j++)// двигается по у
                {
                    map.map[i, j] = 0;// заполняем двумерный массив нулями
                }
            }

            // расположение платформы относительно карты --- по центру нужно

            player.platformX = (MapController.mapWidth - 1) / 2;
            player.platformY = MapController.mapHeight - 1;

            // место размещения платформы на карте

            map.map[player.platformY, player.platformX] = 9; // левый конец платформы
            map.map[player.platformY, player.platformX + 1] = 99;// средина
            map.map[player.platformY, player.platformX + 2] = 999;// правый конец платформы

            // задаем расположение мячика

            map.BallY = player.platformY - 1; // на строчку выше платформы расположен мяч
            map.BallX = player.platformX + 1; // мяч размещен по середине платформы

            // реализация движения мячика
            map.dirX = 1;
            map.dirY = -1;

            GeneratePlatform();

        }

        /// отрисовка элементов
        private void OnPaint(object sender, PaintEventArgs e)
        {
            map.DrawArea(e.Graphics);// рисуем границы игрового поля
            map.DrawMap(e.Graphics);// рисуем карту
        }
    }   
}
