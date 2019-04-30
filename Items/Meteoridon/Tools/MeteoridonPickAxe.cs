using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Items.Meteoridon.Tools
{
    class MeteoridonPickAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteoride PickAxe");
            Tooltip.SetDefault("Tool of meteoride knowledge");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 48;
            item.pick = 201;
            item.axe = 20;
            item.knockBack = 4.5f;
            item.damage = 37;
            item.useTime = 24;
            item.useAnimation = 30;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.melee = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MeteoridonBar"), 20);
            recipe.AddIngredient(mod.GetItem("MeteorideScale"), 20);
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
