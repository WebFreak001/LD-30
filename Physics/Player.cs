using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LudumDare.Json;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using System;

namespace LudumDare.Physics
{
    public class Player
    {
        public Body PhysObj;

        private bool dimension0 = false;

        private PhysicsWorld world;

        private TimeSpan total;
        private TimeSpan finishTimer;

        private int texStep = 0;

        private Sprite playerSprite;

        private Texture[] textures;
        private Texture fall;

        private Point spawn;

        public Point FinishHigh { get; set; }

        public Point FinishLow { get; set; }

        public event EventHandler OnWin;

        public Player(Point spawn, PhysicsWorld world)
        {
            PhysObj = BodyFactory.CreateCapsule(world.world, 5, 3, 400, "capsule;5;3;None;player");
            PhysObj.BodyType = BodyType.Dynamic;
            PhysObj.Position = new Vector2(spawn.X, spawn.Y);
            PhysObj.IsBullet = true;
            PhysObj.FixedRotation = true;
            PhysObj.Friction = 0.8f;
            PhysObj.Restitution = 0;
            this.spawn = spawn;
            world.Add(new BodyEx(PhysObj));
            this.world = world;
            total = TimeSpan.Zero;
            finishTimer = TimeSpan.Zero;
            playerSprite = new Sprite();
            textures = new Texture[7];
            for (int i = 0; i < 7; i++)
            {
                textures[i] = new Texture("Content/playerWalk" + (i + 1) + ".png");
            }
            fall = new Texture("Content/playerFall.png");
            playerSprite.Origin = new Vector2f(55, 64);
            playerSprite.Scale = new Vector2f(0.9f, 0.9f);
            playerSprite.Texture = textures[0];
        }

        public void SwapDimension()
        {
            dimension0 = !dimension0;
            world.ChangeDimension(dimension0);
        }

        public void Render(TimeSpan delta, RenderTarget target)
        {
            Vector2 v = PhysObj.Position * 10;

            if (finishTimer >= TimeSpan.FromSeconds(1.1))
            {
                Vector2 pos = PhysObj.Position;
                if (pos.X > FinishLow.X && pos.X < FinishHigh.X &&
                   pos.Y > FinishLow.Y && pos.Y < FinishHigh.Y)
                {
                    if (OnWin != null)
                        OnWin(this, EventArgs.Empty);
                }
                finishTimer = TimeSpan.Zero;
            }

            if (Math.Abs(PhysObj.LinearVelocity.X) > 2)
            {
                total += delta;
                if (total >= TimeSpan.FromSeconds(0.1))
                {
                    total -= TimeSpan.FromSeconds(0.1);
                    texStep++;
                    if (texStep > 6)
                    {
                        texStep = 1;
                    }
                }
                if (PhysObj.LinearVelocity.X > 1)
                {
                    playerSprite.Scale = new Vector2f(0.9f, 0.9f);
                }
                if (PhysObj.LinearVelocity.X < -1)
                {
                    playerSprite.Scale = new Vector2f(-0.9f, 0.9f);
                }
            }
            else
            {
                total = TimeSpan.Zero;
                playerSprite.Scale = new Vector2f(0.9f, 0.9f);
                texStep = 0;
            }
            finishTimer += delta;
            playerSprite.Texture = textures[texStep];
            if (PhysObj.LinearVelocity.Y > 10) playerSprite.Texture = fall;
            playerSprite.Position = new Vector2f(v.X, v.Y);
            target.Draw(playerSprite);
        }
    }
}