using FarseerPhysics.Dynamics;

namespace LudumDare.Physics
{
    public class Player
    {
        public Body PhysObj;

        private bool dimension0 = true;

        public void SwapDimension()
        {
            dimension0 = !dimension0;
            PhysObj.CollidesWith = (dimension0 ? Category.Cat1 : Category.Cat2) | Category.Cat10;
        }
    }
}