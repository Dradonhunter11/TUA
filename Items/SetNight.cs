using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TUA.API.Dev;
using TUA.API.LiquidAPI.LiquidMod;
using TUA.Dimension.MicroBiome;

namespace TUA.Items
{
    class SetNight : ModItem
    {
        // private int timer = 0;

        public override bool Autoload(ref string name) => SteamID64Checker.Instance.VerifyDevID() && TerrariaUltraApocalypse.devMode;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dev Null\\");
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
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].liquid = 0;
                }
            }
            //Main.time = 0;
            //Main.dayTime = false;
            //TUAWorld.apocalypseMoon = true;
            //Biomes<StardustFrozenForest>.Place((int)player.Center.X / 16, (int)player.Center.Y / 16, new StructureMap());
            Main.PlaySound(19, (int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y, 1, 1f, 0f);
            LiquidRef liquid = LiquidCore.grid[Player.tileTargetX, Player.tileTargetY];
            Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
            liquid.SetLiquidsState(3, true);
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
            TerrariaUltraApocalypse.instance.SetTitle("Hello world", "Yup an hello world message as title", Color.Green, Color.Pink, Main.fontDeathText, 30, 1f, true);
            //Biomes<SolarVolcano>.Place((int) player.Center.X / 16, (int) player.Center.Y / 16 - 4, new StructureMap());
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
