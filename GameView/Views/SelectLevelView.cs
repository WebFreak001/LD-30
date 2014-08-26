using LudumDare.Control;
using LudumDare.Json;
using LudumDare.Physics;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using System;
using System.IO;

namespace LudumDare.GameView.Views
{
    public class SelectLevelView : IGameView
    {
        private Scene scene;
        private MainMenuView menu;

        private FastButton backButton;

        public SelectLevelView(MainMenuView menu)
        {
            scene = new Scene(ScrollInputs.None);
            Font text = new Font("Content/font.ttf");
            (backButton = new FastButton(text, 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 600),
                Size = new Vector2f(280, 49),
                Text = "Back to Menu",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += BackToMenu;

            string[] files = Directory.GetFiles("Content/Levels/", "*.json");
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string name = JsonConvert.DeserializeObject<GameScene>(File.ReadAllText(file)).Name;
                FastButton level = new FastButton(text, 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
                {
                    Position = new Vector2f(300, 50 + i * 50),
                    Size = new Vector2f(680, 49),
                    Text = "Play " + name,
                    Anchor = AnchorPoints.Left | AnchorPoints.Top
                };
                level.OnClick += (s, e) =>
                {
                    Next(this, new InGameView(file, this));
                };

                scene.AddComponent(level);
            }

            scene.AddComponent(backButton);
            this.menu = menu;
        }

        private void BackToMenu(object sender, EventArgs e)
        {
            Next(this, menu);
        }

        public void Update(TimeSpan delta)
        {
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
        }

        public event EventHandler<IGameView> Next;
    }
}