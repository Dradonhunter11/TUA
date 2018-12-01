using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Items.Misc.Tools
{
    class DoodooStickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doodoo stickaxe");
            Tooltip.SetDefault("Good plan, bad execution.");
            Tooltip.SetDefault("A stick that can mine? What else?");
        }

        public override void SetDefaults()
        {
            item.width = 70;
            item.height = 72;
            item.pick = 30;
            item.maxStack = 1;
            item.value = Item.sellPrice(0, 0, 0, 1);
            item.useAnimation = 1;
            item.useTime = 50;
        }
    }
}
