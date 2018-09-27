using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Items.Materials.Meteoridon
{
    class MeteorideScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteoride Scale");
            Tooltip.SetDefault("A scale coming from mysterious space creature");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 18;
            item.rare = 9;
            item.lavaWet = true;
            item.maxStack = 999;
        }
    }
}
