using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Bullet : BaseObject
    {
        Image image1;
        public int Damage { get; set; }

        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image1 = Image.FromFile("bullet.png");
            Damage = 1;
        }

        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(image1, pos);
        }

        public override void Update()
        {
            pos.X = pos.X + dir.X;
        }

        public void Reset()
        {
            pos.X = -30;
            dir.X = 0;
            pos.Y = Game.Height / 2;
        }
    }
}
