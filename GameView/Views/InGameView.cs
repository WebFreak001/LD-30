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
    public class InGameView : IGameView
    {
        private Scene scene;
        private PhysicsWorld world;
        public static Player player;
        private GameEventListener listener;
        private TimeSpan total = TimeSpan.Zero;
        private Texture clouds;
        private float t;
        public Sprite cloudSprite;
        private Sprite bg;
        private Texture bgGradient;
        private GameSettings settings;
        private Sprite portal;
        private IGameView mainMenu;
        private Sprite bgFore;
        private Sprite bgBack;

        public InGameView(string level, IGameView v)
        {
            mainMenu = v;
            if (File.Exists("settings.json"))
                settings = JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText("settings.json"));
            else
            {
                settings = new GameSettings();
                settings.EnableClouds = true;
            }
            bgFore = new Sprite(new Texture("Content/bgFore.png") { Smooth = true });
            bgFore.Origin = new Vector2f(0, 256);
            bgFore.Scale = new Vector2f(2, 2);
            bgBack = new Sprite(new Texture("Content/bgBack.png") { Smooth = true });
            bgBack.Origin = new Vector2f(0, 256);
            bgBack.Scale = new Vector2f(2, 2);
            world = new PhysicsWorld();
            SceneDeserializer d = new SceneDeserializer(SceneLoader.Load(level));
            player = new Player(d.Scene.StartCoords, world);
            d.AddObjects(world);
            player.FinishHigh = d.Scene.FinishHigh;
            player.FinishLow = d.Scene.FinishLow;
            portal = new Sprite(new Texture("Content/portal.png") { Smooth = true });
            portal.Scale = new Vector2f(2, 2);
            portal.Position = new Vector2f(player.FinishLow.X + (player.FinishHigh.X - player.FinishLow.X) * 0.5f, player.FinishLow.Y + (player.FinishHigh.Y - player.FinishLow.Y) * 0.5f) * 10;
            scene = new Scene(ScrollInputs.None);
            listener = new GameEventListener(world, player);
            listener.OnWin += listener_OnWin;
            scene.AddComponent(listener);
            cloudSprite = new Sprite();
            clouds = new Texture(256, 64);
            clouds.Smooth = true;
            if (settings.EnableClouds)
            {
                GenerateClouds();
            }
            bgGradient = new Texture("Content/bg.png");
            bg = new Sprite(bgGradient);
        }

        private void listener_OnWin(object sender, EventArgs e)
        {
            world.Clear();
            Next(this, mainMenu);
        }

        public void Update(TimeSpan delta)
        {
            world.Step((float)delta.TotalSeconds);
            listener.Update((float)delta.TotalMilliseconds);
        }

        public void GenerateClouds()
        {
            byte[] pixels = new byte[256 * 64 * 4];
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    float d = Noise.Generate(x * 0.04f + t * 0.04f, y * 0.04f + t * 0.04f, t * 6) + 1;
                    d *= 1 - y / 64.0f;
                    pixels[(x + y * 256) * 4] = (byte)(255);
                    pixels[(x + y * 256) * 4 + 1] = (byte)(255);
                    pixels[(x + y * 256) * 4 + 2] = (byte)(255);
                    pixels[(x + y * 256) * 4 + 3] = (byte)(d * 64);
                }
            }
            clouds.Update(pixels);
            cloudSprite.Texture = clouds;
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
            bg.Scale = new Vector2f(1280, 1);
            target.Draw(bg);
            bgBack.Position = new Vector2f(player.PhysObj.Position.X * 0.1f, 720);
            target.Draw(bgBack);
            bgFore.Position = new Vector2f(player.PhysObj.Position.X * 0.2f, 720);
            target.Draw(bgFore);
            if (settings.EnableClouds)
            {
                total += delta;
                if (total > TimeSpan.FromSeconds(0.1))
                {
                    t += 0.005f;
                    GenerateClouds();
                    total = TimeSpan.Zero;
                }
                RenderUI(delta, target);
            }
            View v = target.GetView();
            var vx = player.PhysObj.Position * 10;
            v.Center = new Vector2f(vx.X, vx.Y);
            v.Zoom(1.6f);
            target.SetView(v);
            world.Render(target);
            target.Draw(portal);
            player.Render(delta, target);
            View vi = target.GetView();
            vi.Center = new Vector2f(640, 360);
            vi.Zoom(0.625f);
            target.SetView(vi);
        }

        private void RenderUI(TimeSpan delta, RenderTarget target)
        {
            cloudSprite.Position = new Vector2f(0, 0);
            cloudSprite.Scale = new Vector2f(5, 5);
            target.Draw(cloudSprite);
        }

        public event EventHandler<IGameView> Next;
    }
}