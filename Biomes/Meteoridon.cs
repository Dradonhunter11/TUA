using BiomeLibrary.API;
using BiomeLibrary.Enums;
using Terraria;
using TUA.Tiles.NewBiome.Meteoridon;

namespace TUA.Biomes
{
    class Meteoridon : ModBiome
    {
        public override void SetDefault()
        {
            BiomeAlt = BiomeAlternative.hallowAlt;
            EvilSpecific = EvilSpecific.crimson;
            biomeBlock.Add(mod.TileType<MeteoridonGrass>());
            biomeBlock.Add(mod.TileType<MeteoridonStone>());
            biomeBlock.Add(mod.TileType<MeteoridonSand>());
            biomeBlock.Add(mod.TileType<BrownIce>());
            biomeBlock.Add(mod.TileType<MeteoridonHardenedSand>());
            biomeBlock.Add(mod.TileType("MeteoridonSandstone"));
            MinimumTileRequirement = 150;
        }

        public override bool BiomeAltGeneration(ref string message)
        {
            message = "A meteor strike is happening";
            return false;
        }
    }
}
