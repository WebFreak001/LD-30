using FarseerPhysics.Dynamics;
using LudumDare.Json;
using LudumDare.Physics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;

namespace LudumDare
{
    public class SceneRenderer
    {
        public World world;
        public CircleShape unitCircle;
        public RectangleShape unitRect;
        private Stopwatch sw;

        public SceneRenderer(World world)
        {
            this.world = world;
            unitCircle = new CircleShape(5.0f, 32);
            unitRect = new RectangleShape(new Vector2f(10, 10));
            unitRect.Origin = new Vector2f(5, 5);
            unitCircle.Origin = new Vector2f(5, 5);
            unitCircle.FillColor = Color.White;
            unitRect.FillColor = Color.White;

            sw = new Stopwatch();
        }

        public void Render(RenderTarget target)
        {
            Dimension d;
            foreach (Body body in world.BodyList)
            {
                string type;
                float radius;
                Point dimension;
                SceneDeserializer.GetUserData(body.UserData.ToString(), out type, out radius, out dimension, out d);

                if (type == "circle")
                {
                    unitCircle.Scale = new Vector2f(radius, radius) * 2;
                    unitCircle.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10);
                    unitCircle.Rotation = MathHelper.ToDegrees(body.Rotation);
                    target.Draw(unitCircle);
                }
                else if (type == "box")
                {
                    unitRect.Scale = new Vector2f(dimension.X, dimension.Y);
                    unitRect.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10);
                    unitRect.Rotation = MathHelper.ToDegrees(body.Rotation);
                    target.Draw(unitRect);
                }
                else
                {
                    unitCircle.Scale = new Vector2f(dimension.Y, dimension.Y) * 2;
                    unitCircle.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10 - dimension.X * 5);
                    target.Draw(unitCircle);
                    unitCircle.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10 + dimension.X * 5);
                    target.Draw(unitCircle);
                    unitRect.Scale = new Vector2f(dimension.Y, dimension.X);
                    unitRect.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10);
                    unitRect.Rotation = MathHelper.ToDegrees(body.Rotation);
                    target.Draw(unitRect);
                }
            }
        }
    }
}