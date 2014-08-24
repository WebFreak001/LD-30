using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.Control
{
    public class RectControl : UIComponent
    {
        public bool IsFocusable { get { return false; } }

        public Vector2f MaxSize { get; set; }

        public Vector2f MinSize { get; set; }

        public Vector2f Size { get; set; }

        public Vector2f Position { get; set; }

        public bool IsFocused { get; set; }

        public uint TextSize { get; set; }

        public AnchorPoints Anchor { get; set; }

        public Alignment TextAlignment { get; set; }

        public FloatRect Padding { get; set; }

        public Color BackgroundColor { get; set; }

        public RectangleShape shape;

        public RectControl()
        {
            shape = new RectangleShape();
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

        public void HandleMouseUp(Vector2i mousePosition, Mouse.Button button)
        {
        }

        public void HandleMouseMove(Vector2i mousePosition)
        {
        }

        public void Render(RenderTarget target, Vector2f position)
        {
            shape.Position = position;
            shape.FillColor = BackgroundColor;
            shape.Size = Size;
            target.Draw(shape);
        }
    }
}