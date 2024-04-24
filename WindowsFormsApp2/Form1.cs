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
        Physics2DController physics;

        public readonly Label ScoreLabel;
        public readonly Label ScoreNumberLabel;
        public readonly Label LivesLabel;
        public readonly Label PauseMessage;
        public readonly Label Game_over;

        private Image image = Image.FromFile("C:\\Users\\Елена\\Desktop\\Arc\\WindowsFormsApp2\\img_for_w2\\arca.jpg");
        public readonly Label StartPauseButton;
        private readonly Label RestartButton;

        public readonly Bitmap CroppedStartButton1;
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
            this.KeyPreview = true;
            Createtimer();
            physics = new Physics2DController();        
            map = new MapController();

            // вычисление размеров окна, которые будут подстраиваться под нашу карту
            this.Width = (map.Width + 8) * 20;
            this.Height = (map.Height + 2) * 20;

            // общий фон
            this.BackColor = Color.White;

            // надпись Score
            ScoreLabel = new Label
            {
                Location = new Point((map.Width) * 20 + 6, 50),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 23, FontStyle.Bold),
                Text = "SCORES"
            };
            this.Controls.Add(ScoreLabel); // добавляем метки в форму --- в коллекцию элементов управления в форме

            // количество заработанных очков
            ScoreNumberLabel = new Label
            {
                Location = new Point((map.Width) * 20 + 4, 80),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 23, FontStyle.Bold),
                Text = physics.GetPlayerScore().ToString()
            };
            this.Controls.Add(ScoreNumberLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // количество жизней

            LivesLabel = new Label
            {
                Location = new Point((map.Width) * 20 + 5, 4),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 30, FontStyle.Regular),
                Text = ""
           
            };

            for (int i = 0; i < physics.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            this.Controls.Add(LivesLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // кнопка смены состояний START - PAUSE
            Rectangle CroppPauseButton = new Rectangle(598, 40, 115, 115);
            Rectangle CroppStartButton = new Rectangle(220, 40, 115, 115);

            CroppedStartButton1 = CropSprite(Image, CroppStartButton);
            CroppedPauseButton = CropSprite(Image, CroppPauseButton);

            StartPauseButton = new Label
            {
                Location = new Point((map.Width) * 20 + 40, 125),
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
                Location = new Point((map.Width) * 20 + 40, 300),
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
                Location = new Point((map.Width) * 20 + 30, 215),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(EasyLevel);

            EasyLevelText = new Label
            {
                Location = new Point((map.Width) * 20 + 55, 212),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Easy"
            };
            this.Controls.Add(EasyLevelText);

            MediumLevel = new Label
            {
                Location = new Point((map.Width) * 20 + 30, 235),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(MediumLevel);

            MediumLevelText = new Label
            {
                Location = new Point((map.Width) * 20 + 55, 232),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Medium"
            };
            this.Controls.Add(MediumLevelText);

            HardLevel = new Label
            {
                Location = new Point((map.Width) * 20 + 30, 255),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(HardLevel);

            HardLevelText = new Label
            {
                Location = new Point((map.Width) * 20 + 55, 253),
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
                Location = new Point((map.Width) * 20 + 95, 395),
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
                Location = new Point((map.Width) * 20 + 5, 395),
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
                Location = new Point((map.Width) * 20 + 45, 500),
                BackgroundImage = CroppedExitButton,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(ExitButton);
            // надпись при нажатии PAUSED
            PauseMessage = new Label
            {
                Location = new Point((map.Width) * 3, (map.Height) * 8),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 50, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                Text = "PAUSE"
            };
            PauseMessage.Hide();
            this.Controls.Add(PauseMessage);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // надпись Game Over
            Game_over = new Label
            {
                Location = new Point((map.Width), (map.Height) * 8),
                Text = "GAME OVER",
                Font = new Font("Times new roman", 40, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                AutoSize = true
            };
            Game_over.Hide();
            this.Controls.Add(Game_over);  // добавляем метки в форму --- в коллекцию элементов управления в форме
           
            // обработчик событий для кнопок для движения платформы
            this.KeyDown += new KeyEventHandler(InputCheck);
        }

        //для элемента timer1 
        public void Createtimer()
        {           
            timer1.Tick += new EventHandler(Update); // для движения мяча
            timer1.Interval = 50;// нужна для мячика
        }
        public void StartPauseButtonClick(object sender, EventArgs e)
        {
            if (((Label)sender).BackgroundImage == CroppedStartButton1 ) 
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
            Application.Restart();
        }

        // обрезка элементов спрайта
        private Bitmap CropSprite(Image image, Rectangle cropRectangle)
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
            map.map[map.platformY, map.platformX] = 0;
            map.map[map.platformY, map.platformX + 1] = 0;
            map.map[map.platformY, map.platformX + 2] = 0;

            switch (e.KeyCode)
            {   // если нажали кнопку вправо
                case Keys.Right:
                        if (map.platformX + 4 < map.Width - 1)
                        // сдвинем координату платформы на единичку по оси x 
                             map.platformX+=2;
                    break;                 
                // если нажали кнопку влево
                case Keys.Left:
                    
                    if (map.platformX > 1)
                        // сдвинем координату платформы на единичку по оси x
                        map.platformX -= 2;
                    break;                                     
            }

            //разместить платформу с учетом новых координат
            map.map[map.platformY, map.platformX] = 9;
            map.map[map.platformY, map.platformX + 1] = 99;
            map.map[map.platformY, map.platformX + 2] = 999;
                
        }

        
        // движение мячика
        public void Update(object sender, EventArgs e)
        {
            // столкновение с нижней частью границы ---> Игра начинается заново, если мяч коснулся нижней границы
            if (map.BallY + map.dirY > map.Height - 1)
            {
                physics.DamagePlayer();

                // коснулись нижней части платформы -жизнь
                if (physics.GetPlayerLives() <= 0)
                {
                    timer1.Stop();
                    StartPauseButton.BackgroundImage = CroppedStartButton1;
                    map = new MapController();
                    Game_over.Show();
                }
                else
                    Continue();
            }
            //очищаем предыдущее место
            map.map[map.BallY, map.BallX] = 0;

            // проверка на выход из карты - массива
            if (!physics.IsCollade(map, ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                map.BallX += map.dirX;
            }
            if (!physics.IsCollade(map, ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                map.BallY += map.dirY;
            }
            // задаем новое место
            map.map[map.BallY, map.BallX] = 8;

            // место размещения платформы на карте

            map.map[map.platformY, map.platformX] = 9; // левый конец платформы
            map.map[map.platformY, map.platformX + 1] = 99;// средина
            map.map[map.platformY, map.platformX + 2] = 999;// правый конец платформы

            Invalidate();
        }

        // продолжаем игру не изменяя состояние карты
        public void Continue()
        {
            timer1.Interval = 50;// нужна для мячика
            ScoreLabel.Text = "Score";
            ScoreNumberLabel.Text = physics.GetPlayerScore().ToString();
            LivesLabel.Text = "";
            for (int i = 0; i < physics.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            // место размещения платформы на карте

            map.map[map.platformY, map.platformX] = 9; // левый конец платформы
            map.map[map.platformY, map.platformX + 1] = 99;// средина
            map.map[map.platformY, map.platformX + 2] = 999;// правый конец платформы
            map.map[map.BallY, map.BallX] = 0;

            // задаем расположение мячика

            map.BallY = map.platformY - 4; // на строчку выше платформы расположен мяч
            map.BallX = map.platformX + 1; // мяч размещен по середине платформы

            // место размещения мяча на карте
            map.map[map.BallY, map.BallX] = 8;

            // реализация движения мячика
            map.dirX = 1;
            map.dirY = -1;

            // запуск таймера для осуществления цикла игры
            timer1.Start();
        }

        // отрисовка элементов
        public void OnPaint(object sender, PaintEventArgs e)
        {
            map.DrawArea(e.Graphics);// рисуем границы игрового поля
            map.DrawMap(e.Graphics);// рисуем карту
        }
    }   
}
