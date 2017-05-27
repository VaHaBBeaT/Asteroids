using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Asteroids : BaseObject
    {
        Image image1;
        public int Power { get; set; }

        public Asteroids(Point pos, Point dir, Size size):base(pos,dir,size)
        {
            image1 = Image.FromFile("asteroid2_1.png");
            Power = 1;
        }

        public override void Draw()
        {            
            Game.buffer.Graphics.DrawImage(image1, pos);
            Game.buffer.Graphics.FillEllipse(Brushes.Lime, new Rectangle(pos.X+12, pos.Y-5, 5, 5));
        }

        public override void Update()
        {
            pos.X += dir.X;
            if (pos.X < 0)
            {
                pos.X = Game.Width;
                pos.Y = Game.rnd.Next(5, Game.Height - 5);
            }
            if (pos.X > Game.Width) dir.X = -dir.X;
        }

        public void Reset()
        {
            pos.X = Game.Width;
            pos.Y = Game.rnd.Next(5, Game.Height - 5);
        }
    }
}
