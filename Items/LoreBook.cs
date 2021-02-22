using Terraria;
using Terraria.ModLoader;
using TUA.Utilities;

namespace TUA.Items
{
    class LoreBook : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The lore book");
            Tooltip.SetDefault("Serves to guide you during your adventure in the wild lands of Terraria.");
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
            if (Main.myPlayer == player.whoAmI)
            {
                UIManager.OpenLoreUI(player);
                return false;
            }
            return true;
        }
    }
}
