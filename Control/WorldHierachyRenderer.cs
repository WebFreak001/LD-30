using FarseerPhysics.Dynamics;
using LudumDare.Physics;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.Control
{
    public class WorldHierachyRenderer : UIComponent
    {
        public AnchorPoints Anchor
        {
            get;
            set;
        }

        public SFML.Graphics.Color BackgroundColor
        {
            get;
            set;
        }

        public bool IsFocusable
        {
            get { return false; }
        }

        public bool IsFocused
        {
            get;
            set;
        }

        public Vector2f MaxSize
        {
            get;
            set;
        }

        public Vector2f MinSize
        {
            get;
            set;
        }

        public Vector2f Position
        {
            get;
            set;
        }

        public Vector2f Size
        {
            get;
            set;
        }

        public FastText label;
        public RectangleShape bg;
        public PhysicsWorld world;
        public List<PhysicsInfoItem> items;

        public RectangleShape TreeViewRect;

        public Texture LineI;
        public Texture LineIL;
        public Texture LineL;
        public Texture LinePlus;
        public Texture EyeTexture;
        private int LockI = -1;

        public WorldHierachyRenderer(PhysicsWorld world)
        {
            label = new FastText(new Font("Content/font.ttf"), 22)
            {
                Text = "World",
                TextAlignment = Alignment.MiddleCenter,
                Color = Color.Black
            };

            items = new List<PhysicsInfoItem>();
            bg = new RectangleShape();

            this.world = world;
            world.OnBodyAdded += OnBodyAdded;
            world.OnClear += world_OnClear;

            TreeViewRect = new RectangleShape(new Vector2f(40, 40));

            LineI = new Texture("Content/LineI.png");
            LineL = new Texture("Content/LineL.png");
            LineIL = new Texture("Content/LineIL.png");
            LinePlus = new Texture("Content/LinePlus.png");
            EyeTexture = new Texture("Content/IconEye.png");

            LineI.Smooth = true;
            LineL.Smooth = true;
            LinePlus.Smooth = true;

            label.Size = new Vector2f(300, 30);
        }

        private void world_OnClear(object sender, EventArgs e)
        {
            items.Clear();
        }

        public void OnBodyAdded(object sender, Body e)
        {
            if (e.BodyType != BodyType.Static)
            {
                var item = new PhysicsInfoItem(e);
                item.Position = new Vector2f(0, 30 + items.Count * 30);
                items.Add(item);
            }
        }

        public void HandleKeyDown(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
        }

        public void HandleKeyUp(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
        }

        public void HandleMouseDown(Vector2i mousePosition, Mouse.Button button)
        {
            mousePosition += new Vector2i(-(int)Position.X, -(int)Position.Y);
            if (mousePosition.Y > 30 && mousePosition.Y < 30 + 30 * items.Count && mousePosition.X < 300)
            {
                int i = (mousePosition.Y - 30) / 30;
                world.CamLock = items[i].Body;
                LockI = i;
            }
        }

        public void HandleMouseMove(Vector2i mousePosition)
        {
        }

        public void HandleMouseUp(Vector2i mousePosition, Mouse.Button button)
        {
            world.CamLock = null;
            LockI = -1;
        }

        public void Render(RenderTarget target, Vector2f position)
        {
            bg.Size = Size;
            bg.FillColor = BackgroundColor;
            bg.Position = position;
            target.Draw(bg);

            label.Render(target, position);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Size = new Vector2f(Size.X, 30);
                items[i].Render(target, items[i].Position + position + new Vector2f(60, 0));

                if (i == items.Count - 1)
                {
                    TreeViewRect.Texture = LineL;
                }
                else if (i == 0)
                {
                    TreeViewRect.Texture = LineIL;
                }
                else
                {
                    TreeViewRect.Texture = LinePlus;
                }
                TreeViewRect.Size = new Vector2f(40, 40);
                TreeViewRect.Position = new Vector2f(-10, 27 + i * 30) + position;
                target.Draw(TreeViewRect);
                if (LockI != i)
                {
                    TreeViewRect.Size = new Vector2f(24, 24);
                    TreeViewRect.Texture = EyeTexture;
                    TreeViewRect.Position = new Vector2f(34, 36 + i * 30) + position;
                    target.Draw(TreeViewRect);
                }
            }
        }
    }
}