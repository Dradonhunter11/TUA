using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Tiles.Solar
{
    class SolarDirt : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            this.MergeTile(mod.TileID("SolarRock"));
            this.MergeTile(mod.TileID("SolarMineralObsidian"));
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));


        }

    }
}
