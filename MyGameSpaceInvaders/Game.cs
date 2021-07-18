using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MyGameSpaceInvaders
{
    public partial class Game : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        Gameplay gameplay;
        Timer timer = new Timer();

        public Game(DirectoryInfo imagesDirectory = null)
        {
            DoubleBuffered = true;
            InitializeComponent();
            gameplay = new Gameplay(500, 500);
            BackColor = Color.Black;
            Size = new Size(540, 550);
            MaximumSize = new Size(540, 550);
            MinimumSize = new Size(540, 550);
            Text = "Space Invaders!";
            StartPosition = FormStartPosition.CenterScreen;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            var time = 0;
            timer = new Timer()
            { Interval = 50 };
            timer.Tick += (sender, args) =>
            {
                time++;
                gameplay.ChangeWorldOnTick();
                if (gameplay.isFinished)
                    timer.Stop();
                if (gameplay.isWin)
                    timer.Stop();
                gameplay.CheckIsWin();
                Invalidate();
            };

            Paint += (sender, args) =>
            {
                if (!gameplay.isStarted && !gameplay.isFinished)
                    args.Graphics.DrawImage(bitmaps["start_pic.png"], 210, 3);
                if (gameplay.isFinished)
                    args.Graphics.DrawImage(bitmaps["you_lose_pic.png"], gameplay.fieldWight / 2 - 180, gameplay.fieldHeight / 2 - 60);
                if (gameplay.isStarted)
                {
                    args.Graphics.DrawImage(bitmaps["lives_pic.png"], 440, 3);
                    args.Graphics.DrawImage(bitmaps[$@"{gameplay.player.Health}.png"], 480, 3);
                }
                if (gameplay.isWin)
                    args.Graphics.DrawImage(bitmaps["win_pic.png"], gameplay.fieldWight / 2 - 180, gameplay.fieldHeight / 2 - 60);
                args.Graphics.DrawImage(bitmaps["score_pic.png"], 3, 3);
                var x = 120;
                foreach (var i in gameplay.FindScoreOrder())
                {
                    args.Graphics.DrawImage(bitmaps[$@"{i}.png"], x, 3);
                    x += 18;
                }
                if (gameplay.specAlien.Alive)
                    args.Graphics.DrawImage(bitmaps["spec alien.png"], gameplay.specAlien.X, gameplay.specAlien.Y);
                if (!gameplay.isFinished)
                    foreach (var a in gameplay.aliens)
                       if (a.Alive)  
                            args.Graphics.DrawImage(bitmaps["alien.png"], a.X, a.Y);
                foreach (var w in gameplay.walls)
                    if (w.health > 0)
                        args.Graphics.DrawImage(bitmaps[$@"wall{w.health}.png"], w.X, w.Y);
                args.Graphics.DrawImage(bitmaps[$@"Gun{gameplay.player.Health}.png"], gameplay.player.X, gameplay.player.Y);
                args.Graphics.DrawImage(bitmaps["bullet.png"], gameplay.pbullet.X, gameplay.pbullet.Y);
                args.Graphics.DrawImage(bitmaps["bullet.png"], gameplay.abullet.X, gameplay.abullet.Y);

            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        if (gameplay.isStarted)
                            gameplay.TryMove(-1);
                        break;
                    }
                case Keys.Right:
                    {
                        if (gameplay.isStarted)
                            gameplay.TryMove(1);
                        break;
                    }
                case Keys.Space:
                    {
                        if (gameplay.isStarted)
                            gameplay.shoot = true;
                        break;
                    }
                case Keys.Escape:
                    {
                        gameplay.isPaused = !gameplay.isPaused;
                        if (gameplay.isPaused)
                            timer.Stop();
                        else
                            timer.Start();
                        break;
                    }
                case Keys.S:
                    if (!gameplay.isFinished)
                        timer.Start();
                        gameplay.Start();
                    break;
                case Keys.R:
                    if (gameplay.isFinished)
                        gameplay.Restart();
                    break;
                case Keys.N:
                    if (gameplay.isWin)
                    {
                        timer.Interval -= 5;
                        gameplay.NextLvl();
                        timer.Start();
                        gameplay.Start();
                        gameplay.isWin = false;
                    }
                    break;
            }
        }
    }
}