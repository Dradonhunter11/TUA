using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API
{    
    public class TUAModItem : ModItem
    {
        public virtual bool Ultra { get; set; }
        public virtual bool Furnace { get; set; }
        public override bool CloneNewInstances { get { return true; } }

        public override bool NewPreReforge()
        {
            if (Ultra)  item.rare = -12;
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (Ultra)
            {
                item.rare = -12;
                item.expert = false;
            }
        }

        protected string AddUltraTooltip(string tooltip) {
            return tooltip + "\nUltra";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
            if (Ultra)
            {
                TooltipLine ultraline = new TooltipLine(mod, "IsUltraItem", "Ultra");
                tooltips.Add(ultraline);
            }

            if (item.rare == 99 && tt != null)
            {
                int index = tooltips.IndexOf(tt);
                tooltips[index] = new TooltipLine(mod, "Name", "[c/660000:" + item.Name + "]");
            }
        }

        // TODO: Agrair, this block possibility of custom recipe system
        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            if (CraftingMaterials(out (int item, int stack)[] items))
            {
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    recipe.AddIngredient(item.item, item.stack);
                }
            }
            CraftingConditions(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        protected virtual bool CraftingMaterials(out (int type, int stack)[] items)
        {
            items = new (int, int)[0];
            return false;
        }

        protected virtual void CraftingConditions(ModRecipe recipe)
        {

        }*/
    }
}
