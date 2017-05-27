using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace MyGame
{
    static class Game
    {
        static BufferedGraphicsContext context;
        static public BufferedGraphics buffer;
        static public int Width { get; set; }
        static public int Height { get; set; }

        static int score = 0;
        static int asteroidsCount = 10;
        static int asteroidsBigCount = 3;
        static int nebulaCount = 1;
        static int waveCount = 1;

        static Image background;

        public static Random rnd = new Random();
        static Timer timer = new Timer();

        static BaseObject[] objs;
        static List<Asteroids> asteroids = new List<Asteroids>();
        static List<AsteroidsBig> asteroidsBig = new List<AsteroidsBig>();
        static List<Nebula> nebula = new List<Nebula>();
        static List<Bullet> bullet = new List<Bullet>();
        static List<Boss> boss = new List<Boss>();

        static Ship ship;
        static Medkit medkit;

        static Game()
        {
        }

        static public void Init(Form form)
        {
            background = Image.FromFile("background.png");
            Graphics g;

            context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Width = form.Width;
            Height = form.Height;

            buffer = context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();

            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
        }

        static private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) bullet.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + 2), new Point(20, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) ship.Up();
            if (e.KeyCode == Keys.Down) ship.Down();
            if (e.KeyCode == Keys.Left) ship.Left();
            if (e.KeyCode == Keys.Right) ship.Right();
            if (e.KeyCode == Keys.Escape) Application.Exit();
        }


        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        static public void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            buffer.Graphics.DrawImage(background, 0, 0, Width, Height);

            foreach (BaseObject obj in objs)
                obj.Draw();

            foreach (Asteroids ast in asteroids)
                if (ast != null) ast.Draw();

            foreach (AsteroidsBig astBig in asteroidsBig)
                if (astBig != null) astBig.Draw();

            foreach (Nebula neb in nebula)
                if (neb != null) neb.Draw();
            
            foreach (Bullet b in bullet)
                if (b != null) b.Draw();

            foreach (Boss bss in boss)
                if (bss != null) bss.Draw();

            ship.Draw();
            medkit.Draw();

            buffer.Graphics.DrawString("Health: " + ship.Health, SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
            buffer.Graphics.DrawString("Score: " + score, SystemFonts.DefaultFont, Brushes.White, new Point(80, 10));
            buffer.Graphics.DrawString("Wave: " + waveCount, SystemFonts.DefaultFont, Brushes.White, new Point(150, 10));
            buffer.Render();
        }

        static public void Update()
        {
            foreach (BaseObject obj in objs)
                obj.Update();
                        
            foreach (Bullet b in bullet)
                if (b != null) b.Update();

            for (int i = 0; i < asteroids.Count; i++)
            {                
                if (asteroids[i] != null)
                {
                    asteroids[i].Update();
                    for (int j = 0; j < bullet.Count; j++)
                        if (asteroids[i] != null && bullet[j].Collision(asteroids[i]))
                        {
                            score += 1;
                            bullet.RemoveAt(j);
                            buffer.Graphics.DrawString("Score: " + score, SystemFonts.DefaultFont, Brushes.White, new Point(80, 10));
                            Console.WriteLine("Little asteriod killed. Score: {0}", score);
                            j--;                            
                            asteroids[i] = null;
                            continue;
                        }

                    if (asteroids[i] != null && ship.Collision(asteroids[i]))
                    {                        
                        asteroids[i].Reset();
                        ship.Reset();
                        ship.Health -= 10;
                        Console.WriteLine("Collision with little asteroid. Health: {0}", ship.Health);
                        if (ship.Health <= 0)
                        {
                            ship.Die();
                            Console.WriteLine("Game Over!!");
                        }
                    }
                }
                else
                {
                    asteroids.RemoveAt(i);
                    i--;
                    Console.WriteLine("Small asteroids left: {0}", asteroids.Count);
                }
            }

            for (int i = 0; i < asteroidsBig.Count; i++)
            {
                if (asteroidsBig[i] != null)
                {
                    asteroidsBig[i].Update();
                    for (int j = 0; j < bullet.Count; j++)
                        if (asteroidsBig[i] != null && bullet[j].Collision(asteroidsBig[i]))
                        {
                            asteroidsBig[i].Power -= bullet[j].Damage;
                            bullet.RemoveAt(j);
                            if (asteroidsBig[i].Power == 0)
                            {
                                asteroidsBig[i] = null;
                                score += 3;
                                Console.WriteLine("Big asteriod killed. Score: {0}", score);
                            }
                            buffer.Graphics.DrawString("Score: " + score, SystemFonts.DefaultFont, Brushes.White, new Point(80, 10));
                            j--;
                            continue;
                        }
                    if (asteroidsBig[i] != null && ship.Collision(asteroidsBig[i]))
                    {
                        asteroidsBig[i].Reset();
                        ship.Reset();
                        ship.Health -= 30;
                        Console.WriteLine("Collision with Big asteroid. Health: {0}", ship.Health);
                        if (ship.Health <= 0)
                        {
                            ship.Die();
                            Console.WriteLine("Game Over!!");
                        }
                    }
                }
                else
                {
                    asteroidsBig.RemoveAt(i);
                    i--;
                    Console.WriteLine("Big asteroids left: {0}", asteroidsBig.Count);
                }
            }

            for (int i = 0; i < nebula.Count; i++)
            {
                if (nebula[i] != null)
                {
                    nebula[i].Update();
                    for (int j = 0; j < bullet.Count; j++)
                        if (nebula[i] != null && bullet[j].Collision(nebula[i]))
                        {
                            nebula[i].Power -= bullet[j].Damage;
                            bullet.RemoveAt(j);
                            if (nebula[i].Power == 0)
                            {
                                nebula[i] = null;
                                score += 5;
                                Console.WriteLine("Big asteriod killed. Score: {0}", score);
                            }
                            buffer.Graphics.DrawString("Score: " + score, SystemFonts.DefaultFont, Brushes.White, new Point(80, 10));
                            j--;
                            continue;
                        }
                    if (nebula[i] != null && ship.Collision(nebula[i]))
                    {
                        nebula[i].Reset();
                        ship.Reset();
                        ship.Health -= 50;
                        Console.WriteLine("Collision with Big asteroid. Health: {0}", ship.Health);
                        if (ship.Health <= 0)
                        {
                            ship.Die();
                            Console.WriteLine("Game Over!!");
                        }
                    }
                }
                else
                {
                    nebula.RemoveAt(i);
                    i--;
                    Console.WriteLine("Nebula left: {0}", nebula.Count);
                }
            }

            for (int i = 0; i < boss.Count; i++)
            {
                if (boss[i] != null)
                {
                    boss[i].Update();
                    for (int j = 0; j < bullet.Count; j++)
                        if (boss[i] != null && bullet[j].Collision(boss[i]))
                        {
                            boss[i].Power -= bullet[j].Damage;
                            bullet.RemoveAt(j);
                            if (boss[i].Power == 0)
                            {
                                boss[i] = null;
                                score += 20;
                                Console.WriteLine("Boss killed. Score: {0}", score);
                                waveCount = 0;
                            }
                            buffer.Graphics.DrawString("Score: " + score, SystemFonts.DefaultFont, Brushes.White, new Point(80, 10));
                            j--;
                            continue;
                        }
                    if (boss[i] != null && ship.Collision(boss[i]))
                    {
                        ship.Reset();
                        boss[i].Reset();
                        ship.Health -= 70;
                        buffer.Graphics.DrawString("Health: " + ship.Health, SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
                        Console.WriteLine("Collision with Boss. Health: {0}", ship.Health);
                        if (ship.Health <= 0)
                        {
                            ship.Die();
                            Console.WriteLine("Game Over!!");
                        }
                    }
                }
                else
                {
                    boss.RemoveAt(i);
                    i--;
                    Console.WriteLine("Boss left: {0}", boss.Count);
                }
            }

            if (ship.Collision(medkit))
            {
                ship.Health += medkit.Restore;
                buffer.Graphics.DrawString("Health: " + ship.Health, SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
                Console.WriteLine("Collected MedKit. Health: {0}", ship.Health);
                medkit.Reset();
            }
            
            medkit.Update();
            ship.Update();

            if (asteroids.Count == 0 && asteroidsBig.Count == 0 && nebula.Count == 0 && boss.Count == 0)
            {
                waveCount++;
                if (waveCount == 3) BossCreate();
                if (waveCount < 3) Reload(asteroidsCount, asteroidsBigCount, nebulaCount, waveCount);
            }
        }        

        static public void BossCreate()
        {
            boss.Add(new Boss(new Point(Width - 250, Height / 2), new Point(0, 5), new Size(150, 150)));
        }

        static public void Reload (int astCount, int astBigCount, int nebCount, int wvCount)
        {
            for (int i = 0; i < astCount + wvCount; i++)
                asteroids.Add(new Asteroids(new Point(Width, rnd.Next(40, Height - 30)), new Point(6 - i, 0), new Size(50, 50)));

            for (int i = 0; i < astBigCount + wvCount; i++)
                asteroidsBig.Add(new AsteroidsBig(new Point(Width, rnd.Next(40, Height - 30)), new Point(6 - i, 5), new Size(50, 50)));

            for (int i = 0; i < nebCount + wvCount; i++)
                nebula.Add(new Nebula(new Point(Width, rnd.Next(40, Height - 30)), new Point(3 - i, 1), new Size(70, 70)));
        }

        static public void Load()
        {
            ship = new Ship(new Point(20, Height / 2), new Point (0,0), new Size(20, 20));
            medkit = new Medkit(new Point (Width - 50, rnd.Next(1, Height)), new Point(-5, 0), new Size(20, 20));
            //boss.Add(new Boss(new Point(Width - 250, Height / 2), new Point(0, 5), new Size(150, 150)));

            objs = new BaseObject[50];
            for (int i = 0; i < objs.Length; i++)                
                objs[i] = new Star(new Point(rnd.Next(1, Width), rnd.Next(1,Height)), new Point(15 - i, 0), new Size(20, 20));

            for (int i = 0; i < asteroidsCount; i++)
                asteroids.Add(new Asteroids(new Point(Width, rnd.Next(40, Height - 30)), new Point(6 - i, 0), new Size(50, 50)));

            for (int i = 0; i < asteroidsBigCount; i++)
                asteroidsBig.Add(new AsteroidsBig(new Point(Width, rnd.Next(40, Height - 30)), new Point(6 - i, 5), new Size(50, 50)));

            for (int i = 0; i < nebulaCount; i++)
                nebula.Add(new Nebula(new Point(Width, rnd.Next(40, Height - 30)), new Point(3 - i, 3-i), new Size(70, 70)));
        }

        static public void Finish()
        {
            timer.Stop();
            buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            buffer.Render();
        }
    }
}