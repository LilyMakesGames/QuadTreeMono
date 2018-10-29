using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuadTreeMono
{
    public class BoundingVolume
    {
        public Rectangle rect;
        public Point speed;
        public bool printed = false;
        public Node parent;

        public BoundingVolume(Point position, Point speed,Node parent)
        {
            rect = new Rectangle(position, new Point(10, 10));
            this.speed = speed;
            this.parent = parent;
        }

        public void Update()
        {
            rect.X += speed.X;
            rect.Y += speed.Y;
            if (rect.X + speed.X < 0 || rect.X + speed.X + rect.Width > Game1.windowWidth)
                speed.X *= -1;
            if (rect.Y + speed.Y  < 0 || rect.Y + speed.Y + rect.Height > Game1.windowHeight)
                speed.Y *= -1;
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(Game1.pixel, rect, color);
        }
    }
}
