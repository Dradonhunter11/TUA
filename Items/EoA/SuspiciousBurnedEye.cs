using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrariaUltraApocalypse.Items.EoA
{
    class SuspiciousBurnedEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Suspicous Burned Eye");
            Tooltip.SetDefault("Summon the eye of cthulhu\nMaybe this is not a good idea...");
            DisplayName.AddTranslation(GameCulture.French, "Oeil brulé suspicieux");
            Tooltip.AddTranslation(GameCulture.French, "Était une partie d'un dieux, maintenant brulé...");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 0;
            item.consumable = false;
            item.stack = 20;
        }

        public override bool UseItem(Player player)
        {
            if (NPC.downedMoonlord) {
                NPC.SpawnOnPlayer(player.whoAmI, NPCID.EyeofCthulhu);
                return true;
            }
            return false;
        }
    }
}
