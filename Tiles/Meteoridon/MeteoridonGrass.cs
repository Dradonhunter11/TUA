using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Tiles.Meteoridon
{
    class MeteoridonGrass : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));
            SetModTree(new MeteoridonTree());
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = Dust.NewDust(new Vector2(i, j), 5, 5, DustID.AmberBolt);
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.Next(2) == 0)
            {
                WorldGen.GrowTree(i - 1, j);
            }
            base.RandomUpdate(i, j);
        }
    }
}
