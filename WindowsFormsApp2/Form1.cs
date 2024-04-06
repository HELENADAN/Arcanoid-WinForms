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

        public Label ScoreLabel;
        public Label score_label;
        public Label lives_lable;
        public Label pause;

        public Image Background;
        public Label Mode_Button;
       
        // конструктор формы
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;

            // общий фон
            Background = new Bitmap("C:\\Users\\Елена\\Desktop\\2курс(2)\\c#\\WindowsFormsApp2\\WindowsFormsApp2\\img_for_w2\\space.bmp");

            // надпись Score
            ScoreLabel = new Label();
            ScoreLabel.Location = new Point((MapController.mapWidth) * 20 + 1, 80);
            ScoreLabel.AutoSize = true;
            ScoreLabel.BackgroundImage = Background;
            ScoreLabel.ForeColor = Color.White;
            ScoreLabel.Font = new Font("Times new roman", 23, FontStyle.Bold);
            this.Controls.Add(ScoreLabel); // добавляем метки в форму --- в коллекцию элементов управления в форме

            //сами очки
            score_label = new Label();
            score_label.Location = new Point((MapController.mapWidth) * 20 + 1, 117);
            score_label.AutoSize = true;
            score_label.BackgroundImage = Background;
            score_label.ForeColor = Color.White;
            score_label.Font = new Font("Times new roman", 16, FontStyle.Bold);
            this.Controls.Add(score_label);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // heart
            lives_lable = new Label();
            lives_lable.Location = new Point((MapController.mapWidth) * 20 + 1, 25);
            lives_lable.AutoSize = true;
            lives_lable.BackgroundImage = Background;
            lives_lable.ForeColor = Color.Red;
            lives_lable.Font = new Font("Times new roman", 18, FontStyle.Bold);
            this.Controls.Add(lives_lable);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // надпись при нажатии PAUSED
            pause = new Label();
            pause.Location = new Point((MapController.mapWidth)*3, (MapController.mapHeight)*8);
            pause.ForeColor = Color.White;
            pause.Font = new Font("Times new roman", 50, FontStyle.Bold);
            pause.AutoSize = true;
            pause.BackgroundImage = Background;
            pause.Hide();
            this.Controls.Add(pause);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // кнопка смены состояний START - PAUSE - RESUME
            Mode_Button = new Label();
            Mode_Button.Location = new Point((MapController.mapWidth) * 20 + 11, 400);
            Mode_Button.Text= "▶";
            Mode_Button.Font = new Font("Times new roman", 35, FontStyle.Bold);
            Mode_Button.Size = new System.Drawing.Size(100,100);
            Mode_Button.BackgroundImage = Background;
            Mode_Button.ForeColor = Color.Red;
            Mode_Button.Click += Mode_Button_click;
            this.Controls.Add(Mode_Button);  // добавляем метки в форму --- в коллекцию элементов управления в форме

            //обработчик событий для элемента timer1 
            timer1.Tick += new EventHandler(update); // для движения мяча

            // обработчик событий для кнопок для движения платформы
            this.KeyDown += new KeyEventHandler(inputCheck);

            Init();

        }

        // движение платформы
        private void inputCheck(object sender, KeyEventArgs e)
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

        public void Message_box()
        {
            DialogResult result = MessageBox.Show(
                   "Your result: " + player.score + "\nLet's play it again?",
                   "Game over!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);


            if (player.lives <= 0)
            {
                if (result == DialogResult.Yes)
                {   
                    Init();                 
                }
                else
                {
                    this.Close();
                }
            }
        }

        // движение мячика
        public void update(object sender, EventArgs e)
        {
            // столкновение с нижней частью границы ---> Игра начинается заново, если мяч коснулся нижней границы
            if (player.BallY + player.dirY > MapController.mapHeight - 1)
            {
                player.lives--; // коснулись нижней части платформы -жизнь
                if (player.lives <= 0)
                    Message_box();
                   // Init();                              
                else 
                  Continue();
            }
            //очищаем предыдущее место
            map.map[player.BallY, player.BallX] = 0;
            // проверка на выход из карты - массива
            if (!physics.IsCollade(player,map,ScoreLabel, score_label))
            {
                // меняем координаты
                player.BallX += player.dirX;
            }
            if (!physics.IsCollade(player, map, ScoreLabel, score_label))
            {
                // меняем координаты
                player.BallY += player.dirY;
            }
            // задаем новое место
            map.map[player.BallY, player.BallX] = 8;

            // место размещения платформы на карте

            map.map[player.platformY, player.platformX] = 9; // левый конец платформы
            map.map[player.platformY, player.platformX + 1] = 99;// средина
            map.map[player.platformY, player.platformX + 2] = 999;// правый конец платформы

            Invalidate();//перерисовка холста
        }

        public void Mode_Button_click(object sender, EventArgs e)
        {

            if (Mode_Button.Text == "▶")
            {
                Mode_Button.Text = "⏸";
                pause.Hide();
                // место размещения мяча на карте
                map.map[player.BallY, player.BallX] = 8;

                // запуск таймера для осуществления цикла игры
                timer1.Start();
            }

            else
            {               
                Mode_Button.Text = "▶";
                timer1.Stop();
                pause.Show();
            }
          
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
                    int currPlatform = r.Next(1, 5);
                    map.map[i, j] = currPlatform;
                    map.map[i, j + 1] = currPlatform + currPlatform * 10; // для определения коллизий с мячом
                }
            }

        }
        public void Continue() // продолжаем игру не изменяя состояние карты
        {
            timer1.Interval = 50;// нужна для мячика
            ScoreLabel.Text = "Score";
            score_label.Text = player.score.ToString();
            lives_lable.Text = "";
            for (int i = 0; i < player.lives; i++)
                lives_lable.Text = lives_lable.Text + "♥";

            // место размещения платформы на карте

            map.map[player.platformY, player.platformX] = 9; // левый конец платформы
            map.map[player.platformY, player.platformX + 1] = 99;// средина
            map.map[player.platformY, player.platformX + 2] = 999;// правый конец платформы
            map.map[player.BallY, player.BallX] = 0;

            // задаем расположение мячика

            player.BallY = player.platformY - 1; // на строчку выше платформы расположен мяч
            player.BallX = player.platformX + 1; // мяч размещен по середине платформы

            // место размещения мяча на карте
            map.map[player.BallY, player.BallX] = 8;

            // реализация движения мячика
            player.dirX = 1;
            player.dirY = -1;

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

            this.Width = (MapController.mapWidth + 5) * 20;
            this.Height = (MapController.mapHeight + 2) * 20;

            timer1.Interval = 50;// нужна для мячика

            player.score = 0;
            player.lives = 5;
            pause.Text = "PAUSE";
            ScoreLabel.Text = "Score";
            score_label.Text = player.score.ToString();

            //pause.Text = "PAUSED";

            lives_lable.Text = "";
            for (int i = 0; i < player.lives; i++)
                lives_lable.Text = lives_lable.Text + "♥";

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

            player.BallY = player.platformY - 1; // на строчку выше платформы расположен мяч
            player.BallX = player.platformX + 1; // мяч размещен по середине платформы

            // реализация движения мячика
            player.dirX = 1;
            player.dirY = -1;

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
