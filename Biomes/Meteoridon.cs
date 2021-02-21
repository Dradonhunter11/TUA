using BiomeLibrary;
using BiomeLibrary.Enums;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using TUA.Tiles.Meteoridon;

namespace TUA.Biomes
{
    class Meteoridon : ModBiome
    {
        public new IReadOnlyList<int> BiomeBlockTypes => new List<int>()
        {
            ModContent.TileType<MeteoridonGrass>(),
            ModContent.TileType<MeteoridonStone>(),
            ModContent.TileType<MeteoridonSand>(),
            ModContent.TileType<BrownIce>(),
            ModContent.TileType<MeteoridonHardenedSand>(),
            ModContent.TileType<MeteoridonSandstone>()
        };

        public override int MinimumTileRequirement => 150;

        public override void SetDefault()
        {
            BiomeAlternative = BiomeAlternative.Hallow;
            
        }

        public override bool BiomeAlternativeGeneration(ref string message)
        {
            message = "A meteor strike is happening";
            return base.BiomeAlternativeGeneration(ref message);
        }
    }
}
