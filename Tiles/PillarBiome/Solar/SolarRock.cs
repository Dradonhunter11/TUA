using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Tiles.PillarBiome.Solar
{
    class SolarRock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            this.MergeTile(mod.TileID("SolarMineralObsidian"));
            this.MergeTile(mod.TileID("SolarDirt"));
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(178, 98, 0));
        }
    }
}
