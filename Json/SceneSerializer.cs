using FarseerPhysics.Dynamics;
using LudumDare.Physics;
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

        public void AddBody(BodyEx body)
        {
            SceneObject obj = new SceneObject();
            obj.Shape = new PhysicsShape();
            obj.Shape.Bullet = body.Body.IsBullet;
            obj.Shape.Dimension = body.Dimension;
            obj.Shape.FixedRotation = body.Body.FixedRotation;
            obj.Shape.Friction = body.Body.Friction;
            obj.Shape.IgnoreGravity = body.Body.IgnoreGravity;
            obj.Shape.Mass = body.Body.Mass;
            obj.Shape.Mesh = body.Shape.ToString().ToLower();
            obj.Shape.Position = new Point(body.Body.Position.X, body.Body.Position.Y);
            obj.Shape.Radius = body.Length;
            obj.Shape.Restitution = body.Body.Restitution;
            obj.Shape.Rotation = body.Body.Rotation;
            obj.Shape.Type = body.Body.BodyType.ToString();
            obj.Shape.GameDimension = body.GameDimension;
            obj.Shape.UserData = "";
            obj.Texture = body.TexPath;
            obj.TexStart = new Point(body.TexRect.Left, body.TexRect.Top);
            obj.TexSize = new Point(body.TexRect.Width, body.TexRect.Height);
            scene.Objects.Add(obj);
        }

        public void Save(string file)
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(scene, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore }));
        }
    }
}