using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.LiquidAPI
{
    class LiquidWorld : ModWorld
    {
        public override void PreUpdate()
        {
            if (Liquid.skipCount >= 0)
            {
                LiquidExtension.UpdateLiquid();
            }

            Liquid.skipCount++;
        }
    }
}
