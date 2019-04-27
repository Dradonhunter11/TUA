using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TUA.API.Dev;
using TUA.Dimension.MicroBiome;
using Terraria.Cinematics;
using TUA.Movie.Boss;
using TUA.Structure.DungeonLike;

namespace TUA.Items
{
    class SetNight : ModItem
    {

        public override bool Autoload(ref string name) => SteamID64Checker.Instance.VerifyDevID() && TUA.devMode;

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
            item.useTime = 5;
            item.useAnimation = 5;
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
            /*for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].liquid = 0;
                }
            }*/

            TUAPlayer.initialPoint = Main.MouseWorld / 16;

            //CinematicManager.Instance.PlayFilm(new UEoCCutscene(new Vector2(player.position.X + 7000, player.position.Y + 6500)));
            //Main.time = 0;
            //Main.dayTime = false;
            //TUAWorld.apocalypseMoon = true;
            //Biomes<StardustFrozenForest>.Place((int)player.Center.X / 16, (int)player.Center.Y / 16, new StructureMap());
            //Main.PlaySound(19, (int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y, 1, 1f, 0f);
            //LiquidRef liquid = LiquidCore.grid[Player.tileTargetX, Player.tileTargetY];
            //Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
            //liquid.SetLiquidsState(3, true);
            //WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
            //TUA.instance.SetTitle("Hello world", "Yup an hello world message as title", Color.Green, Color.Pink, Main.fontDeathText, 30, 1f, true);
            //Biomes<SolarVolcano>.Place((int) player.Center.X / 16, (int) player.Center.Y / 16 - 4, new StructureMap());
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            TUAPlayer.endPoint = Main.MouseWorld / 16;
            Biomes<SolarVolcano>.Place((int)player.Center.X / 16, (int)player.Center.Y / 16 - 4, new StructureMap());
            //SolarDungeon.PlaceALine(TUAPlayer.initialPoint, TUAPlayer.endPoint);
            //SolarDungeon.PlaceRoomBox((int)(TUAPlayer.endPoint.X - 75 / 2), (int)(TUAPlayer.endPoint.Y - 15), 75, 30, 5);
            /*SolarDungeon.GenerateSpike(TUAPlayer.initialPoint);
            TUAPlayer.initialPoint = TUAPlayer.endPoint;
            Main.NewText("Line placed");*/
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

