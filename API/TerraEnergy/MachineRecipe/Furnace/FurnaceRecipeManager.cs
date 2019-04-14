using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.TerraEnergy.MachineRecipe.Furnace
{
    class FurnaceRecipeManager
    {
        private static readonly List<FurnaceRecipe> furnaceRecipeList = new List<FurnaceRecipe>();
        private static readonly Lazy<FurnaceRecipeManager> instance = new Lazy<FurnaceRecipeManager>();

        public static FurnaceRecipeManager Instance => instance.Value;

        public static FurnaceRecipe CreateRecipe(Mod mod)
        {
            return new FurnaceRecipe(mod); ;
        }

        public static void AddRecipe(FurnaceRecipe recipe)
        {
            furnaceRecipeList.Add(recipe);
        }

        public bool IsValid(Item ingredient)
        {
            for (int k = 0; k < furnaceRecipeList.Count; k++)
            {
                FurnaceRecipe i = furnaceRecipeList[k];
                if (i.CheckItem(ingredient) && i.CheckQuantity(ingredient.stack))
                {
                    Recipe = i;
                    return true;
                }
            }
            return false;
        }

        public FurnaceRecipe Recipe { get; private set; }
    }
}
