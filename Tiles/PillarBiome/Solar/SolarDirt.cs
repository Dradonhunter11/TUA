using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API;
using TUA.Tiles.NewBiome.Meteoridon;

namespace TUA.Tiles.PillarBiome.Solar
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
