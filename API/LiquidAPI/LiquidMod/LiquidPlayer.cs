using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.LiquidAPI.Swap;

namespace TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod
{
    class LiquidPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            /*bool[] liquidCollision = CollisionSwap.ModdedWetCollision(player.Center, player.width, player.height);
            for (byte i = 0; i < LiquidRegistery.liquidList.Capacity; i++)
            {
                if (liquidCollision[i])
                {
                    LiquidRegistery.PlayerInteraction(i, player);
                }
            }*/
        }
    }
}
