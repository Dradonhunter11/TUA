using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TUA.API.Dev;
using TUA.Dimension.MicroBiome;

namespace TUA.Items
{
    class SetNight : ModItem
    {
        // private int timer = 0;

        public override bool Autoload(ref string name) => SteamID64Checker.Instance.VerifyID() && TerrariaUltraApocalypse.devMode;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dev Null'\'");
            Tooltip.SetDefault("Dev item");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 40;
            item.useStyle = 4;
            item.useTime = 2;
            item.useAnimation = 2;
            item.rare = 9;
            item.autoReuse = false;
            item.lavaWet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            //Main.time = 0;
            //Main.dayTime = false;
            //TUAWorld.apocalypseMoon = true;
            //Biomes<StardustFrozenForest>.Place((int)player.Center.X / 16, (int)player.Center.Y / 16, new StructureMap());
            //LiquidRef liquid = LiquidCore.grid[Player.tileTargetX, Player.tileTargetY];
            //Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 240;
            //liquid.setLiquidsState(3, true);

            Biomes<SolarVolcano>.Place((int) player.Center.X / 16, (int) player.Center.Y / 16 - 4, new StructureMap());
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (item.expert)
            {
                item.expert = false;
            }
        }



        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (item.owner == 0)
            {
                item.expert = true;
            }
        }
    }
}
