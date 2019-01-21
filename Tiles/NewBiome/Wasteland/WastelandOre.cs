using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Wasteland
{
    class WastelandOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wastestone");
            AddMapEntry(Color.YellowGreen, name);
            
        }
    }
}
