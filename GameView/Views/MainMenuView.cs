using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.GameView.Views
{
    public class MainMenuView : IGameView
    {
        private Scene scene;

        public ButtonControl playButton;
        public ButtonControl settingsButton;
        public ButtonControl quitButton;

        public MainMenuView()
        {
            scene = new Scene(ScrollInputs.None);

            (playButton = new ButtonControl(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 200),
                Size = new Vector2f(280, 49),
                Text = "Play",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += PlayButton_OnClick;

            (settingsButton = new ButtonControl(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 260),
                Size = new Vector2f(280, 49),
                Text = "Settings",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += SettingsButton_OnClick;

            (quitButton = new ButtonControl(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 320),
                Size = new Vector2f(280, 49),
                Text = "Quit",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += QuitButton_OnClick;

            scene.AddComponent(playButton);
            scene.AddComponent(settingsButton);
            scene.AddComponent(quitButton);
        }

        private void PlayButton_OnClick(object sender, EventArgs e)
        {
        }

        private void SettingsButton_OnClick(object sender, EventArgs e)
        {
        }

        private void QuitButton_OnClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void Update()
        {
        }

        public void Render(RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
        }

        public event EventHandler<IGameView> Next;
    }
}