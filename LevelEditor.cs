using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LudumDare.Control;
using LudumDare.Json;
using LudumDare.Physics;
using Microsoft.Xna.Framework;
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

namespace LudumDare
{
    public class LevelEditor : IDisposable
    {
        private RenderWindow window;
        private UISceneManager ui;
        private PhysicsWorld world;
        private bool enabled;
        private float zoom = 1.0f;
        private Vector2f old = new Vector2f();
        private Vector2f offset = new Vector2f();
        private Vector2f startClick = new Vector2f();
        private Body selected = null;
        private bool dragging = false;
        private ContextMenu contextMenu;
        private object savedData;
        private Scene messageScene;
        private Sprite grid;

        public LevelEditor()
        {
        }

        public void Run()
        {
            window = new RenderWindow(new VideoMode(1280, 720), "30 Editor", Styles.Titlebar | Styles.Close);
            window.Closed += window_Closed;
            window.Resized += window_Resized;
            window.MouseWheelMoved += window_MouseWheelMoved;
            window.MouseButtonPressed += window_MouseButtonPressed;
            window.MouseMoved += window_MouseMoved;
            window.MouseButtonReleased += window_MouseButtonReleased;
            window.KeyPressed += window_KeyPressed;
            window.KeyReleased += window_KeyReleased;

            world = new PhysicsWorld(true);

            ui = new UISceneManager();
            ui.Init(window);
            Scene scene = new Scene(ScrollInputs.None);
            RectControl bg = new RectControl() { Position = new Vector2f(0, 0), Size = new Vector2f(1280, 50), Anchor = AnchorPoints.Left | AnchorPoints.Top | AnchorPoints.Right, BackgroundColor = Colors.WhiteSmoke };

            FastButton runButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/playButton.png", "Content/playButton.png", "Content/playButton.png") { Position = new Vector2f(0, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            runButton.OnClick += (s, e) => { enabled = true; };
            FastButton pauseButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/pauseButton.png", "Content/pauseButton.png", "Content/pauseButton.png") { Position = new Vector2f(50, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            pauseButton.OnClick += (s, e) => { enabled = false; };
            FastButton saveButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/saveButton.png", "Content/saveButton.png", "Content/saveButton.png") { Position = new Vector2f(125, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            saveButton.OnClick += (s, e) => { Export(); };
            FastButton loadButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/loadButton.png", "Content/loadButton.png", "Content/loadButton.png") { Position = new Vector2f(175, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            loadButton.OnClick += (s, e) =>
            {
                var ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Level Files (*.json)|*.json";
                ofd.InitialDirectory = Directory.GetCurrentDirectory();
                var r = ofd.ShowDialog();
                if (r == System.Windows.Forms.DialogResult.OK)
                {
                    Import(ofd.FileName);
                }
            };
            FastButton addBoxButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/boxButton.png", "Content/boxButton.png", "Content/boxButton.png") { Position = new Vector2f(250, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            addBoxButton.OnClick += (s, e) => { world.CreateBox(new Vector2f(10, 10), 4, BodyType.Static).Position = new Vector2(offset.X, offset.Y) * -0.1f; world.Step(0); };
            FastButton addCircleButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/circleButton.png", "Content/circleButton.png", "Content/circleButton.png") { Position = new Vector2f(300, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            addCircleButton.OnClick += (s, e) => { world.CreateCircle(5, 5, BodyType.Static).Position = new Vector2(offset.X, offset.Y) * -0.1f; world.Step(0); };

            scene.AddComponent(bg);
            scene.AddComponent(runButton);
            scene.AddComponent(pauseButton);
            scene.AddComponent(addBoxButton);
            scene.AddComponent(addCircleButton);
            scene.AddComponent(saveButton);
            scene.AddComponent(loadButton);
            // scene.AddComponent(new WorldHierachyRenderer(world) { Size = new Vector2f(300, 670), Position = new Vector2f(0, 50), BackgroundColor = Colors.Snow });
            // Does not update on change dynamic/static
            ui.CurrentScene = scene;

            grid = new Sprite(new Texture("Content/grid.png"));
            grid.Origin = new Vector2f(512, 512);

            contextMenu = new ContextMenu();
            contextMenu.Add(() =>
            {
                selected.BodyType = BodyType.Dynamic;
            }, "Set Dynamic");
            contextMenu.Add(() =>
            {
                selected.BodyType = BodyType.Static;
            }, "Set Static");
            contextMenu.Add(() =>
            {
                world.FindBody(selected).GameDimension = Dimension.None;
            }, "Set No Dimension");
            contextMenu.Add(() =>
            {
                world.FindBody(selected).GameDimension = world.FindBody(selected).GameDimension == Dimension.OneO ? Dimension.TwoX : Dimension.OneO;
            }, "Switch Dimension");
            contextMenu.Add(() =>
            {
                selected.Rotation += 0.0872664626f;
            }, "+5 Rotation");
            contextMenu.Add(() =>
            {
                selected.Rotation += 0.785398163f;
            }, "+45 Rotation");
            contextMenu.Add(() =>
            {
                selected.Rotation -= 0.0872664626f;
            }, "-5 Rotation");
            contextMenu.Add(() =>
            {
                selected.Rotation -= 0.785398163f;
            }, "-45 Rotation");
            contextMenu.Add(() =>
            {
                world.Copy(selected, new Vector2(offset.X, offset.Y) * -0.1f);
            }, "Duplicate");
            contextMenu.Add(() =>
            {
                world.Remove(selected);
            }, "Remove");

            messageScene = new Scene(ScrollInputs.None);

            Stopwatch sw = new Stopwatch();
            TimeSpan elapsed = TimeSpan.Zero;
            TimeSpan secondCounter = TimeSpan.Zero;
            int frames = 0;
            world.Step(0);

            View v;
            Console.WriteLine(window.GetView().Center);

            while (window.IsOpen())
            {
                sw.Start();
                window.DispatchEvents();
                window.Clear();

                if (enabled)
                    world.Step((float)elapsed.TotalSeconds);

                v = window.GetView();
                v.Zoom(zoom);
                v.Center = (world.CamLock == null ? -offset : new Vector2f(world.CamLock.Position.X * 10, world.CamLock.Position.Y * 10));
                if (world.CamLock != null) v.Rotation = world.CamLock.Rotation * 57.2957795f;
                else v.Rotation = 0;
                window.SetView(v);

                window.Draw(grid);

                world.Render(window);

                v = window.GetView();
                v.Size = new Vector2f(1280, 720);
                v.Rotation = 0;
                v.Center = new Vector2f(640, 360);
                window.SetView(v);

                ui.Render(window);
                ui.CurrentScene = messageScene;
                ui.Render(window);
                ui.CurrentScene = scene;
                contextMenu.Render(window);

                window.Display();
                sw.Stop();
                elapsed = sw.Elapsed;
                secondCounter += elapsed;
                frames++;
                if (secondCounter >= TimeSpan.FromSeconds(1))
                {
                    Console.WriteLine(frames / secondCounter.TotalSeconds);
                    secondCounter -= TimeSpan.FromSeconds(1);
                    frames = 0;
                }
                sw.Reset();
            }
        }

