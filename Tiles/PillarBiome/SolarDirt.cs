using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Tiles.PillarBiome
{
    class SolarDirt : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[mod.TileType<SolarDirt>()][mod.TileType<SolarRock>()] = true;
            Main.tileMerge[mod.TileType<SolarRock>()][mod.TileType<SolarDirt>()] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));


        }

    }
}
