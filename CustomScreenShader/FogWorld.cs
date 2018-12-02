using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.CustomScreenShader
{
    class FogWorld : ModWorld
    {
        public override void PostUpdate()
        {
            ScreenFog.Update(TerrariaUltraApocalypse.SolarFog);
        }

        public override void PostDrawTiles()
        {
            if (BiomeLibs.InBiome("Meteoridon") && Main.netMode == 0)
            {
                ScreenFog.Draw(TerrariaUltraApocalypse.SolarFog, 0.3f, 0.1f);
            }
        }
    }
}
