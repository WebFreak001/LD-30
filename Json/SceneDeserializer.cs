using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LudumDare.Physics;
using Microsoft.Xna.Framework;
using System;

namespace LudumDare.Json
{
    public class SceneDeserializer
    {
        private GameScene scene;

        public SceneDeserializer(GameScene scene)
        {
            this.scene = scene;
        }

        public void AddObjects(PhysicsWorld world)
        {
            Console.WriteLine("Generating Scene " + scene.Name);
            foreach (SceneObject obj in scene.Objects)
            {
                PhysicsShape shape = obj.Shape;
                Body body = null;
                if (shape.Mesh.Trim().ToLower() == "rectangle" || shape.Mesh.Trim().ToLower() == "box")
                {
                    body = BodyFactory.CreateRectangle(world.world, shape.Dimension.X, shape.Dimension.Y, shape.Mass);
                }
                else if (shape.Mesh.Trim().ToLower() == "circle")
                {
                    body = BodyFactory.CreateCircle(world.world, shape.Radius, shape.Mass);
                }
                else
                {
                    Console.WriteLine("Skipped invalid mesh type: " + shape.Type);
                    continue;
                }
                string t = shape.Type.Trim().ToLower();
                body.Mass = shape.Mass;
                body.BodyType = t == "kinematic" ? BodyType.Kinematic : (t == "static" ? BodyType.Static : BodyType.Dynamic);
                body.Position = new Vector2(shape.Position.X, shape.Position.Y);
                body.FixedRotation = shape.FixedRotation;
                body.Friction = shape.Friction;
                body.IsBullet = shape.Bullet;
                body.Restitution = shape.Restitution;
                body.Rotation = shape.Rotation;
                body.UserData = (shape.Mesh.Trim().ToLower() == "circle" ? "circle;" : "box;") + (shape.Mesh.Trim().ToLower() == "circle" ? shape.Radius + ";" : shape.Dimension.X + ";" + shape.Dimension.Y + ";") + shape.UserData;
                body.IgnoreGravity = shape.IgnoreGravity;

                world.Add(body);
                Console.WriteLine("Body created: " + body.UserData);
            }
        }

        public static void GetUserData(string userData, out string type, out float radius, out Point dimension)
        {
            string[] splits = userData.Split(';');
            type = splits[0].Trim().ToLower() == "circle" ? "circle" : (splits[0].Trim().ToLower() == "capsule" ? "capsule" : "box");
            if (type == "circle")
            {
                radius = float.Parse(splits[1]);
                dimension = new Point();
            }
            else if (type == "box")
            {
                radius = 0;
                dimension = new Point(float.Parse(splits[1]), float.Parse(splits[2]));
            }
            else
            {
                radius = 0;
                dimension = new Point(float.Parse(splits[1]), float.Parse(splits[2]));
            }
        }
    }
}