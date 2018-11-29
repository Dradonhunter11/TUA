using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API
{
    class TUAModLegacyItem : TUAModItem
    {
        public override bool Autoload(ref string name)
        {
            if (name == "TUAModLegacyItem")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine legacy = new TooltipLine(mod, "Legacy", "[c/006400:- Legacy -]");
            tooltips.Add(legacy);
            TooltipLine legacyDesc = new TooltipLine(mod, "LegacyDesc", "[c/006400:This item come from original Terraria apocalypse mod]");
            tooltips.Add(legacyDesc);
        }
    }
}
