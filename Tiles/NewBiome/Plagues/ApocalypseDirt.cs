using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;


namespace TUA.Tiles.NewBiome.Plagues
{
    class ApocalypseDirt : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Microsoft.Xna.Framework.Color(0, 0, 0));
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.item[Main.lastItemUpdate].position.X == i && Main.item[Main.lastItemUpdate].position.Y == j + 1)
            {
                Main.NewText(Main.item[Main.lastItemUpdate].Name);
            }
        }
    }
}
