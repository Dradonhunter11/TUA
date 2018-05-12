using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse.API
{
    
    class TUAModItem : ModItem
    {
        public virtual bool ultra { get; set; }
        public virtual bool furnace { get; set; }

        protected void manipulateUltraProperty(bool isExpert) {
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

        protected String addUltraTooltip(String tooltip) {
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

        public override void AddRecipes()
        {
            if (furnace) {

            }
        }
    }
}
