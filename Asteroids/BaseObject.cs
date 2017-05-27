using System;
using System.Drawing;


namespace MyGame
{
    abstract class BaseObject:ICollision
    {
        protected Point pos;
        protected Point dir;
        protected Size size;

        public delegate void Message();

        public BaseObject(Point pos, Point dir, Size size)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }

        public abstract void Draw();        

        public abstract void Update();

        public bool Collision(ICollision obj)
        {
            if (obj.Rect.IntersectsWith(this.Rect))
                return true;
            else return false;
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(pos, size);
            }
        }

    }
}
