using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Medkit : BaseObject
    {
        Image image1;
        public int Restore { get; set; }

        public Medkit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("medkit1.png");
            Restore = 20;
        }

        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(image1, pos);
        }

        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y;
            if (pos.X < 0) pos.X = Game.Width;
            if (pos.X > Game.Width) dir.X = -dir.X;
        }

        public void Reset()
        {
            pos.X = Game.Width;
            pos.Y = Game.rnd.Next(5, Game.Height - 5);
        }
    }
}
