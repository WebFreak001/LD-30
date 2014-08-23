using FarseerPhysics.Dynamics;
using LudumDare.Json;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;

namespace LudumDare
{
    public class SceneRenderer
    {
        public World world;
        public CircleShape unitCircle;
        public RectangleShape unitRect;

        public SceneRenderer()
        {
            world = new World(new Vector2(0, 9.7f));
            unitCircle = new CircleShape(5.0f, 32);
            unitRect = new RectangleShape(new Vector2f(10, 10));
            unitRect.Origin = new Vector2f(5, 5);
            unitCircle.Origin = new Vector2f(5, 5);
            unitCircle.FillColor = Color.White;
            unitRect.FillColor = Color.White;
        }

        public void Clear()
        {
            world.Clear();
        }

        public void Render(RenderTarget target)
        {
            foreach (Body body in world.BodyList)
            {
                string type;
                float radius;
                Point dimension;
                SceneDeserializer.GetUserData(body.UserData.ToString(), out type, out radius, out dimension);

                if (type == "circle")
                {
                    unitCircle.Scale = new Vector2f(radius, radius) * 2;
                    unitCircle.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10);
                    target.Draw(unitCircle);
                }
                else
                {
                    unitRect.Scale = new Vector2f(dimension.X, dimension.Y);
                    unitRect.Position = new Vector2f(body.Position.X * 10, body.Position.Y * 10);
                    unitRect.Rotation = MathHelper.ToDegrees(body.Rotation);
                    target.Draw(unitRect);
                }
            }
        }
    }
}