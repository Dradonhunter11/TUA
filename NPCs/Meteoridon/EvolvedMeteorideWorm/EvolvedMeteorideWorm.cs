using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUltraApocalypse.NPCs.AI;

namespace TerrariaUltraApocalypse.NPCs.Meteoridon.EvolvedMeteorideWorm
{
    abstract class EvolvedMeteorideWorm : Worm
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteoride Worm");
        }

        public override void Init()
        {
            minLength = 6;
            maxLength = 12;
            //tailType = mod.NPCType<ExampleWormTail>();
            //bodyType = mod.NPCType<ExampleWormBody>();
            headType = mod.NPCType<EvolvedMeteorideWormHead>();
            speed = 5.5f;
            turnSpeed = 0.045f;
        }
    }
}
