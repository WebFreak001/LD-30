﻿using FarseerPhysics.Collision;
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
        private Vector2f startClick;
        private Body selected = null;

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

            world = new PhysicsWorld();

            ui = new UISceneManager();
            ui.Init(window);
            Scene scene = new Scene(ScrollInputs.None);
            RectControl bg = new RectControl() { Position = new Vector2f(0, 0), Size = new Vector2f(1280, 50), Anchor = AnchorPoints.Left | AnchorPoints.Top | AnchorPoints.Right, BackgroundColor = Colors.WhiteSmoke };

            FastButton runButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/playButton.png", "Content/playButton.png", "Content/playButton.png") { Position = new Vector2f(0, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            runButton.OnClick += (s, e) => { enabled = true; };
            FastButton pauseButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/pauseButton.png", "Content/pauseButton.png", "Content/pauseButton.png") { Position = new Vector2f(50, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            pauseButton.OnClick += (s, e) => { enabled = false; };
            FastButton addBoxButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/boxButton.png", "Content/boxButton.png", "Content/boxButton.png") { Position = new Vector2f(100, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            addBoxButton.OnClick += (s, e) => { world.CreateBox(new Vector2f(10, 10), 4, BodyType.Static); };
            FastButton addCircleButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/circleButton.png", "Content/circleButton.png", "Content/circleButton.png") { Position = new Vector2f(150, 0), Size = new Vector2f(50, 48), Text = "", Anchor = AnchorPoints.Left | AnchorPoints.Top };
            addCircleButton.OnClick += (s, e) => { world.CreateCircle(5, 5, BodyType.Static); };

            scene.AddComponent(bg);
            scene.AddComponent(runButton);
            scene.AddComponent(pauseButton);
            scene.AddComponent(addBoxButton);
            scene.AddComponent(addCircleButton);
            scene.AddComponent(new WorldHierachyRenderer(world) { Size = new Vector2f(300, 670), Position = new Vector2f(0, 50), BackgroundColor = Colors.Snow });
            ui.CurrentScene = scene;
            Import();
            Export();

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
                Update();

                if (enabled)
                    world.Step((float)elapsed.TotalSeconds);

                v = window.GetView();
                v.Zoom(zoom);
                v.Center = (world.CamLock == null ? (new Vector2f(640, 360) - offset) : new Vector2f(world.CamLock.Position.X * 10, world.CamLock.Position.Y * 10));
                if (world.CamLock != null) v.Rotation = world.CamLock.Rotation * 57.2957795f;
                else v.Rotation = 0;
                window.SetView(v);

                world.Render(window);

                v = window.GetView();
                v.Size = new Vector2f(1280, 720);
                v.Rotation = 0;
                v.Center = new Vector2f(640, 360);
                window.SetView(v);

                ui.Render(window);

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

        private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                Vector2f point = ((new Vector2f(e.X, e.Y) - offset) / zoom) * 0.1f;
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
                    Console.WriteLine("Selected");
                    return true;
                }, ref aabb);
            }
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

        public void Update()
        {
        }

        private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Middle)
            {
                zoom = 1;
                offset = new Vector2f();
            }
            else if (e.Button == Mouse.Button.Left)
            {
                selected = null;
            }
        }

        private void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                offset += new Vector2f(e.X, e.Y) - old;
            }
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (selected != null && !enabled)
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

        public void Import()
        {
            world.Clear();
            SceneDeserializer s = new SceneDeserializer(JsonConvert.DeserializeObject<GameScene>(File.ReadAllText("Content/bob.json")));
            s.AddObjects(world);
        }

        public void Export()
        {
            string[] files = Directory.GetFiles("Content/", "*.json");
            string max = files.Where(file => file.StartsWith("LevelSave")).Select(name => name.Substring(9).Trim()).Select(name => name.Substring(0, name.IndexOf('.'))).Max();
            int maxN = 0;
            string save = "LevelSave";
            if (int.TryParse(max, out maxN))
            {
                save += max + 1;
            }
            else
            {
                save += "1";
            }
            save += ".json";
            Console.WriteLine(save);
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