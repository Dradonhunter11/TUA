using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using TUA.API;
using TUA.NPCs.Gods.EoA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using TUA.API.EventManager;

namespace TUA.Items.EoA
{
    class Spawner : TUAModLegacyItem
    {
        public override bool CloneNewInstances => false;
        

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apoclypsio");
            Tooltip.SetDefault("Look in the sky! The moon have changed...\nSummon the god of destruction");
            DisplayName.AddTranslation(GameCulture.French, "Apoclypsio");
            Tooltip.AddTranslation(GameCulture.French, "Invoque le premier des anciens dieux\nUne fois vaincu, votre monde sera en mode Ultra");
        }

        public override void SetDefaults()
        {
            
            item.width = 20;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 4;
            item.rare = 2;
            item.maxStack = 20;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime || TUAWorld.ApoMoonDowned;
        }

        public override bool UseItem(Player player)
        {
            //if (downedMoonlord && TUA.EoCDeath >= 10)
            //{
            if (!Main.expertMode)
            {
                Main.expertMode = true;
            }

            if (TUAWorld.ApoMoonDowned)
            {
                Main.PlaySound(SoundID.Roar, player.position, 0);
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (76 * 16), mod.NPCType<EyeOApocalypse>());
            }
            else
            {
                MoonEventManagerWorld.Activate("Apocalypse Moon");
            }

            if (Main.netMode != 1)
            {
                Main.NewText("The apocalypse is coming, be aware...", Color.DarkGoldenrod);
            }
            else
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The apocalypse is coming, be aware..."), Color.DarkGoldenrod);
            }
            Main.PlaySound(SoundID.MoonLord, player.position, 0);
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (76*16), mod.NPCType<EyeOApocalypse>());
            Main.spriteBatch.Draw(ModContent.GetTexture("Projectile_490"), new Vector2(player.Center.X, player.Center.Y), Color.DarkRed);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 5);
            recipe.AddIngredient(ItemID.MechanicalEye, 1);
            recipe.AddIngredient(null, "SuspiciousBurnedEye", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
