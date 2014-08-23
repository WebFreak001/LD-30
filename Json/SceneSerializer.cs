using FarseerPhysics.Dynamics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LudumDare.Json
{
    public class SceneSerializer
    {
        private GameScene scene;

        public GameScene Scene { get { return scene; } }

        public SceneSerializer(string name)
        {
            scene = new GameScene();
            scene.Name = name;
            scene.Created = DateTime.Now;
            scene.Objects = new List<SceneObject>();
        }

        public void AddBody(Body body, Point size, float? radius, string type)
        {
            SceneObject obj = new SceneObject();
            obj.Shape = new PhysicsShape();
            obj.Shape.Bullet = body.IsBullet;
            if (size != null) obj.Shape.Dimension = size;
            obj.Shape.FixedRotation = body.FixedRotation;
            obj.Shape.Friction = body.Friction;
            obj.Shape.IgnoreGravity = body.IgnoreGravity;
            obj.Shape.Mass = body.Mass;
            obj.Shape.Mesh = type;
            obj.Shape.Position = new Point(body.Position.X, body.Position.Y);
            if (radius.HasValue) obj.Shape.Radius = radius.Value;
            obj.Shape.Restitution = body.Restitution;
            obj.Shape.Rotation = body.Rotation;
            obj.Shape.Type = body.BodyType.ToString();
            obj.Shape.UserData = "";
            scene.Objects.Add(obj);
        }

        public void Save(string file)
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(scene, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore }));
        }
    }
}