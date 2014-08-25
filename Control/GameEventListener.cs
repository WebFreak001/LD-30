using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using LudumDare.Physics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using sfml_ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.Control
{
    public class GameEventListener : UIComponent
    {
        public bool IsFocusable { get { return false; } }

        public Vector2f MaxSize { get; set; }

        public Vector2f MinSize { get; set; }

        public Vector2f Size { get; set; }

        public Vector2f Position { get; set; }

        public bool IsFocused { get; set; }

        public uint TextSize { get; set; }

        public AnchorPoints Anchor { get; set; }

        public Alignment TextAlignment { get; set; }

        public FloatRect Padding { get; set; }

        public Color BackgroundColor { get; set; }

        private PhysicsWorld world;
        private Player player;
        private bool onGround;
        private DateTime lastJump;
        private bool switchReleased = true;
        private bool Left, Right;

        public GameEventListener(PhysicsWorld world, Player player)
        {
            this.world = world;
            this.player = player;
        }

        public void HandleKeyDown(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
            if (key == Keyboard.Key.Space)
            {
                CheckOnGround();
                if (onGround && lastJump < DateTime.Now - TimeSpan.FromSeconds(0.3))
                {
                    player.PhysObj.LinearVelocity += new Vector2(0, -32);
                    lastJump = DateTime.Now;
                }
            }
            if (key == Keyboard.Key.LShift)
            {
                if (switchReleased)
                {
                    player.SwapDimension();
                    switchReleased = false;
                }
            }
            Left = key == Keyboard.Key.A ? true : Left;
            Right = key == Keyboard.Key.D ? true : Right;
        }

        public void CheckOnGround()
        {
            RayCastInput input = new RayCastInput();
            input.Point1 = player.PhysObj.Position;
            input.Point2 = input.Point1 + 10 * new Vector2((float)Math.Sin(0), (float)Math.Cos(0));
            input.MaxFraction = 1;
            float closest = 1;
            foreach (Body b in world.world.BodyList)
            {
                if (b.Enabled)
                    foreach (Fixture f in b.FixtureList)
                    {
                        if (f.CollisionCategories == Category.Cat16 || f.CollisionCategories == Category.Cat25)
                            continue;
                        RayCastOutput output;
                        if (!f.RayCast(out output, ref input, 0))
                            continue;
                        if (output.Fraction < closest)
                        {
                            closest = output.Fraction;
                        }
                    }
            }
            onGround = closest < 0.6f;
        }

        public void HandleKeyUp(Keyboard.Key key, bool Ctrl, bool Shift, bool Alt, bool Windows)
        {
            if (key == Keyboard.Key.LShift)
            {
                switchReleased = true;
            }
            Left = key == Keyboard.Key.A ? false : Left;
            Right = key == Keyboard.Key.D ? false : Right;
            if (key == Keyboard.Key.R && Ctrl && Shift)
            {
                player.PhysObj.Position = new Vector2(10, 10);
            }
        }

        public void HandleMouseDown(Vector2i mousePosition, Mouse.Button button)
        {
        }

        public void HandleMouseUp(Vector2i mousePosition, Mouse.Button button)
        {
        }

        public void HandleMouseMove(Vector2i mousePosition)
        {
        }

        public void Update(float delta)
        {
            CheckOnGround();
            if (Left && onGround && player.PhysObj.LinearVelocity.X > -20) player.PhysObj.LinearVelocity += new Vector2(-0.3f, 0) * delta;
            if (Right && onGround && player.PhysObj.LinearVelocity.X < 20) player.PhysObj.LinearVelocity += new Vector2(0.3f, 0) * delta;
            if (Left && onGround) player.PhysObj.LinearVelocity += new Vector2(-0.01f, 0) * delta;
            if (Right && onGround) player.PhysObj.LinearVelocity += new Vector2(0.01f, 0) * delta;
            if (Left && !onGround) player.PhysObj.LinearVelocity += new Vector2(-0.01f, 0) * delta;
            if (Right && !onGround) player.PhysObj.LinearVelocity += new Vector2(0.01f, 0) * delta;
            if (!onGround) player.PhysObj.LinearVelocity += new Vector2(0, 0.01f) * delta;
            if (onGround && !Left && !Right) player.PhysObj.LinearVelocity = new Vector2(player.PhysObj.LinearVelocity.X * 0.1f, player.PhysObj.LinearVelocity.Y);
            if (player.PhysObj.LinearVelocity.X > 35) player.PhysObj.LinearVelocity = new Vector2(35, player.PhysObj.LinearVelocity.Y);
            if (player.PhysObj.LinearVelocity.X < -35) player.PhysObj.LinearVelocity = new Vector2(-35, player.PhysObj.LinearVelocity.Y);
        }

        public void Render(RenderTarget target, Vector2f position)
        {
        }
    }
}