using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API
{
    [Obsolete("From Dimlibs@1.0")]
    class TUADimItem : Dimlibs.API.DimItem
    {
        public virtual bool Ultra { get; set; }

        public override bool Autoload(ref string name)
        {
            if (name == "TUADimItem")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public override bool NewPreReforge()
        {
            item.rare = -12;
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            item.rare = -12;
            item.expert = false;
        }

        protected string AddUltraTooltip(string tooltip)
        {
            return tooltip + "\nUltra";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Ultra)
            {
                TooltipLine ultraline = new TooltipLine(mod, "ultra", "Ultra");
                tooltips.Add(ultraline);
            }
            base.ModifyTooltips(tooltips);
        }
    }
}