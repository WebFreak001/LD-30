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
        public IntRect TexRect;

        public string TexPath;

        public BodyEx(Body body)
            : this(body, "", new IntRect())
        {
        }

        public BodyEx(Body body, string texture, IntRect rect)
        {
            GameDimension = LudumDare.Physics.Dimension.OneO;
            Body = body;
            string type;
            if (texture != "" && texture != null)
            {
                Tex = new Texture(texture);
                Tex.Repeated = true;
                Tex.Smooth = true;
                TexPath = texture;
                Console.WriteLine("Texture Body: " + texture);
                TexRect = rect;
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