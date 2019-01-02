using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Raids.Items
{
    class GuideVoodooDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide voodoo doll");
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
