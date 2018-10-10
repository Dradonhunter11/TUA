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
    class LiquidGlobalItem : GlobalItem
    {
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            /*bool[] liquidCollision = CollisionSwap.ModdedWetCollision(item.Center, item.width, item.height);
            for (byte i = 0; i < LiquidRegistery.liquidList.Capacity; i++)
            {
                if (liquidCollision[i])
                {
                    LiquidRegistery.ItemInteraction(i, item);
                }
            }*/
        }
    }
}
