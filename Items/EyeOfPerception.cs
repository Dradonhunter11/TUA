using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TUA.API;
using TUA.API.EventManager;
using TUA.NPCs.Gods.EoA;

namespace TUA.Items
{
    public class EyeOfPerception : TUAModItem
    {
        public override bool CloneNewInstances => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of perception");
            Tooltip.SetDefault("To the one who use this, destruction may come");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de la perception");
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
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (76 * 16), ModContent.NPCType<EyeOApocalypse>());
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
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - (76*16), ModContent.NPCType<EyeOApocalypse>());
            Main.spriteBatch.Draw(ModContent.GetTexture("Projectile_490"), new Vector2(player.Center.X, player.Center.Y), Color.DarkRed);
            return true;
        }
    }
}
