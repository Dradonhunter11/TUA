using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.TerraEnergy.MachineRecipe.Forge
{
    class ForgeRecipe
    {
        private Mod mod;
        private int cookTime = 10;
        private Item ingredient;
        private Item ingredient2;
        private string ingredientName;
        private string catalyserName;
        private Item result;
        private string resultName;

        public ForgeRecipe(Mod mod)
        {
            this.mod = mod;
        }

        public void addIngredient(int itemID, int quantity = 1)
        {
            ingredient = new Item();
            ingredient.type = itemID;
            ingredient.stack = quantity;
            ingredientName = ingredient.Name;
        }

        public void addCatalyserIngredient(int itemID, int quantity = 1)
        {
            ingredient2 = new Item();
            ingredient2.type = itemID;
            ingredient2.stack = quantity;
            catalyserName = ingredient.Name;
        }

        public void setResult(int itemID, int quantity = 1)
        {

            result = new Item();
            result.SetDefaults(itemID, false);
            result.stack = quantity;
            resultName = result.Name;
        }

        internal bool checkItem(Item[] ingredients)
        {
            string ingredientList = "";
            foreach (Item i in ingredients)
            {
                if (i != null)
                {
                    ingredientList += i.Name + " ";
                }
            }
            return ingredientList.Contains(ingredient.Name) && ingredientList.Contains(ingredient2.Name);
        }

        internal bool checkQuantity(Item[] ingredients)
        {
            bool ingredientFlag = false;
            bool catalyserFlag = false;
            foreach (Item i in ingredients)
            {
                if (i.Name == ingredient.Name && i.stack >= ingredient.stack)
                {
                    ingredientFlag = true;
                }
                if (i.Name == ingredient2.Name && i.stack >= ingredient2.stack)
                {
                    catalyserFlag = true;
                }
            }
            return ingredientFlag && catalyserFlag;
        }

        public void addRecipe()
        {
            ForgeRecipeManager.getInstance().AddRecipe(this);
        }
    }
}
