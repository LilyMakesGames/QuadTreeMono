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
    public class Node
    {
        public List<BoundingVolume> points;
        public Rectangle rect;
        public Node[] children;
        public Node parent;

        bool divided = false;
        int pointLimit;
        Node nw, ne, sw, se;

        Color colorRect, colorPoints;
        int strokeSize = 1;


        public static Node selected;


        public Node(Rectangle rect, int pointLimit, Node parent)
        {
            points = new List<BoundingVolume>();
            this.rect = rect;
            this.pointLimit = pointLimit;
            this.parent = parent;
        }

        public void Add(BoundingVolume bv)
        {
            if (!rect.Contains(bv.rect.Center))
            {
                return;
            }

            if (points.Count < pointLimit && !divided)
            {
                points.Add(new BoundingVolume(bv.rect.Location,bv.speed,this));

            }
            else
            {
                if (!this.divided)
                    Subdivide();

                points.Add(new BoundingVolume(bv.rect.Location,bv.speed,this));

                foreach (BoundingVolume bound in points)
                {
                    nw.Add(bound);
                    ne.Add(bound);
                    sw.Add(bound);
                    se.Add(bound);
                }
                points.Clear();
            }

        }


        void Subdivide()
        {
            divided = true;

            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width / 2;
            int h = rect.Height / 2;

            nw = new Node(new Rectangle(x, y, w, h), pointLimit,this);
            ne = new Node(new Rectangle(x + w, y, w, h), pointLimit,this);
            sw = new Node(new Rectangle(x, y + h, w, h), pointLimit,this);
            se = new Node(new Rectangle(x + w, y + h, w, h), pointLimit,this);
            children = new Node[4]
            {
                nw,
                ne,
                sw,
                se
            };

        }

        public void CheckChildren(BoundingVolume bv)
        {

            if (rect.Contains(bv.rect.Center))
            {
                if (children != null)
                {
                    foreach (Node node in children)
                    {
                        node.CheckChildren(bv);
                    }
                }
                else
                {
                    selected = this;
                }

            }

        }

        public void Update()
        {
            if(children != null)
            {
                foreach (Node n in children)
                {
                    n.Update();
                }
            }
            else
            {
                foreach (BoundingVolume bv in points)
                {
                    bv.Update();
                }
            }

        }

        public void Draw(SpriteBatch sb)
        {

            if (selected == this)
            {
                colorPoints = Color.LightGreen;
                colorRect = Color.LightGreen;
                strokeSize = 3;

            }else if(selected != null)
            {
                colorPoints = Color.Gray;
                colorRect = Color.Gray;
                strokeSize = 1;
            }
            else
            {
                colorPoints = Color.Red;
                colorRect = Color.White;
                strokeSize = 1;
            }

            sb.Draw(Game1.pixel, new Rectangle(new Point(rect.Left, rect.Top), new Point(strokeSize, rect.Height)), colorRect);
            sb.Draw(Game1.pixel, new Rectangle(new Point(rect.Right - 1, rect.Top), new Point(strokeSize, rect.Height)), colorRect);
            sb.Draw(Game1.pixel, new Rectangle(new Point(rect.Left, rect.Top), new Point(rect.Width, strokeSize)), colorRect);
            sb.Draw(Game1.pixel, new Rectangle(new Point(rect.Left, rect.Bottom - 1), new Point(rect.Width, strokeSize)), colorRect);

            if (children != null)
            {
                nw.Draw(sb);
                ne.Draw(sb);
                sw.Draw(sb);
                se.Draw(sb);

            }
            else
            {
                foreach (BoundingVolume bv in points)
                {
                    sb.Draw(Game1.pixel, bv.rect, colorPoints);
                }

            }
        }

    }
}
