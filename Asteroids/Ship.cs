using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Ship : BaseObject
    {
        Image image1;

        public int Health { get; set; }

        public static event Message MessageDie;

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("ship1.png");
            Health = 100;
        }

        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(image1, pos);
        }

        public override void Update()
        {
            pos.X += dir.X;
            pos.Y += dir.Y;
            Game.buffer.Graphics.DrawString("Health: " + Health, SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
        }

        public void Reset()
        {
            pos.X = 50;
            pos.Y = Game.Height / 2;
            Game.buffer.Graphics.DrawString("Health: " + Health, SystemFonts.DefaultFont, Brushes.White, new Point(10, 10));
        }
        public void Up()
        {
            if (pos.Y > 0) pos.Y = pos.Y - 20;
        }
        public void Down()
        {
            if (pos.Y < Game.Height - 20) pos.Y = pos.Y + 20;
        }
        public void Left()
        {
            if (pos.X > 0) pos.X = pos.X - 20;
        }
        public void Right()
        {
            if (pos.X < Game.Width - 50) pos.X = pos.X + 20;
        }
        public void Die()
        {
            if (MessageDie != null) MessageDie();
        }
    }
}
