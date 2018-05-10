using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse
{
    class BackgroundSwapper : ModUgBgStyle
    {

        public override bool ChooseBgStyle()
        {
            return Main.menuMode == 0;
        }

        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = 120;
            textureSlots[1] = 119;
            textureSlots[2] = 162;
            textureSlots[3] = 163;
            
        }
    }
}
