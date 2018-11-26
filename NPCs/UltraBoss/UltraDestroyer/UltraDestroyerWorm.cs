using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUltraApocalypse.NPCs.AI;

namespace TerrariaUltraApocalypse.NPCs.UltraBoss.UltraDestroyer
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
