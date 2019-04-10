using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TUA.LoreBook.UI;

namespace TUA.LoreBook.Items
{
    class LoreBook : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The lore book");
            Tooltip.SetDefault("Should guide you during your adventure in the terraria land.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 36;
            item.useTime = 1;
            item.useAnimation = 30;
            item.consumable = false;
            item.useStyle = 4;
            item.maxStack = 1;
            item.rare = -12;
        }

        public override bool UseItem(Player player)
        {
            TUA.loreInterface.SetState(new LoreUI(Main.LocalPlayer.GetModPlayer<LorePlayer>()));
            TUA.loreInterface.IsVisible = true;
            return false;
        }
    }
}
