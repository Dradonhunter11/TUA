using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using TerrariaUltraApocalypse.API.Injection;
using TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod;
using TerrariaUltraApocalypse.API.LiquidAPI.Swap;
using TerrariaUltraApocalypse.Dimension.MicroBiome;

namespace TerrariaUltraApocalypse.Items
{
    class SetNight : ModItem
    {
        private int timer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Set Night");
            Tooltip.SetDefault("Set the time to night, just because you can\nUltra mode");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 40;
            item.useStyle = 4;
            item.useTime = 20;
            item.useAnimation = 20;
            item.rare = 9;
            item.lavaWet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.Grass);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            //Main.time = 0;
            //Main.dayTime = false;
            //TUAWorld.apocalypseMoon = true;
            //Biomes<StardustFrozenForest>.Place((int)player.Center.X / 16, (int)player.Center.Y / 16, new StructureMap());
            LiquidRef liquid = LiquidCore.grid[Player.tileTargetX, Player.tileTargetY];
            Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 240;
            liquid.setLiquidsState(3, true);
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
