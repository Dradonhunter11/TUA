using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using TUA.API;

namespace TUA.Items.EoA
{
    class SuspiciousBurnedEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Suspicous Burned Eye");
            Tooltip.AddLine("Summon the eye of cthulhu");
            Tooltip.AddLine("Maybe this is not a good idea...");
            DisplayName.AddTranslation(GameCulture.French, "Oeil brulé suspicieux");
            Tooltip.AddTranslation(GameCulture.French, "Invoque l'oeil de Cthulhu");
            Tooltip.AddLine("Peut-être que ce n'est pas une bonne idée...", GameCulture.French.LegacyId);
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 0;
            item.consumable = false;
            item.maxStack = 20;
        }

        public override bool UseItem(Player player)
        {
            if (NPC.downedMoonlord && !TUAWorld.EoCPostMLDowned && !Main.dayTime)
            {
                NPC.SpawnOnPlayer(player.whoAmI, NPCID.EyeofCthulhu);
                return true;
            }
            else
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("UltraEoC"));
                return true;
            }
            return false;
        }
    }
}
