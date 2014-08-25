using FarseerPhysics.Dynamics;
using LudumDare.Json;
using LudumDare.Physics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare.Control
{
    public class PhysicsInfoItem : UIComponent
    {
        public AnchorPoints Anchor
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
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

        public bool IsFocusable
        {
            get { return false; }
        }

        private string type;
        private FastText text;

        public Body Body;

        public PhysicsInfoItem(Body body)
        {
            Body = body;
            float radius;
            Point dimension;
            Dimension d;
            SceneDeserializer.GetUserData(body.UserData.ToString(), out type, out radius, out dimension, out d);

            text = new FastText(new Font("Content/font.ttf"), 20)
            {
                Text = type + " : " + (type.ToLower().Trim() == "circle" ? radius.ToString() : dimension.ToString()),
                Color = Color.Black,
                Size = new Vector2f(240, 30)
            };
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
            text.Render(target, position);
        }
    }
}