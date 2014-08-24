using LudumDare.Json;
using LudumDare.Physics;
using Newtonsoft.Json;
using SFML.Graphics;
using sfml_ui;
using System;

namespace LudumDare.GameView.Views
{
    public class InGameView : IGameView
    {
        private Scene scene;
        private SceneRenderer renderer;
        public static Player player;

        public InGameView()
        {
            scene = new Scene(ScrollInputs.None);
            renderer = new SceneRenderer();
            player = new Player(new PhysicsWorld(renderer.world));
            SceneDeserializer d = new SceneDeserializer(SceneLoader.Load("Content/bob.json"));
            d.AddObjects(renderer.world);
        }

        public void Update(TimeSpan delta)
        {
            renderer.world.Step((float)delta.TotalSeconds);
            Console.WriteLine(player.PhysObj.Position);
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
            renderer.Render(target);
        }

        public event EventHandler<IGameView> Next;
    }
}