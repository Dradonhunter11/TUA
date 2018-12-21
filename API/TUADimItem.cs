using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API
{
    class TUADimItem : Dimlibs.API.DimItem
    {
        public virtual bool ultra { get; set; }

        public override bool Autoload(ref string name)
        {
            if (name == "TUADimItem")
            {
                return false;
            }
            return base.Autoload(ref name);
        }


        protected void manipulateUltraProperty(bool isExpert)
        {
            item.rare = -12;
            if (isExpert)
            {
                item.expert = false;
                return;
            }
            else
            {
                item.expert = true;
                return;
            }
        }

        public override bool NewPreReforge()
        {
            item.rare = -12;
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            manipulateUltraProperty(true);
        }

        protected string AddUltraTooltip(string tooltip)
        {
            return tooltip + "\nUltra";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (ultra)
            {
                TooltipLine ultraline = new TooltipLine(mod, "ultra", "Ultra");
                tooltips.Add(ultraline);
            }
            base.ModifyTooltips(tooltips);
        }
    }
}