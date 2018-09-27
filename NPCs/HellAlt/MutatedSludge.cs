using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.NPCs.HellAlt
{
    class MutatedSludge : TUAModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutated Sludge");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.YellowSlime);
            npc.lifeMax = 200;
            npc.aiStyle = 1;
            npc.defense = 50;
            npc.damage = 70;
            npc.value = Item.buyPrice(0, 0, 50, 0);
            npc.width = 44;
            npc.height = 32;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax += numPlayers / 2 + 100;
            npc.damage = 70;
        }

        public override void AI()
        {
            if (Main.rand.Next(50) == 0)
            {
                Dust.NewDust(npc.position, 8, 8, DustID.GreenBlood);
            }
        }
    }
}
