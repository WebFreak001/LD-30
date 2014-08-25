using FarseerPhysics.Dynamics;
using LudumDare.Json;

namespace LudumDare.Physics
{
    public class BodyEx
    {
        public Body Body { get; set; }

        public DisplayShape Shape { get; set; }

        public float Length;
        public Point Dimension;
        public Dimension GameDimension;

        public BodyEx(Body body)
        {
            GameDimension = LudumDare.Physics.Dimension.None;
            Body = body;
            string type;
            if (body.UserData != null)
            {
                SceneDeserializer.GetUserData(body.UserData.ToString(), out type, out Length, out Dimension);
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