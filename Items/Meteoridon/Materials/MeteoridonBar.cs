using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Items.Meteoridon.Materials
{
    class MeteoridonBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteoride Bar");
            Tooltip.SetDefault("SPACE!");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.LightRed;
        }
    }
}
