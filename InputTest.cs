using FarseerPhysics.Dynamics;
using LudumDare.Json;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LudumDare
{
    public class InputTest : IDisposable
    {
        private RenderWindow window;
        private UISceneManager ui;
        private SceneRenderer render;
        private Body player;
        private PictureControl dragger;
        private PictureControl draggerR;

        public InputTest()
        {
        }

        public void Run()
        {
            window = new RenderWindow(new VideoMode(1280, 720), "Scene Test", Styles.Default);
            window.Closed += window_Closed;
            window.Resized += window_Resized;
            window.JoystickConnected += window_JoystickConnected;

            render = new SceneRenderer();

            Import();
            render.world.Step(0);
            player = render.world.BodyList.Where(e => e.UserData.ToString().ToLower().Contains("player")).FirstOrDefault();
            if (player == null)
            {
                Console.WriteLine("No player.");
            }

            Stopwatch sw = new Stopwatch();
            TimeSpan elapsed = TimeSpan.Zero;
            TimeSpan total = TimeSpan.Zero;

            ui = new UISceneManager();
            ui.Init(window);
            Scene scene = new Scene(ScrollInputs.None);
            scene.AddComponent(new PictureControl("Content/stick_bg.png") { Position = new Vector2f(20, 540), Size = new Vector2f(160, 160), Anchor = AnchorPoints.Left | AnchorPoints.Bottom });
            dragger = new PictureControl("Content/stick_fg.png") { Position = new Vector2f(52, 572), Size = new Vector2f(96, 96), Anchor = AnchorPoints.Left | AnchorPoints.Bottom };
            scene.AddComponent(dragger);

            scene.AddComponent(new PictureControl("Content/stick_bg.png") { Position = new Vector2f(1100, 540), Size = new Vector2f(160, 160), Anchor = AnchorPoints.Right | AnchorPoints.Bottom });
            draggerR = new PictureControl("Content/stick_fg.png") { Position = new Vector2f(1132, 572), Size = new Vector2f(96, 96), Anchor = AnchorPoints.Right | AnchorPoints.Bottom };
            scene.AddComponent(draggerR);
            ui.CurrentScene = scene;

            while (window.IsOpen())
            {
                sw.Start();
                window.DispatchEvents();
                window.Clear();
                render.world.Step((float)elapsed.TotalSeconds);

                Joystick.Update();
                Update();

                render.Render(window);

                ui.Render(window);

                window.Display();
                sw.Stop();
                elapsed = sw.Elapsed;
                total += elapsed;
                sw.Reset();
            }
        }

        private void window_JoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            Console.WriteLine("Connected: " + e.JoystickId);
        }

        private void Update()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                player.LinearVelocity += new Vector2(0, -0.03f);
            }
            if (Joystick.IsConnected(0))
            {
                dragger.Position = new Vector2f(52 + (Joystick.GetAxisPosition(0, Joystick.Axis.X) * 0.01f) * 32.0f, 572 + (Joystick.GetAxisPosition(0, Joystick.Axis.Y) * 0.01f) * 32.0f);
                draggerR.Position = new Vector2f(1132 + (Joystick.GetAxisPosition(0, Joystick.Axis.U) * 0.01f) * 32.0f, 572 + (Joystick.GetAxisPosition(0, Joystick.Axis.R) * 0.01f) * 32.0f);

                if (Joystick.GetAxisPosition(0, Joystick.Axis.Y) * 0.01f < 0.05f)
                {
                    player.LinearVelocity += new Vector2(0, 0.03f * Joystick.GetAxisPosition(0, Joystick.Axis.Y) * 0.01f);
                }
                //Console.WriteLine("R: " + Joystick.GetAxisPosition(0, Joystick.Axis.R));
                //Console.WriteLine("U: " + Joystick.GetAxisPosition(0, Joystick.Axis.U));

                //Console.WriteLine("X: " + Joystick.GetAxisPosition(0, Joystick.Axis.X));
                //Console.WriteLine("Y: " + Joystick.GetAxisPosition(0, Joystick.Axis.Y));
            }
        }

        private void window_Resized(object sender, SizeEventArgs e)
        {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private void Import()
        {
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