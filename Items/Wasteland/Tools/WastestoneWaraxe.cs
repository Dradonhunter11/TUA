using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Items.Wasteland.Tools
{
    class WastestoneWaraxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eradiated Waraxe");
            Tooltip.SetDefault("Hold the power of 100 eradiated soul");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 38;
            item.axe = 12;
            item.hammer = 125;
            item.melee = true;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.damage = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.crit = 10;
            item.useTime = 30;
            item.useAnimation = 15;
            item.knockBack = 0.7f;
            item.autoReuse = true;
        }
    }
}
