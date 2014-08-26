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
    public class SettingsView : IGameView
    {
        private Scene scene;
        private MainMenuView menu;

        private GameSettings settings;

        private FastButton cloudButton;
        private FastButton menuButton;
        private FastButton volUpButton;
        private FastButton volDownButton;
        private FastText volumeIndicator;

        public SettingsView(MainMenuView menu)
        {
            if (File.Exists("settings.json"))
                settings = JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText("settings.json"));
            else
            {
                settings = new GameSettings();
                settings.EnableClouds = true;
            }

            scene = new Scene(ScrollInputs.None);
            (cloudButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 200),
                Size = new Vector2f(280, 49),
                Text = (settings.EnableClouds ? "Disable" : "Enable") + " Clouds",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += ToggleClouds;

            (volDownButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 300),
                Size = new Vector2f(40, 49),
                Text = "-",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += VolumeDown;

            (volUpButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(740, 300),
                Size = new Vector2f(40, 49),
                Text = "+",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += VolumeUp;

            volumeIndicator = new FastText(new Font("Content/font.ttf"), 22)
            {
                Position = new Vector2f(550, 300),
                Size = new Vector2f(180, 49),
                TextAlignment = Alignment.MiddleCenter,
                Text = "Volume: " + settings.Volume + "%",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            };

            (menuButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 500),
                Size = new Vector2f(280, 49),
                Text = "Back",
                Anchor = AnchorPoints.Left | AnchorPoints.Bottom
            }).OnClick += MenuClick;

            scene.AddComponent(cloudButton);
            scene.AddComponent(menuButton);
            scene.AddComponent(volDownButton);
            scene.AddComponent(volUpButton);
            scene.AddComponent(volumeIndicator);
            this.menu = menu;
        }

        private void VolumeDown(object sender, EventArgs e)
        {
            settings.Volume -= 10f;
            if (settings.Volume < 0)
                settings.Volume = 0;
            volumeIndicator.Text = "Volume: " + settings.Volume + "%";
            Game.BGM.Volume = settings.Volume;
        }

        private void VolumeUp(object sender, EventArgs e)
        {
            settings.Volume += 10f;
            if (settings.Volume > 100)
                settings.Volume = 100;
            volumeIndicator.Text = "Volume: " + settings.Volume + "%";
            Game.BGM.Volume = settings.Volume;
        }

        private void MenuClick(object sender, EventArgs e)
        {
            using (StreamWriter w = new StreamWriter("settings.json", false))
            {
                w.Write(JsonConvert.SerializeObject(settings, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include }));
            }
            Next(this, menu);
        }

        private void ToggleClouds(object sender, EventArgs e)
        {
            settings.EnableClouds = !settings.EnableClouds;
            cloudButton.Text = (settings.EnableClouds ? "Disable" : "Enable") + " Clouds";
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