using LudumDare.Control;
using LudumDare.Json;
using LudumDare.Physics;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using System;

namespace LudumDare.GameView.Views
{
    public class InGameView : IGameView
    {
        private Scene scene;
        private PhysicsWorld world;
        public static Player player;
        private GameEventListener listener;

        public InGameView(string level)
        {
            world = new PhysicsWorld();
            player = new Player(world);
            SceneDeserializer d = new SceneDeserializer(SceneLoader.Load(level));
            d.AddObjects(world);
            scene = new Scene(ScrollInputs.None);
            listener = new GameEventListener(world, player);
            scene.AddComponent(listener);
        }

        public void Update(TimeSpan delta)
        {
            world.Step((float)delta.TotalSeconds);
            listener.Update((float)delta.TotalMilliseconds);
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
            View v = target.GetView();
            var vx = player.PhysObj.Position * 10;
            v.Center = new Vector2f(vx.X, vx.Y);
            v.Zoom(2.0f);
            target.SetView(v);
            world.Render(target);
            View vi = target.GetView();
            var vix = player.PhysObj.Position * 10;
            vi.Center = new Vector2f(vix.X, vix.Y);
            vi.Zoom(0.5f);
            target.SetView(vi);
        }

        public event EventHandler<IGameView> Next;
    }
}