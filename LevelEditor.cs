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
    public class LevelEditor : IDisposable
    {
        private RenderWindow window;
        private UISceneManager ui;
        private SceneRenderer render;
        private bool enabled;
        private float zoom = 1.0f;
        private Vector2f old = new Vector2f();
        private Vector2f offset = new Vector2f();

        public LevelEditor()
        {
        }

        public void Run()
        {
            window = new RenderWindow(new VideoMode(1280, 720), "Scene Test", Styles.Default);
            window.Closed += window_Closed;
            window.Resized += window_Resized;
            window.MouseWheelMoved += window_MouseWheelMoved;
            window.MouseMoved += window_MouseMoved;
            window.MouseButtonReleased += window_MouseButtonReleased;

            ui = new UISceneManager();
            ui.Init(window);
            Scene scene = new Scene(ScrollInputs.None);
            TextControl bg = new TextControl(new Font("Content/font.ttf")) { Position = new Vector2f(0, 0), Size = new Vector2f(1280, 50), Anchor = AnchorPoints.Left | AnchorPoints.Top | AnchorPoints.Right, Text = "", BackgroundColor = Colors.WhiteSmoke };

            ButtonControl runButton = new ButtonControl(new Font("Content/font.ttf"), 22, "Content/playButton.png", "Content/playButton.png", "Content/playButton.png") { Position = new Vector2f(0, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            runButton.OnClick += (s, e) => { enabled = !enabled; };

            scene.AddComponent(bg);
            scene.AddComponent(runButton);
            ui.CurrentScene = scene;

            render = new SceneRenderer();
            Import();

            Stopwatch sw = new Stopwatch();
            TimeSpan elapsed = TimeSpan.Zero;
            render.world.Step(0);

            View v;
            Console.WriteLine(window.GetView().Center);

            while (window.IsOpen())
            {
                sw.Start();
                window.DispatchEvents();
                window.Clear();
                if (enabled)
                    render.world.Step((float)elapsed.TotalSeconds);

                v = window.GetView();
                v.Zoom(zoom);
                v.Center = new Vector2f(640, 360) - offset;
                window.SetView(v);

                render.Render(window);

                v = window.GetView();
                v.Size = new Vector2f(1280, 720);
                v.Center = new Vector2f(640, 360);
                window.SetView(v);

                ui.Render(window);

                window.Display();
                sw.Stop();
                elapsed = sw.Elapsed;
                sw.Reset();
            }
        }

        private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Middle)
            {
                zoom = 1;
                offset = new Vector2f();
            }
        }

        private void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                offset += new Vector2f(e.X, e.Y) - old;
            }
            old = new Vector2f(e.X, e.Y);
        }

        private void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            zoom -= e.Delta * 0.1f;
        }

        private void window_Resized(object sender, SizeEventArgs e)
        {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        public void Import()
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