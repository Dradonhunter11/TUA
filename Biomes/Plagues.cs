using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary.API;
using BiomeLibrary.Enums;

namespace TerrariaUltraApocalypse.Biomes
{
    class Plagues : ModBiome
    {
        public override void SetDefault()
        {
            BiomeAlt = BiomeAlternative.noAlt;
            biomeBlock.Add(mod.TileType("ApocalypseDirt"));
            MinimumTileRequirement = 300;
        }
    }
}
