using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
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

        public void Clear()
        {
            if (OnClear != null)
                OnClear(this, EventArgs.Empty);

            world.Clear();
        }
    }
}