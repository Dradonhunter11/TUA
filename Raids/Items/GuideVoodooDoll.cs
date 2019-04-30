using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Raids.Items
{
    class GuideVoodooDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide Voodoo Doll");
            Tooltip.AddLine("Bring it to the guide");
            Tooltip.AddLine("Used to start the great hell ride raids!");
            DisplayName.AddTranslation(GameCulture.French, "Poupé voodoo du guide");
            Tooltip.AddLine("Amener ceci au guide", GameCulture.French.LegacyId);
            Tooltip.AddLine("Permet de commencer la grande descente au enfer!", GameCulture.French.LegacyId);
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.GuideVoodooDoll);
        }
    }
}
