using Microsoft.Xna.Framework;
using Terraria;

namespace TUA.NPCs.NewBiome.Meteoridon.EvolvedMeteorideWorm
{
    class EvolvedMeteorideWormHead : EvolvedMeteorideWorm
    {
        public override void SetDefaults()
        {
            npc.width = 58;
            npc.height = 66;
            npc.life = 5000;
            npc.defense = 50;
            npc.damage = 75;
            npc.aiStyle = -1;
            npc.color = Color.Aqua;
        }

        public override void Init()
        {
            base.Init();
            head = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void CustomBehavior()
        {
            
            
        }
    }
}