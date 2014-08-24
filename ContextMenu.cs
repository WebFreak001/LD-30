using LudumDare.Control;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare
{
    public class ContextMenu
    {
        private List<FastText> options;
        private List<Action> clickActions;
        private bool isVisible;
        public Vector2f Position;
        private RectangleShape shape;

        public bool IsOpen { get { return isVisible; } }

        public ContextMenu()
        {
            options = new List<FastText>();
            clickActions = new List<Action>();
            Position = new Vector2f(0, 0);
            isVisible = false;
            shape = new RectangleShape();
            shape.FillColor = Colors.Gray;
        }

        public void Add(Action click, string Text)
        {
            clickActions.Add(() => { click(); });
            options.Add(new FastText(new Font("Content/font.ttf")) { Size = new Vector2f(300, 20), Color = Color.Black, BackgroundColor = Color.White, Text = Text });
        }

        public void OnClick(Vector2f position)
        {
            if (isVisible && position.Y >= 0 && position.X >= 0 && position.X < 300)
            {
                int i = (int)(position.Y / 20);
                if (i >= 0 && i < clickActions.Count)
                {
                    Console.WriteLine(position.Y + " - " + i);
                    clickActions[i]();
                    Close();
                }
            }
        }

        public void Open(Vector2f position)
        {
            this.Position = position;
            isVisible = true;
        }

        public void Close()
        {
            isVisible = false;
        }

        public void Render(RenderTarget target)
        {
            if (isVisible)
            {
                shape.Size = new Vector2f(302, options.Count * 20 + 2);
                shape.Position = Position - new Vector2f(1, 1);
                target.Draw(shape);
                for (int i = 0; i < options.Count; i++)
                {
                    options[i].Render(target, Position + new Vector2f(0, i * 20));
                }
            }
        }
    }
}