﻿using LudumDare.GameView;
using LudumDare.GameView.Views;
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
        private IGameView view;

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
            ui.CurrentScene = new Scene(ScrollInputs.None);

            render = new SceneRenderer();

            Stopwatch sw = new Stopwatch();
            TimeSpan elapsed = TimeSpan.Zero;

            view = new MainMenuView();
            view.Next += view_Next;

            while (window.IsOpen())
            {
                sw.Start();
                window.DispatchEvents();
                window.Clear();
                render.world.Step((float)elapsed.TotalSeconds);
                view.Update();

                //render.Render(window);

                view.Render(window, ui);
                ui.Render(window);

                window.Display();
                sw.Stop();
                elapsed = sw.Elapsed;
                sw.Reset();
            }
        }

        private void view_Next(object sender, IGameView e)
        {
            view = e;
            view.Next += view_Next;
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