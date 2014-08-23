using FarseerPhysics.Dynamics;
using LudumDare.Json;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Linq;

namespace LudumDare
{
    public class RenderBody
    {
        private DisplayShape type;
        private Shape shape;
        private int id;

        public RenderBody(Body body, int id)
        {
            this.id = id;
            string[] data = body.UserData.ToString().Split(';');
            if (data[0].Trim().ToLower() == "circle") type = DisplayShape.Circle;
            else type = DisplayShape.Box;
            if (type == DisplayShape.Circle)
            {
                shape = new CircleShape(float.Parse(data[1]) * 10, (uint)(float.Parse(data[1]) * 7.5f));
                shape.Origin = new Vector2f(float.Parse(data[1]) * 5, float.Parse(data[1]) * 5);
            }
            else if (type == DisplayShape.Box)
            {
                string[] split = data[1].Trim().Split(' ');
                shape = new RectangleShape(new Vector2f(float.Parse(split[0]) * 10, float.Parse(split[1]) * 10));
                shape.Origin = new Vector2f(float.Parse(split[0]) * 5, float.Parse(split[1]) * 5);
            }
            shape.FillColor = Color.White;
            Console.WriteLine("Created");
        }

        public void Render(World world, RenderTarget target)
        {
            Vector2 pos = world.BodyList.Where(e => e.BodyId == id).FirstOrDefault().Position;
            shape.Position = new Vector2f(pos.X, pos.Y);
            Console.WriteLine(pos);
            target.Draw(shape);
        }
    }
}