        public void OpenDropDownDialog(object data, string title, Action<object, string> action, string defaultSelect, params string[] selects)
        {
            messageScene = new Scene(ScrollInputs.None);
            messageScene.AddComponent(new FastText(new Font("Content/font.ttf"), 22) { Color = Color.Black, BackgroundColor = Colors.LightGrey, Position = new Vector2f(440, 200), TextAlignment = Alignment.MiddleCenter, Text = title, Size = new Vector2f(400, 30) });
            messageScene.AddComponent(new RectControl() { BackgroundColor = Color.White, Size = new Vector2f(400, 300), Position = new Vector2f(440, 230) });
            messageScene.AddComponent(new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png") { Size = new Vector2f(300, 30), Position = new Vector2f(490, 490) });
        }

        private void window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {
                enabled = false;
            }
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {
                enabled = true;
            }
        }

        private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            contextMenu.OnClick(new Vector2f(e.X, e.Y) - contextMenu.Position);
            contextMenu.Close();
            if (e.Button == Mouse.Button.Middle)
            {
                zoom = 1;
                offset = new Vector2f();
            }
            else if (e.Button == Mouse.Button.Left)
            {
                selected = null;
            }
            else if (e.Button == Mouse.Button.Right && selected != null)
            {
                Console.WriteLine(startClick);
                if (Math.Abs(startClick.X) < 4 && Math.Abs(startClick.Y) < 4)
                {
                    contextMenu.Open(new Vector2f(e.X, e.Y));
                    Console.WriteLine(new Vector2f(e.X, e.Y));
                }
            }
            dragging = false;
        }

        private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if ((e.Button == Mouse.Button.Left || e.Button == Mouse.Button.Right) && !contextMenu.IsOpen)
            {
                Vector2f point = ((new Vector2f(e.X, e.Y) - offset - (new Vector2f(window.Size.X, window.Size.Y) * 0.5f))) * 0.1f;
                Console.WriteLine(point);
                AABB aabb = new AABB(new Vector2(point.X, point.Y), 1, 1);

                world.world.QueryAABB((fix) =>
                {
                    var shape = fix.Shape;
                    var pointB2 = new Vector2(point.X, point.Y);
                    FarseerPhysics.Common.Transform transform;
                    fix.Body.GetTransform(out transform);
                    if (shape.TestPoint(ref transform, ref pointB2))
                    {
                        selected = fix.Body;
                        Console.WriteLine("Selected");
                        return false;
                    }
                    return true;
                }, ref aabb);
                dragging = true;
            }
            startClick = new Vector2f(0, 0);
        }

        private void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                offset += new Vector2f(e.X, e.Y) - old;
                startClick += new Vector2f(e.X, e.Y) - old;
            }
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (selected != null && !enabled && dragging)
                {
                    selected.Awake = true;
                    selected.Position += (new Vector2(e.X, e.Y) - new Vector2(old.X, old.Y)) * 0.1f;
                }
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

        public void Import(string file)
        {
            world.Clear();
            SceneDeserializer s = new SceneDeserializer(JsonConvert.DeserializeObject<GameScene>(File.ReadAllText(file)));
            s.AddObjects(world);
        }

        public void Export(int i = 0)
        {
            i++;
            string save = "Content/LevelSave" + i + ".json";
            if (File.Exists(save))
            {
                Export(i);
                return;
            }

            Console.WriteLine("Saving in " + save);

            SceneSerializer serializer = new SceneSerializer(save);

            foreach (BodyEx body in world.Bodies)
            {
                serializer.AddBody(body);
            }

            File.WriteAllText(save, JsonConvert.SerializeObject(serializer.Scene, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore }));
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