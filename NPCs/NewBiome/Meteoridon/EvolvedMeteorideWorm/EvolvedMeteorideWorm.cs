using Terraria.ModLoader;
using TUA.NPCs.AI;

namespace TUA.NPCs.NewBiome.Meteoridon.EvolvedMeteorideWorm
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
            headType = ModContent.NPCType<EvolvedMeteorideWormHead>();
            speed = 5.5f;
            turnSpeed = 0.045f;
        }

        public override void CustomBehavior()
        {
            
        }
    }
}
