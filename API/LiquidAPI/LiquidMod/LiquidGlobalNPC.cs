using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.LiquidAPI.Swap;

namespace TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod
{
    class LiquidGlobalNPC : GlobalNPC
    {
        public override void PostAI(NPC npc)
        {
            /*bool[] liquidCollision = CollisionSwap.ModdedWetCollision(npc.Center, npc.width, npc.height);
            for (byte i = 0; i < LiquidRegistery.liquidList.Capacity; i++)
            {
                if (liquidCollision[i])
                {
                    LiquidRegistery.NPCInteraction(i, npc);
                }
            }*/
        }
    }
}
