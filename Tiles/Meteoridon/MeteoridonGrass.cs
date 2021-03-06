﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TUA.Tiles.Meteoridon
{
    class MeteoridonGrass : BaseMeteoridonTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            TileID.Sets.Grass[Type] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));
            SetModTree(new MeteoridonTree());
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = Dust.NewDust(new Vector2(i, j), 5, 5, DustID.AmberBolt);
            return true;
        }
    }
}
