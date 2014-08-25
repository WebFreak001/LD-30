using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace LudumDare.Physics
{
    public class Player
    {
        public Body PhysObj;

        private bool dimension0 = false;

        private PhysicsWorld world;

        public Player(PhysicsWorld world)
        {
            PhysObj = BodyFactory.CreateCapsule(world.world, 5, 3, 400, "capsule;5;3;None;player");
            PhysObj.BodyType = BodyType.Dynamic;
            PhysObj.Position = new Vector2(10, 0);
            PhysObj.IsBullet = true;
            PhysObj.FixedRotation = true;
            PhysObj.Friction = 0.8f;
            PhysObj.Restitution = 0;
            world.Add(new BodyEx(PhysObj));
            this.world = world;
        }

        public void SwapDimension()
        {
            dimension0 = !dimension0;
            world.ChangeDimension(dimension0);
        }
    }
}