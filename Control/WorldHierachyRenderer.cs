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

        public TextControl label;
        public RectangleShape bg;
        public PhysicsWorld world;
        public List<PhysicsInfoItem> items;

        public WorldHierachyRenderer(PhysicsWorld world)
        {
            label = new TextControl(new Font("Content/font.ttf"), 22);
            label.Text = "World";
            label.TextAlignment = Alignment.MiddleCenter;
            label.Color = Color.Black;

            items = new List<PhysicsInfoItem>();
            bg = new RectangleShape();

            this.world = world;
        }

        public void HandleKeyDown(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
        }

        public void HandleKeyUp(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
        }

        public void HandleMouseDown(Vector2i mousePosition, Mouse.Button button)
        {
        }

        public void HandleMouseMove(Vector2i mousePosition)
        {
        }

        public void HandleMouseUp(Vector2i mousePosition, Mouse.Button button)
        {
        }

        public void Render(RenderTarget target, Vector2f position)
        {
            bg.Size = Size;
            bg.FillColor = BackgroundColor;
            bg.Position = position;
            target.Draw(bg);

            label.Size = new Vector2f(Size.X, 30);
            label.Render(target, position);
        }
    }
}