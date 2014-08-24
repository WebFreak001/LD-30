using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace LudumDare.Physics
{
    public class PhysicsWorld
    {
        public World world;
        public List<BodyEx> Bodies;

        public event EventHandler<Body> OnBodyAdded;

        public event EventHandler OnClear;

        private SceneRenderer renderer;

        public Body CamLock { get; set; }

        public PhysicsWorld(World world)
        {
            this.world = world;
            renderer = new SceneRenderer(world);
            Bodies = new List<BodyEx>();
        }

        public PhysicsWorld()
            : this(new World(new Vector2(0, 9.7f)))
        {
        }

        public void Add(Body body)
        {
            if (OnBodyAdded != null)
                OnBodyAdded(this, body);
            Bodies.Add(new BodyEx(body));
        }

        public void Step(float elapsed)
        {
            world.Step(elapsed);
        }

        public void Render(RenderTarget target)
        {
            renderer.Render(target);
        }

        public Body CreateBox(Vector2f dimension, float density, BodyType type)
        {
            Body b = BodyFactory.CreateRectangle(world, dimension.X, dimension.Y, density);
            b.UserData = "box;" + dimension.X + ";" + dimension.Y + ";user";
            b.BodyType = type;
            Add(b);
            return b;
        }

        public Body CreateCircle(float radius, float density, BodyType type)
        {
            Body b = BodyFactory.CreateCircle(world, radius, density);
            b.UserData = "circle;" + radius + ";user";
            b.BodyType = type;
            Add(b);
            return b;
        }

        public void Clear()
        {
            if (OnClear != null)
                OnClear(this, EventArgs.Empty);

            world.Clear();
        }
    }
}