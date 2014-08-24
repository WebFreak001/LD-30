using FarseerPhysics.Dynamics;
using System.Collections.Generic;

namespace LudumDare.Physics
{
    public class PhysicsWorld
    {
        public World world;
        public List<BodyEx> Bodies;

        public PhysicsWorld(World world)
        {
            this.world = world;
        }

        public void Add(Body body)
        {
        }
    }
}