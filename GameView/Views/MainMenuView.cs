﻿using LudumDare.Control;
using LudumDare.ParticleSystem;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using sfml_ui.Controls;
using System;

namespace LudumDare.GameView.Views
{
    public class MainMenuView : IGameView, IDisposable
    {
        private Scene scene;

        public FastButton playButton;
        public FastButton settingsButton;
        public FastButton quitButton;
        public IndexedParticleSystem particles;
        public Sprite particle;
        private Emitter particleEmitter;
        private Emitter greenEmitter;
        private float Time;
        private RenderTexture particleTexture;
        private Sprite bg;

        public MainMenuView()
        {
            scene = new Scene(ScrollInputs.None);

            (playButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 200),
                Size = new Vector2f(280, 49),
                Text = "Play",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += PlayButton_OnClick;

            (settingsButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 260),
                Size = new Vector2f(280, 49),
                Text = "Settings",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += SettingsButton_OnClick;

            (quitButton = new FastButton(new Font("Content/font.ttf"), 22, "Content/button.png", "Content/button_hover.png", "Content/button_pressed.png")
            {
                Position = new Vector2f(500, 320),
                Size = new Vector2f(280, 49),
                Text = "Quit",
                Anchor = AnchorPoints.Left | AnchorPoints.Top
            }).OnClick += QuitButton_OnClick;

            scene.AddComponent(playButton);
            scene.AddComponent(settingsButton);
            scene.AddComponent(quitButton);

            particles = new IndexedParticleSystem();
            particle = new Sprite(new Texture("Content/particle.png"));
            particle.Origin = new Vector2f(32, 32);

            particleEmitter = new Emitter();
            particleEmitter.Position = new Vector2f(50, 50);
            particleEmitter.ParticlesSpawnRate = 200;
            particleEmitter.Spread = 30;
            particleEmitter.Color = Color.Blue;
            particles.AddEmitter(particleEmitter);

            greenEmitter = new Emitter();
            greenEmitter.Position = new Vector2f(50, 50);
            greenEmitter.ParticlesSpawnRate = 150;
            greenEmitter.Spread = 30;
            greenEmitter.Color = Colors.Lime;
            particles.AddEmitter(greenEmitter);

            bg = new Sprite();
            particleTexture = new RenderTexture(1280, 720);
            particleTexture.Clear(Color.Blue);
        }

        private void PlayButton_OnClick(object sender, EventArgs e)
        {
            Next(this, new SelectLevelView(this));
        }

        private void SettingsButton_OnClick(object sender, EventArgs e)
        {
            Next(this, new SettingsView(this));
        }

        private void QuitButton_OnClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void Update(TimeSpan delta)
        {
            particles.Update(delta);
            Time += (float)delta.TotalSeconds * 2;
            particleEmitter.Position = new Vector2f((float)Math.Sin(Time) * 540 + 640, (float)Math.Cos(Time) * 260 + 160);
            greenEmitter.Position = new Vector2f((float)-Math.Sin(Time) * 340 + 640, (float)Math.Cos(Time) * 60 + 160);
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            target.Clear(Colors.White);
            particles.Render(particleTexture, particle);
            bg.Texture = particleTexture.Texture;
            bg.Color = new Color(150, 150, 150, 150);
            target.Draw(bg);
            ui.CurrentScene = scene;
        }

        public void Dispose()
        {
            particleTexture.Dispose();
        }

        public event EventHandler<IGameView> Next;
    }
}