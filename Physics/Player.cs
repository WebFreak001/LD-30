using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace LudumDare.Physics
{
    public class Player
    {
        public Body PhysObj;

        private bool dimension0 = true;

        public Player(PhysicsWorld world)
        {
            PhysObj = BodyFactory.CreateCapsule(world.world, 5, 3, 4, "capsule;5;3;player");
            PhysObj.BodyType = BodyType.Dynamic;
            PhysObj.Position = new Vector2(10, 0);
            PhysObj.IsBullet = true;
            PhysObj.FixedRotation = true;
            PhysObj.Friction = 0;
            PhysObj.CollidesWith = Category.Cat1 | Category.Cat10;
            world.Add(PhysObj);
        }

        public void SwapDimension()
        {
            dimension0 = !dimension0;
            PhysObj.CollidesWith = (dimension0 ? Category.Cat1 : Category.Cat2) | Category.Cat10;
        }
    }
}