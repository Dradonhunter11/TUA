using TUA.NPCs.AI;

namespace TUA.NPCs.UltraBoss.UltraDestroyer
{
    abstract class UltraDestroyerWorm : Worm
    {
        public override void Init()
        {
            this.minLength = 150;
            this.maxLength = 150;
            this.speed = 4f;
            this.turnSpeed = 0.045f;
            
        }

        public override void CustomBehavior()
        {
            base.CustomBehavior();
        }
    }
}
