using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.TerraEnergy.MachineRecipe.Furnace
{
    class FurnaceRecipe : BaseRecipe
    {
        private int energyRequired = 10;
        
        private Item ingredient;
        private string ingrediantName;
        private Item result;
        private string resultName;

        public FurnaceRecipe(Mod mod)
        {

        }

        public void setCostAndCookTime(int energyRequired)
        {
            this.energyRequired = energyRequired;
        }

        public int getCookTime()
        {
            return energyRequired;
        }

        public void addIngredient(int itemID, int quantity = 1)
        {
            ingredient = new Item();
            ingredient.type = itemID;
            ingredient.stack = quantity;
            ingrediantName = ingredient.Name;
        }

        public void setResult(int itemID, int quantity = 1)
        {

            result = new Item();
            result.SetDefaults(itemID, false);
            result.stack = quantity;
            resultName = result.Name;
        }

        public void addRecipe()
        {
            FurnaceRecipeManager.addRecipe(this);
        }



        internal bool checkQuantity(int i)
        {
            if (i >= ingredient.stack)
            {
                return true;
            }
            return false;
        }

        internal bool checkItem(Item item)
        {
            if (ingredient.type == item.type)
            {
                return true;
            }
            return false;
        }

        internal Item getResult()
        {
            Item item = result.Clone();
            return item;
        }

        internal int getIngredientStack()
        {
            return ingredient.stack;
        }
    }
}