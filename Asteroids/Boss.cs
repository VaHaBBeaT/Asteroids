using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Boss : BaseObject
    {
        Image image1;
        public int Power { get; set; }

        public Boss(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("boss1.png");
            Power = 30;
        }

        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(image1, pos);
            for (int i = 1; i <= Power; i++)
            {
                Game.buffer.Graphics.FillEllipse(Brushes.Lime, new Rectangle(pos.X + 20 + i * 6, pos.Y + 15 , 5, 5));
            }
        }

        public override void Update()
        {
            pos.Y = pos.Y + dir.Y;
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > Game.Height - 270) dir.Y = -dir.Y;
        }
        public void Reset()
        {
            pos.X = Game.Width - 250;
            pos.Y = Game.rnd.Next(5, Game.Height - 5);
            Power = 30;
        }
    }
}
