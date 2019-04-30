using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Wasteland
{
    class WastestoneBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation tileTranslation = CreateMapEntryName();
            tileTranslation.SetDefault("Wastestone Brick");
            AddMapEntry(new Color(173, 255, 47));
        }
    }
}
