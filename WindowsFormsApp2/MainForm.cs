using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using ToolTip = System.Windows.Forms.ToolTip;

namespace WindowsFormsApp2

{   // В этом коде создается форма с игровым полем и элементами, такими как шарик и платформа. 
    public partial class MainForm : Form
    {

        GameField field;
        GameLogic logic;

        public readonly Label ScoreLabel;
        public readonly Label ScoreNumberLabel;
        public readonly Label LivesLabel;
        public readonly Label PauseMessage;
        public readonly Label Game_over;

        private Image image = Image.FromFile("C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\img_for_w2\\arca.jpg");
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
        private readonly Label ResultButton;
        private readonly Label ExitButton;
        private Image Image { get => image; set => image = value; }

        string filePath = "C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\Results.txt";
        public int FinalPlayersScore;

        // конструктор формы
        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            Createtimer();
            logic = new GameLogic();        
            field = new GameField();

            // вычисление размеров окна, которые будут подстраиваться под нашу карту
            this.Width = (field.Width + 8) * 20;
            this.Height = (field.Height + 2) * 20;

            // общий фон
            this.BackColor = Color.White;

            // надпись Score
            ScoreLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 6, 50),
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
                Location = new Point((field.Width) * 20 + 4, 80),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 23, FontStyle.Bold),
                Text = logic.GetPlayerScore().ToString()
            };
            this.Controls.Add(ScoreNumberLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // количество жизней

            LivesLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 5, 4),
                AutoSize = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 30, FontStyle.Regular),
                Text = ""
           
            };

            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            this.Controls.Add(LivesLabel);// добавляем метки в форму --- в коллекцию элементов управления в форме

            // кнопка смены состояний START - PAUSE
            Rectangle CroppPauseButton = new Rectangle(598, 40, 115, 115);
            Rectangle CroppStartButton = new Rectangle(220, 40, 115, 115);

            CroppedStartButton1 = CropSprite(Image, CroppStartButton);
            CroppedPauseButton = CropSprite(Image, CroppPauseButton);

            StartPauseButton = new Label
            {
                Location = new Point((field.Width) * 20 + 40, 125),
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
                Location = new Point((field.Width) * 20 + 40, 300),
                BackgroundImage = CroppedRestartButton,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            RestartButton.Click += RestartButtonClick;
            this.Controls.Add(RestartButton);
            ToolTip toolTip_res = new ToolTip();
            toolTip_res.SetToolTip(RestartButton, "Начать заново");

            // кнопки Level

            Rectangle CroppEasyButton = new Rectangle(604, 240, 35, 35);

            CroppedEasyButton = CropSprite(Image, CroppEasyButton);

            EasyLevel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 215),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(EasyLevel);
            ToolTip toolTip_easyl = new ToolTip();
            toolTip_easyl.SetToolTip(EasyLevel, "Простой уровень");

            EasyLevelText = new Label
            {
                Location = new Point((field.Width) * 20 + 55, 212),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Easy"
            };
            this.Controls.Add(EasyLevelText);
            ToolTip toolTip_easy = new ToolTip();
            toolTip_easy.SetToolTip(EasyLevelText, "Простой уровень");

            MediumLevel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 235),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(MediumLevel);
            ToolTip toolTip_mediuml = new ToolTip();
            toolTip_mediuml.SetToolTip(MediumLevel, "Средний уровень");

            MediumLevelText = new Label
            {
                Location = new Point((field.Width) * 20 + 55, 232),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Medium"
            };
            this.Controls.Add(MediumLevelText);
            ToolTip toolTip_medium = new ToolTip();
            toolTip_medium.SetToolTip(MediumLevelText, "Средний уровень");

            HardLevel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 255),
                BackgroundImage = CroppedEasyButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(HardLevel);
            ToolTip toolTip_hardl = new ToolTip();
            toolTip_hardl.SetToolTip(HardLevel, "Сложный уровень");

            HardLevelText = new Label
            {
                Location = new Point((field.Width) * 20 + 55, 253),
                ForeColor = Color.Black,
                Font = new Font("Times new roman", 14, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.White,
                Text = "Hard"
            };
            this.Controls.Add(HardLevelText);
            ToolTip toolTip_hard = new ToolTip();
            toolTip_hard.SetToolTip(HardLevelText, "Сложный уровень");

            // кнопка Справка

            Rectangle CroppInformButton = new Rectangle(632, 347, 79, 112);

            CroppedInformButton = CropSprite(Image, CroppInformButton);

            InformButton = new Label
            {
                Location = new Point((field.Width) * 20 + 95, 395),
                BackgroundImage = CroppedInformButton,
                AutoSize = false,
                Size = new Size(45, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(InformButton);
            InformButton.Click += InformButtonClick;
            ToolTip toolTip_inf = new ToolTip();
            toolTip_inf.SetToolTip(InformButton, "Об игре");

            // кнопка Результат

            Rectangle CroppResaltButton = new Rectangle(395, 358, 135, 102);

            CroppedResaltButton = CropSprite(Image, CroppResaltButton);

            ResultButton = new Label
            {
                Location = new Point((field.Width) * 20 + 5, 395),
                BackgroundImage = CroppedResaltButton,
                AutoSize = false,
                Size = new Size(80, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(ResultButton);
            ResultButton.Click += ResultButtonClick;
            ToolTip toolTip_result = new ToolTip();
            toolTip_result.SetToolTip(ResultButton, "Лучшие результаты");

            // кнопка Выход

            Rectangle CroppExitButton = new Rectangle(818, 347, 115, 115);

            CroppedExitButton = CropSprite(Image, CroppExitButton);

            ExitButton = new Label
            {
                Location = new Point((field.Width) * 20 + 45, 500),
                BackgroundImage = CroppedExitButton,
                AutoSize = false,
                Size = new Size(60, 60),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(ExitButton);
            ToolTip toolTip_exit = new ToolTip();
            toolTip_exit.SetToolTip(ExitButton, "Выход");
            ExitButton.Click += ExitButtonClick;

            // надпись при нажатии PAUSED
            PauseMessage = new Label
            {
                Location = new Point((field.Width) * 3, (field.Height) * 8),
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
                Location = new Point((field.Width), (field.Height) * 8),
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
                field.field[field.BallY, field.BallX] = 8;
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

        public void InformButtonClick(object sender, EventArgs e)
        {
            // Создание формы
            Form InformForm = new Form();
            InformForm.Text = "Об игре";
            InformForm.Width = 700;
            InformForm.Height = 550;
            InformForm.AutoSize = true; // Автоматическое изменение размера формы в зависимости от содержимого

            GroupBox name = new GroupBox();
            name.Text = "Арканоид";
            name.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            name.Location = new System.Drawing.Point(10, 10);
            name.Size = new System.Drawing.Size(InformForm.Width - 20, 90);
            InformForm.Controls.Add(name);

            Label gamename = new Label();
            gamename.Text = "Классическая аркадная игра, целью которой является разрушение всех блоков на экране с помощью мяча, отскакивающего от платформы, управляемой игроком.";
            gamename.Location = new System.Drawing.Point(10, 20);
            gamename.Font = new Font("Times New Roman", 11, FontStyle.Regular);
            gamename.Size = new System.Drawing.Size(name.Width - 20, 90);
            name.Controls.Add(gamename);

            GroupBox logic = new GroupBox();
            logic.Text = "Логика";
            logic.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            logic.Location = new System.Drawing.Point(10, name.Bottom + 10);
            logic.Size = new System.Drawing.Size(InformForm.Width - 20,140); 
            InformForm.Controls.Add(logic);

            Label gameLogic = new Label();
            gameLogic.Text = "В игре Арканоид игрок управляет платформой, которая движется горизонтально вдоль нижней части экрана. Целью игры является отбивание мяча, чтобы разрушить все блоки на экране. Мяч отскакивает от платформы и стен, а при попадании на блок разрушает его. Игрок должен предотвращать падение мяча за нижнюю границу экрана, в противном случае он потеряет одну из своих жизней.";
            gameLogic.Location = new System.Drawing.Point(10, 20);
            gameLogic.Size = new System.Drawing.Size(logic.Width - 20, 140);
            gameLogic.Font = new Font("Times New Roman", 11, FontStyle.Regular);
            logic.Controls.Add(gameLogic);

            GroupBox aboutGame = new GroupBox();
            aboutGame.Text = "Управление";
            aboutGame.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            aboutGame.Location = new System.Drawing.Point(10, logic.Bottom + 10);
            aboutGame.Size = new System.Drawing.Size(InformForm.Width - 20, 60);
            InformForm.Controls.Add(aboutGame);

            Label work = new Label();
            work.Text = "Для управления платформой используются стрелки влево-вправо на клавиатуре.";
            work.Location = new System.Drawing.Point(10, 20);
            work.Size = new System.Drawing.Size(aboutGame.Width - 20, 60);
            work.Font = new Font("Times New Roman", 11, FontStyle.Regular);
            aboutGame.Controls.Add(work);

            GroupBox Author = new GroupBox();
            Author.Text = "Автор";
            Author.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            Author.Location = new System.Drawing.Point(10, aboutGame.Bottom + 10);
            Author.Size = new System.Drawing.Size(InformForm.Width - 20, 40);
            InformForm.Controls.Add(Author);

            Label AboutAuthor = new Label();
            AboutAuthor.Text = "студентка гр. 2-80 Данченко Е.А.";
            AboutAuthor.Location = new System.Drawing.Point(10, 20);
            AboutAuthor.Size = new System.Drawing.Size(Author.Width - 20, 40);
            AboutAuthor.Font = new Font("Times New Roman", 11, FontStyle.Regular);
            Author.Controls.Add(AboutAuthor);

            GroupBox Year = new GroupBox();
            Year.Text = "Год создания";
            Year.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            Year.Location = new System.Drawing.Point(10, Author.Bottom + 10);
            Year.Size = new System.Drawing.Size(InformForm.Width - 20, 40);
            InformForm.Controls.Add(Year);

            Label AboutYear = new Label();
            AboutYear.Text = "2024";
            AboutYear.Location = new System.Drawing.Point(10, 20);
            AboutYear.Size = new System.Drawing.Size(Year.Width - 20, 40);
            AboutYear.Font = new Font("Times New Roman", 11, FontStyle.Regular);
            Year.Controls.Add(AboutYear);

            InformForm.Show();
        }
        public void ExitButtonClick(object sender, EventArgs e)
        {             
               this.Close();
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
            field.field[field.platformY, field.platformX] = 0;
            field.field[field.platformY, field.platformX + 1] = 0;
            field.field[field.platformY, field.platformX + 2] = 0;

            switch (e.KeyCode)
            {   // если нажали кнопку вправо
                case Keys.Right:
                        if (field.platformX + 4 < field.Width - 1)
                        // сдвинем координату платформы на единичку по оси x 
                             field.platformX+=2;
                    break;                 
                // если нажали кнопку влево
                case Keys.Left:
                    
                    if (field.platformX > 1)
                        // сдвинем координату платформы на единичку по оси x
                        field.platformX -= 2;
                    break;                                     
            }

            //разместить платформу с учетом новых координат
            field.field[field.platformY, field.platformX] = 9;
            field.field[field.platformY, field.platformX + 1] = 99;
            field.field[field.platformY, field.platformX + 2] = 999;
                
        }
      
        // обработка коллизий мяча и обновление карты 
        public void Update(object sender, EventArgs e)
        {
            HandleBottomCollision(); // Обрабатывает столкновение с нижней границей карты
            ClearPreviousBallPosition(); // Очищает предыдущее положение мяча
            HandleBallCollision(); // Обработка коллизий мяча
            UpdateBallCoordinates();// Обновление координат мяча
            UpdatePlatformCoordinates();// Обновляет местоположение платформы на карте
            Invalidate();
        }
        private void HandleBottomCollision()
        {
            // столкновение с нижней частью границы ---> Игра начинается заново, если мяч коснулся нижней границы

            if (field.BallY + field.dirY > field.Height - 1)
            {
                logic.DamagePlayer();
                
                if (logic.GetPlayerLives() <= 0)                  
                {
                    FinalPlayersScore = logic.GetPlayerScore();
                    AddResults(FinalPlayersScore);
                    timer1.Stop();
                    StartPauseButton.BackgroundImage = CroppedStartButton1;
                    field = new GameField();
                    Game_over.Show();                   
                }
                else
                {
                    Continue();
                }
            }
        }
        public void AddResults(int FinalPlayersScore)
        {
            // Создание массива фиксированной длины из 10 чисел
            int[] numbersFromFile = new int[11];
            int currentIndex = 0;
            int newNumber = FinalPlayersScore;

            // Чтение 11 чисел из файла

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null && currentIndex < 11)
                {
                    if (int.TryParse(line, out int number))
                    {
                        numbersFromFile[currentIndex] = number;
                        numbersFromFile[10] = newNumber;
                        currentIndex++;
                    }
                }
            }

            // Добавление полученных очков в массив после сравнения (сортировка вставкой)

            for (int i = 1; i < numbersFromFile.Length; i++) // Начинаем с индекса 1 (так как элемент с индексом 0 считается уже отсортированным). Проходим по массиву слева направо.
            {
                int key = numbersFromFile[i]; // Текущий элемент, который нужно вставить в правильное место

                int j = i - 1;// Индекс элемента, перед которым нужно вставить текущий элемент

                // Сравнение и сдвиг элементов влево, пока текущий элемент больше предыдущего
                while (j >= 0 && numbersFromFile[j] < key)
                {
                    numbersFromFile[j + 1] = numbersFromFile[j]; //  Обмен местами текущего элемента и предыдущего элемента.
                    j--;
                }

                numbersFromFile[j + 1] = key; // Вставка текущего элемента на его правильное место после того, как он был сдвинут влево.
            }

            // Запись массива обратно в файл
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (int num in numbersFromFile)
                {
                    sw.WriteLine(num);
                }
            }           
        }
        private void ResultButtonClick(object sender, EventArgs e)
        {

            // Создание формы
            Form ResultsForm = new Form();
            ResultsForm.Text ="Лучшие результаты";

            Label[] labels = new Label[10];

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Location = new Point(ResultsForm.Width / 2, 25*i);
                ResultsForm.Controls.Add(labels[i]);
            }

            // Показ файла

            using (StreamReader reader = new StreamReader(filePath))
            {
                int index = 0;// для отслеживания текущего индекса в массиве label
                string line; // для хранения строки, прочитанной из файла

                while (index < labels.Length && (line = reader.ReadLine()) != null)
                {   
                    if (line != "" && int.Parse(line) != 0) labels[index].Text = line;
                    index++;
                }
            }

            // Вывод формы
            ResultsForm.Show();
        } 
        private void ClearPreviousBallPosition()
        {
            field.field[field.BallY, field.BallX] = 0;
        }
        private void HandleBallCollision()
        {
            if (!logic.IsCollade(field, ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                field.BallX += field.dirX;
            }
            if (!logic.IsCollade(field, ScoreLabel, ScoreNumberLabel))
            {
                // меняем координаты
                field.BallY += field.dirY;
            }
        }
        private void UpdateBallCoordinates()
        {
            // Обновляет координаты мяча на карте
            // задаем новое место
            field.field[field.BallY, field.BallX] = 8;
        }
        private void UpdatePlatformCoordinates()
        {
            // Размещает платформу на карте с помощью специальных маркеров для разных частей платформы
            field.field[field.platformY, field.platformX] = 9; // левый конец платформы
            field.field[field.platformY, field.platformX + 1] = 99;// средина
            field.field[field.platformY, field.platformX + 2] = 999;// правый конец платформы
        }

        // продолжаем игру не изменяя состояние карты
        public void Continue()
        {
            timer1.Interval = 50;// нужна для мячика
            ScoreLabel.Text = "Score";
            ScoreNumberLabel.Text = logic.GetPlayerScore().ToString();
            LivesLabel.Text = "";
            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            // место размещения платформы на карте

            field.field[field.platformY, field.platformX] = 9; // левый конец платформы
            field.field[field.platformY, field.platformX + 1] = 99;// средина
            field.field[field.platformY, field.platformX + 2] = 999;// правый конец платформы
            field.field[field.BallY, field.BallX] = 0;

            // задаем расположение мячика

            field.BallY = field.platformY - 4; // на строчку выше платформы расположен мяч
            field.BallX = field.platformX + 1; // мяч размещен по середине платформы

            // место размещения мяча на карте
            field.field[field.BallY, field.BallX] = field.GetBallCode();

            // реализация движения мячика
            field.dirX = 1;
            field.dirY = -1;

            // запуск таймера для осуществления цикла игры
            timer1.Start();
        }

        // отрисовка элементов
        public void OnPaint(object sender, PaintEventArgs e)
        {
            field.DrawArea(e.Graphics);// рисуем границы игрового поля
            field.DrawMap(e.Graphics);// рисуем карту
        }
        
    }   
}
