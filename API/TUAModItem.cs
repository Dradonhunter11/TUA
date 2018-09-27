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
        public override bool CloneNewInstances { get { return true; } }

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
            if (ultra)
            {
                item.rare = -12;
            }
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (ultra)
            {
                manipulateUltraProperty(true);
            }
        }

        protected String AddUltraTooltip(String tooltip) {
            return tooltip + "\nUltra";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
            if (ultra)
            {
                TooltipLine ultraline = new TooltipLine(mod, "ultra", "Ultra");
                tooltips.Add(ultraline);
            }

            if (item.rare == 99 && tt != null)
            {
                int index = tooltips.IndexOf(tt);
                tooltips[index] = new TooltipLine(mod, "Name", "[c/660000:" + item.Name + "]");
            }
        }

        public override void AddRecipes()
        {
            if (furnace) {

            }
        }
    }
}
