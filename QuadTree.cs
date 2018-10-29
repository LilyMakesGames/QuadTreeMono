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
    class QuadTree
    {
        public Node mainNode;
        List<BoundingVolume> bvList = new List<BoundingVolume>();
        bool pressedLeft,pressedRight;

        public static BoundingVolume checkBV;

        public void Adicionar(BoundingVolume bv)
        {
            bvList.Add(bv);
        }

        public void Consultar(BoundingVolume bv)
        {
            mainNode.CheckChildren(bv);
        }

        public void Build()
        {
            mainNode = new Node(new Rectangle(0, 0, Game1.windowWidth, Game1.windowHeight), 4, null);
            foreach (BoundingVolume bv in bvList)
            {
                mainNode.Add(bv);
                bv.Update();
            }
            mainNode.Update();
        }

        public void MouseInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !pressedLeft && mainNode.rect.Contains(Mouse.GetState().Position))
            {
                pressedLeft = true;
                BoundingVolume bv = new BoundingVolume(Mouse.GetState().Position, new Point(Game1.r.Next(-3, 3), Game1.r.Next(-3, 3)), mainNode);
                Adicionar(bv);
                Node.selected = null;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                pressedLeft = false;
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed && !pressedRight && mainNode.rect.Contains(Mouse.GetState().Position))
            {
                pressedRight = true;
                checkBV = new BoundingVolume(Mouse.GetState().Position, Point.Zero, null);
                Consultar(checkBV);
            }
            if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                pressedRight = false;
            }
        }

        public void KeyboardInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ClearCheck();
            }

        }

        public void ClearCheck()
        {
            checkBV = null;
            Node.selected = null;

        }

        public void Update()
        {
            MouseInput();
            KeyboardInput();
            Build();
            if (checkBV != null)
            {
                Consultar(checkBV);
            }

        }

        public void Draw(SpriteBatch sb)
        {
            if(checkBV != null)
            checkBV.Draw(sb,Color.Pink);
            mainNode.Draw(sb);
        }
    }
}
