using FarseerPhysics.Dynamics;
using LudumDare.Json;
using SFML.Graphics;
using System;

namespace LudumDare.Physics
{
    public class BodyEx
    {
        public Body Body;

        public DisplayShape Shape { get; set; }

        public float Length;
        public Point Dimension;

        public Dimension GameDimension;
        public Texture Tex;

        public BodyEx(Body body, string texture = "")
        {
            GameDimension = LudumDare.Physics.Dimension.OneO;
            Body = body;
            string type;
            if (texture != "" && texture != null)
            {
                Tex = new Texture(texture);
                Console.WriteLine("Texture Body: " + texture);
            }
            if (body.UserData != null)
            {
                SceneDeserializer.GetUserData(body.UserData.ToString(), out type, out Length, out Dimension, out GameDimension);
                type = type.ToLower().Trim();
                if (type == "circle")
                {
                    Shape = DisplayShape.Circle;
                }
                else if (type == "box")
                {
                    Shape = DisplayShape.Box;
                }
                else if (type == "capsule")
                {
                    Shape = DisplayShape.Capsule;
                }
            }
        }
    }
}