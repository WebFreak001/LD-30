using Newtonsoft.Json;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare
{
    public class Game : IDisposable
    {
        private RenderWindow window;
        private UISceneManager ui;
        private SceneRenderer render;

        public Game()
        {
        }

        public void Run()
        {
            window = new RenderWindow(new VideoMode(1280, 720), "LD30", Styles.Default);
            window.Closed += window_Closed;
            window.Resized += window_Resized;

            ui = new UISceneManager();
            ui.Init(window);
            Scene scene = new Scene(ScrollInputs.None);
            ButtonControl importButton = new ButtonControl(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png") { Position = new Vector2f(10, 70), Size = new Vector2f(200, 49), Text = "Import", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            importButton.OnClick += importButton_OnClick;
            scene.AddComponent(importButton);
            ui.CurrentScene = scene;

            render = new SceneRenderer();

            Stopwatch sw = new Stopwatch();
            TimeSpan elapsed = TimeSpan.Zero;

            while (window.IsOpen())
            {
                sw.Start();
                window.DispatchEvents();
                window.Clear();
                render.world.Step((float)elapsed.TotalSeconds);

                render.Render(window);

                ui.Render(window);

                window.Display();
                sw.Stop();
                elapsed = sw.Elapsed;
                sw.Reset();
            }
        }

        private void window_Resized(object sender, SizeEventArgs e)
        {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private void importButton_OnClick(object sender, EventArgs e)
        {
            render.Clear();
            SceneDeserializer s = new SceneDeserializer(JsonConvert.DeserializeObject<GameScene>(File.ReadAllText("Content/bob.json")));
            s.AddObjects(render.world);
        }

        private void window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }

        public void Dispose()
        {
            window.Dispose();
        }
    }
}