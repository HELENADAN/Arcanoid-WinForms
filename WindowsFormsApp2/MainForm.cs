using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;
using ToolTip = System.Windows.Forms.ToolTip;

namespace WindowsFormsApp2

{   
    // В этом коде создается форма с игровым полем и элементами
    public partial class MainForm : Form
    {
        GameField field = new GameField();
        readonly GameLogic logic;

        private readonly Label ScoreLabel;
        private readonly Label ScoreNumberLabel;
        private readonly Label LivesLabel;
        private readonly Label PauseMessage;
        private readonly Label Game_over;

        private Image image = Image.FromFile("C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\img_for_w2\\arca.jpg");
        private readonly Label StartPauseButton;
        private readonly Label RestartButton;

        private readonly Bitmap CroppedStartButton1;
        private readonly Bitmap CroppedPauseButton;
        private readonly Bitmap CroppedRestartButton;
        private readonly Bitmap CroppedUncheckedButton;
        private readonly Bitmap CroppedInformButton;
        private readonly Bitmap CroppedResaltButton;
        private readonly Bitmap CroppedExitButton;
        private readonly Bitmap CroppedCheckedButton;

        private readonly Label EasyUncheckedLabel;
        private readonly Label MediumUncheckedLabel;
        private readonly Label HardUncheckedLabel;
        private readonly Label EasyLevelText;
        private readonly Label MediumLevelText;
        private readonly Label HardLevelText;
        private readonly Label EasyCheckedLabel;
        private readonly Label MediumCheckedLabel;
        private readonly Label HardCheckedLabel;

        private readonly Label InformButton;
        private readonly Label ResultButton;
        private readonly Label ExitButton;
        private readonly Label GoBallLabel;
        private Image Image { get => image; set => image = value; }

        private readonly string filePath = "C:\\Users\\Елена\\Desktop\\Arcanoid\\WindowsFormsApp2\\Results.txt";
        private int FinalPlayersScore;
        private readonly int[] numbersFromFile = new int[11];
        private readonly string[] namesFromFile = new string[11];
        private string UserName = "noname";

        private bool blinking = false;
        private bool ballMoves = false;
        private bool paused = false;

        DifficultyLevel selectedLevel = DifficultyLevel.Medium;

        // конструктор формы
        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            Createtimer();
            field = new GameField();
            logic = new GameLogic(field);        
            
            field.GenerateBlocks(selectedLevel);
           
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

            //////////////////// кнопки Level ///////////////////////////
            /////////////////// Easy Lvl ///////////////////////////////
            
            Rectangle CroppUncheckedButton = new Rectangle(604, 240, 35, 35);
            CroppedUncheckedButton = CropSprite(Image, CroppUncheckedButton);

            Rectangle CroppCheckedButton = new Rectangle(674, 238, 35, 35);
            CroppedCheckedButton = CropSprite(Image, CroppCheckedButton);

            EasyUncheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 215),
                BackgroundImage = CroppedUncheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(EasyUncheckedLabel);
            ToolTip toolTip_easyl = new ToolTip();
            toolTip_easyl.SetToolTip(EasyUncheckedLabel, "Простой уровень");
            EasyUncheckedLabel.Click += EasyCheck;

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
            EasyLevelText.Click += EasyCheck;
           
            EasyCheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 215),
                BackgroundImage = CroppedCheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(EasyCheckedLabel);
            ToolTip toolTip_easy_choose = new ToolTip();
            toolTip_easy_choose.SetToolTip(EasyCheckedLabel, "Простой уровень");
            EasyCheckedLabel.Hide();
            

            ///////////////////////// Medium Lvl /////////////////////////

            MediumUncheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 235),
                BackgroundImage = CroppedUncheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(MediumUncheckedLabel);
            ToolTip toolTip_mediuml = new ToolTip();
            toolTip_mediuml.SetToolTip(MediumUncheckedLabel, "Средний уровень");
            MediumUncheckedLabel.Click += MediumCheck;
            MediumUncheckedLabel.Hide();

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
            MediumLevelText.Click += MediumCheck;

            MediumCheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 235),
                BackgroundImage = CroppedCheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(MediumCheckedLabel);
            ToolTip toolTip_medium_choose = new ToolTip();
            toolTip_medium_choose.SetToolTip(MediumCheckedLabel, "Средний уровень");
            MediumCheckedLabel.Show();

            ///////////////////////// Hard Lvl /////////////////////////
            
            HardUncheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 255),
                BackgroundImage = CroppedUncheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(HardUncheckedLabel);
            ToolTip toolTip_hardl = new ToolTip();
            toolTip_hardl.SetToolTip(HardUncheckedLabel, "Сложный уровень");
            HardUncheckedLabel.Click += HardCheck;

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
            HardLevelText.Click += HardCheck;

            HardCheckedLabel = new Label
            {
                Location = new Point((field.Width) * 20 + 30, 255),
                BackgroundImage = CroppedCheckedButton,
                AutoSize = false,
                Size = new Size(20, 20),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(HardCheckedLabel);
            ToolTip toolTip_hard_choose = new ToolTip();
            toolTip_hard_choose.SetToolTip(HardCheckedLabel, "Сложный уровень");
            HardCheckedLabel.Hide();

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

            // надпись с пояснениями как начать игру
            GoBallLabel = new Label
            {
                Location = new Point((field.Width/2) +20, (field.Height) * 8),
                Text = "Нажмите пробел для запуска мяча",
                Font = new Font("Times new roman", 15, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                AutoSize = true
            };
            GoBallLabel.Hide();
            this.Controls.Add(GoBallLabel);  // добавляем метки в форму --- в коллекцию элементов управления в форме

            // обработчик событий для кнопок для движения платформы
            this.KeyDown += InputCheck;

            Timer_Tick(null, null);

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    int index = 0;// для отслеживания текущего индекса в массиве label
                    string line; // для хранения строки, прочитанной из файла

                    while (index < numbersFromFile.Length && (line = reader.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            int num = int.Parse(line.Substring(line.IndexOf(" ") + 1));
                            if (num != 0)
                            {
                                numbersFromFile[index] = num;
                                namesFromFile[index] = line.Substring(0, line.IndexOf(" "));
                            }
                        }
                        index++;
                    }
                }
            }
            catch (FileNotFoundException) {}
        }

        //для элемента timer1 
        public void Timer_Tick(object sender, EventArgs e)
        {
            if (blinking)
            {
                // Switch visibility of the label using Invoke to access it from the UI thread
                GoBallLabel.Invoke((MethodInvoker)delegate {
                    GoBallLabel.Visible = !GoBallLabel.Visible;
                });
            }

            // Schedule the next tick
            System.Threading.Timer timer = new System.Threading.Timer((state) =>
            {
                Timer_Tick(null, null);
            }, null, 400, Timeout.Infinite);
        }
        //для элемента timer_Tick
        public void Createtimer()
        {
            timer1.Tick += new EventHandler(Update); // для движения мяча
            timer1.Interval = 50;// нужна для мячика
        }
        private void EasyCheck(object sender, EventArgs e)
        {           
            EasyCheckedLabel.Show();
            EasyUncheckedLabel.Hide();
            MediumCheckedLabel.Hide();
            MediumUncheckedLabel.Show();
            HardCheckedLabel.Hide();
            HardUncheckedLabel.Show();
            selectedLevel = DifficultyLevel.Easy;
            field.GenerateBlocks(selectedLevel);
            paused = false;
            ballMoves = false;
            blinking = true;
            StartPauseButton.BackgroundImage = CroppedPauseButton;

            ScoreNumberLabel.Text = "0";
            logic.RefreshPlayerScore();
            logic.RefreshPlayerLife();
            LivesLabel.Text = "";
            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";
            
            field.HideBall();
            field.SetStartBallPosition();
            PauseMessage.Hide();
            GoBallLabel.Show();
            Invalidate();

        }
        private void MediumCheck(object sender, EventArgs e)
        {
            EasyCheckedLabel.Hide();
            EasyUncheckedLabel.Show();
            MediumCheckedLabel.Show();
            MediumUncheckedLabel.Hide();
            HardCheckedLabel.Hide();
            HardUncheckedLabel.Show();
            selectedLevel = DifficultyLevel.Medium;
            field.GenerateBlocks(selectedLevel);
            paused = false;
            ballMoves = false;
            blinking = true;
            StartPauseButton.BackgroundImage = CroppedPauseButton;

            ScoreNumberLabel.Text = "0";
            logic.RefreshPlayerScore();
            logic.RefreshPlayerLife();
            LivesLabel.Text = "";
            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";
                      
            field.HideBall();
            field.SetStartBallPosition();
            PauseMessage.Hide();
            GoBallLabel.Show();
            Invalidate();
        }
        private void HardCheck(object sender, EventArgs e)
        {           
            EasyCheckedLabel.Hide();
            EasyUncheckedLabel.Show();
            MediumCheckedLabel.Hide();
            MediumUncheckedLabel.Show();
            HardCheckedLabel.Show();
            HardUncheckedLabel.Hide();
            selectedLevel = DifficultyLevel.Hard;
            field.GenerateBlocks(selectedLevel);
            paused = false;
            ballMoves = false;
            blinking = true;
            StartPauseButton.BackgroundImage = CroppedPauseButton;

            ScoreNumberLabel.Text = "0";
            logic.RefreshPlayerScore();
            logic.RefreshPlayerLife();
            LivesLabel.Text = "";
            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            field.HideBall();
            field.SetStartBallPosition();
            PauseMessage.Hide();
            GoBallLabel.Show();
            Invalidate();
        }    
        public void StartPauseButtonClick(object sender, EventArgs e)
        {
            if (((Label)sender).BackgroundImage == CroppedStartButton1 ) 
            {
                paused = false;
                Game_over.Hide();
                PauseMessage.Hide();
                GoBallLabel.Show();
                ((Label)sender).BackgroundImage = CroppedPauseButton;
                // До момента нажатия моргает
                blinking = true;

                // задаем расположение мячика
                field.SetStartBallPosition();
                Invalidate();

                if (logic.GetPlayerLives() <= 0)
                {
                    Game_over.Hide();
                    field.HideBall();
                    field.SetStartBallPosition();
                    Invalidate();
                }
                 switch (selectedLevel)
                {
                    case DifficultyLevel.Easy:
                        MediumCheckedLabel.Hide();
                        MediumUncheckedLabel.Hide();
                        MediumLevelText.Hide();
                        HardCheckedLabel.Hide();
                        HardUncheckedLabel.Hide();
                        HardLevelText.Hide();
                        break;
                    case DifficultyLevel.Medium:
                        EasyCheckedLabel.Hide();
                        EasyUncheckedLabel.Hide();
                        EasyLevelText.Hide();
                        HardCheckedLabel.Hide();
                        HardUncheckedLabel.Hide();
                        HardLevelText.Hide();
                        break;
                    case DifficultyLevel.Hard:
                        MediumCheckedLabel.Hide();
                        MediumUncheckedLabel.Hide();
                        MediumLevelText.Hide();
                        EasyCheckedLabel.Hide();
                        EasyUncheckedLabel.Hide();
                        EasyLevelText.Hide();
                        break;
                }
            }
            else 
            {
                ((Label)sender).BackgroundImage = CroppedStartButton1;
                paused = true;
                Game_over.Hide();
                timer1.Stop();
                PauseMessage.Show();
                GoBallLabel.Hide();
                blinking = false;

                switch (selectedLevel)
                {
                    case DifficultyLevel.Easy:
                        MediumCheckedLabel.Hide();
                        MediumUncheckedLabel.Show();
                        MediumLevelText.Show();
                        HardCheckedLabel.Hide();
                        HardUncheckedLabel.Show();
                        HardLevelText.Show();
                        break;
                    case DifficultyLevel.Medium:
                        EasyCheckedLabel.Hide();
                        EasyUncheckedLabel.Show();
                        EasyLevelText.Show();
                        HardCheckedLabel.Hide();
                        HardUncheckedLabel.Show();
                        HardLevelText.Show();
                        break;
                    case DifficultyLevel.Hard:
                        MediumCheckedLabel.Hide();
                        MediumUncheckedLabel.Show();
                        MediumLevelText.Show();
                        EasyCheckedLabel.Hide();
                        EasyUncheckedLabel.Show();
                        EasyLevelText.Show();
                        break;
                }
            }
        }
        public void RestartButtonClick(object sender, EventArgs e)
        {   
            paused = false;
            Application.Restart();
        }
        public void InformButtonClick(object sender, EventArgs e)
        {
            // Создание формы
            Form InformForm = new Form
            {
                Text = "Об игре",
                Width = 700,
                Height = 550,
                AutoSize = true // Автоматическое изменение размера формы в зависимости от содержимого
            };

            GroupBox name = new GroupBox
            {
                Text = "Арканоид",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(InformForm.Width - 20, 90)
            };
            InformForm.Controls.Add(name);

            Label gamename = new Label
            {
                Text = "Классическая аркадная игра, целью которой является разрушение всех блоков на экране с помощью мяча, отскакивающего от платформы, управляемой игроком.",
                Location = new System.Drawing.Point(10, 20),
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Size = new System.Drawing.Size(name.Width - 20, 90)
            };
            name.Controls.Add(gamename);

            GroupBox logic = new GroupBox
            {
                Text = "Логика",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Location = new System.Drawing.Point(10, name.Bottom + 10),
                Size = new System.Drawing.Size(InformForm.Width - 20, 140)
            };
            InformForm.Controls.Add(logic);

            Label gameLogic = new Label
            {
                Text = "В игре Арканоид игрок управляет платформой, которая движется горизонтально вдоль нижней части экрана. Целью игры является отбивание мяча, чтобы разрушить все блоки на экране. Мяч отскакивает от платформы и стен, а при попадании на блок разрушает его. Игрок должен предотвращать падение мяча за нижнюю границу экрана, в противном случае он потеряет одну из своих жизней.",
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(logic.Width - 20, 140),
                Font = new Font("Times New Roman", 11, FontStyle.Regular)
            };
            logic.Controls.Add(gameLogic);

            GroupBox aboutGame = new GroupBox
            {
                Text = "Управление",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Location = new System.Drawing.Point(10, logic.Bottom + 10),
                Size = new System.Drawing.Size(InformForm.Width - 20, 60)
            };
            InformForm.Controls.Add(aboutGame);

            Label work = new Label
            {
                Text = "Для управления платформой используются стрелки влево-вправо на клавиатуре.",
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(aboutGame.Width - 20, 60),
                Font = new Font("Times New Roman", 11, FontStyle.Regular)
            };
            aboutGame.Controls.Add(work);

            GroupBox Author = new GroupBox
            {
                Text = "Автор",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Location = new System.Drawing.Point(10, aboutGame.Bottom + 10),
                Size = new System.Drawing.Size(InformForm.Width - 20, 40)
            };
            InformForm.Controls.Add(Author);

            Label AboutAuthor = new Label
            {
                Text = "студентка гр. 2-80 Данченко Е.А.",
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(Author.Width - 20, 40),
                Font = new Font("Times New Roman", 11, FontStyle.Regular)
            };
            Author.Controls.Add(AboutAuthor);

            GroupBox Year = new GroupBox
            {
                Text = "Год создания",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Location = new System.Drawing.Point(10, Author.Bottom + 10),
                Size = new System.Drawing.Size(InformForm.Width - 20, 40)
            };
            InformForm.Controls.Add(Year);

            Label AboutYear = new Label
            {
                Text = "2024",
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(Year.Width - 20, 40),
                Font = new Font("Times New Roman", 11, FontStyle.Regular)
            };
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
            if (!paused) 
            {              
                // очистим предыдущее место размещение платформы
                field.HidePlatform();

                if (!ballMoves)
                {
                    // место размещения мяча на карте
                    field.HideBall();
                }

                // какая клавиша нажата
                switch (e.KeyCode)
                {   // если нажали кнопку вправо
                    case Keys.Right:
                        field.PlatformMoveRight();
                        break;
                    // если нажали кнопку влево
                    case Keys.Left:
                        field.PlatformMoveLeft();
                        break;
                    //если нажали пробел
                    case Keys.Space:
                        if (!ballMoves)
                        {
                            GoBallLabel.Hide();
                            Continue();
                            blinking = false;
                            ballMoves = true;
                        }
                        break;
                }

                field.UpdatePlatformCoordinates();// Обновляет местоположение платформы на карте

                if (!ballMoves)
                {   
                    field.SetStartBallPosition();
                }
                Invalidate();
            }
        }
        // обработка коллизий мяча и обновление карты 
        public void Update(object sender, EventArgs e)
        {
            HandleBottomCollision(); // Обрабатывает столкновение с нижней границей карты
            field.HideBall(); // Очищает предыдущее положение мяча
            logic.HandleBallCollision(); // Обработка коллизий мяча
            ScoreLabel.Text = "SCORES"; // обновим счетчик
            ScoreNumberLabel.Text = logic.GetPlayerScore().ToString();
            field.PlaceBall();// Обновление координат мяча
            field.UpdatePlatformCoordinates();// Обновляет местоположение платформы на карте
            Invalidate();
        }
        private void HandleBottomCollision()
        {
            // столкновение с нижней частью границы ---> Игра начинается заново, если мяч коснулся нижней границы

            if (logic.NeedHandleBottomCollision())
            {
                logic.DamagePlayer();
                ballMoves = false;
                timer1.Stop();

                if (logic.GetPlayerLives() <= 0)
                {
                    paused = true;
                    FinalPlayersScore = logic.GetPlayerScore();

                    ScoreNumberLabel.Text = "0";
                    logic.RefreshPlayerScore();
                    LivesLabel.Text = "×_×";

                    StartPauseButton.BackgroundImage = CroppedStartButton1;
                    field = new GameField();
                    Game_over.Show();

                    // форма для имени игрока

                    Form UserForm = new Form
                    {
                        Text = "Сохранение результата",
                        Width = 350,
                        Height = 200,
                        AutoSize = true
                    };


                    TextBox Usname = new TextBox
                    {
                        Text = "Имя",
                        Font = new Font("Times New Roman", 12, FontStyle.Bold),
                        Location = new System.Drawing.Point(20, 70),
                        Size = new System.Drawing.Size(150, 30)
                    };
                    UserForm.Controls.Add(Usname);

                    GroupBox name = new GroupBox
                    {
                        Text = "Введите Ваше Имя:",
                        Font = new Font("Times New Roman", 12, FontStyle.Bold),
                        Location = new System.Drawing.Point(10, 50),
                        Size = new System.Drawing.Size(200, 30)
                    };
                    UserForm.Controls.Add(name);

                    // Создаем кнопку "Сохранить"
                    Button saveButton = new Button
                    {
                        Text = "Сохранить",
                        Location = new System.Drawing.Point(200, 200)
                    };
                    UserForm.Controls.Add(saveButton);
                   
                    UserForm.Show();
                    saveButton.Click += (object sender, EventArgs e) => SaveButton_Click(sender, e, UserForm, Usname);

                    StartPauseButton.Hide();
                    RestartButton.Location = new Point((field.Width) * 20 + 40, 125);
                    ResultButton.Location = new Point((field.Width) * 20 + 5, 300);
                    InformButton.Location = new Point((field.Width) * 20 + 95, 300);
                    ExitButton.Location = new Point((field.Width) * 20 + 45, 400);
                }
                else
                {
                    LivesLabel.Text = "";
                    for (int i = 0; i < logic.GetPlayerLives(); i++)
                        LivesLabel.Text += "♥️";

                    // место размещения мяча на карте
                    field.HideBall();

                    // задаем расположение мячика
                    field.SetStartBallPosition();
                    GoBallLabel.Show();
                    blinking = true;
                    Timer_Tick(null, null);
                    Invalidate();
                }
            }
        }
        private void SaveButton_Click(object sender, EventArgs e, Form UserForm, TextBox Usname)
        {
            UserName = Usname.Text;
            AddResults(FinalPlayersScore, UserName);
            UserForm.Close();
        }

        public void AddResults(int FinalPlayersScore, string FinalPlayersName)
        {
           
            numbersFromFile[numbersFromFile.Length - 1] = FinalPlayersScore;
            namesFromFile[namesFromFile.Length - 1] = FinalPlayersName;

            // Добавление полученных очков в массив после сравнения (сортировка вставкой)

            for (int i = 1; i < numbersFromFile.Length; i++) // Начинаем с индекса 1 (так как элемент с индексом 0 считается уже отсортированным). Проходим по массиву слева направо.
            {
                int key = numbersFromFile[i]; // Текущий элемент, который нужно вставить в правильное место
                string keyName = namesFromFile[i];

                int j = i - 1;// Индекс элемента, перед которым нужно вставить текущий элемент

                // Сравнение и сдвиг элементов влево, пока текущий элемент больше предыдущего
                while (j >= 0 && numbersFromFile[j] < key)
                {
                    numbersFromFile[j + 1] = numbersFromFile[j]; //  Обмен местами текущего элемента и предыдущего элемента.
                    namesFromFile[j + 1] = namesFromFile[j];
                    j--;
                }

                numbersFromFile[j + 1] = key; // Вставка текущего элемента на его правильное место после того, как он был сдвинут влево.
                namesFromFile[j + 1] = keyName;
            }

            // Запись массива обратно в файл
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                for (int i = 0; i < numbersFromFile.Length - 1; i++)
                {
                    if (namesFromFile[i] == null || namesFromFile[i].Length == 0) namesFromFile[i] = "unknown";
                    sw.WriteLine(namesFromFile[i] + " " + numbersFromFile[i]);
                }
            }        
        }
        private void ResultButtonClick(object sender, EventArgs e)
        {
            // Создание формы
            Form ResultsForm = new Form
            {
                Text = "Лучшие результаты"
            };

            Label[] labels = new Label[10];

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label
                {
                    Location = new Point(ResultsForm.Width / 2, 25 * i)
                };
                ResultsForm.Controls.Add(labels[i]);
            }
            bool resultWas = false;
            for (int i = 0; i < numbersFromFile.Length - 1; i++)
            {
                if (numbersFromFile[i] != 0)
                {
                    labels[i].Text = namesFromFile[i] + " " + numbersFromFile[i].ToString();
                    resultWas = true;
                }

            }
            if (!resultWas) labels[0].Text = "0";
            // Вывод формы
            ResultsForm.Show();
        }

        
        // продолжаем игру (в случае потери жизни), не изменяя состояние карты
        public void Continue()
        {
            timer1.Interval = 50;// нужен для мячика
            ScoreNumberLabel.Text = logic.GetPlayerScore().ToString();
            LivesLabel.Text = "";
            for (int i = 0; i < logic.GetPlayerLives(); i++)
                LivesLabel.Text += "♥";

            // место размещения платформы на карте

            field.UpdatePlatformCoordinates();
            field.HideBall();

            // реализация движения мячика
            field.ResetBallDirectory();

            // запуск таймера для осуществления цикла игры
            timer1.Start();
        }
        // отрисовка элементов формы
        public void OnPaint(object sender, PaintEventArgs e)
        {
            field.DrawArea(e.Graphics);// рисуем границы игрового поля
            field.DrawMap(e.Graphics);// рисуем карту с игровыми компонентами    
        }    
    }   
}
