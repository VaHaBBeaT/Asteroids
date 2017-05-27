using System;
using System.Drawing;
using System.IO;

namespace MyGame
{
    class Star : BaseObject
    {
        Image image1;

        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("star1.png");
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
        }
    }
}
