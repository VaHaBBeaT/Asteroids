using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Nebula : BaseObject
    {
        Image image1;
        public int Power { get; set; }

        public Nebula(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("nebula1.png");
            Power = 5;
        }

        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(image1, pos);
            for (int i = 1; i <= Power; i++)
            {
                Game.buffer.Graphics.FillEllipse(Brushes.Lime, new Rectangle(pos.X + 40 + i * 6, pos.Y + 15, 5, 5));
            }
        }

        public override void Update()
        {
            pos.X += dir.X;
            pos.Y = pos.Y + (int)(Math.Sin(pos.X * Math.PI / 180) * 3);
            if (pos.X < 0) pos.X = Game.Width;
            if (pos.X > Game.Width) dir.X = -dir.X;
        }

        public void Reset()
        {
            pos.X = Game.Width;
            pos.Y = Game.rnd.Next(5, Game.Height - 5);
            Power = 5;
        }
    }
}
