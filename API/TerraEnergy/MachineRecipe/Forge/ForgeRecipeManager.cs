using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;

namespace TUA.API.TerraEnergy.MachineRecipe.Forge
{
    class ForgeRecipeManager
    {
        private static List<ForgeRecipe> forgeRecipeList = new List<ForgeRecipe>();
        private static ForgeRecipeManager instance;
        private ForgeRecipe currentRecipe;

        public static ForgeRecipeManager getInstance()
        {
            if (instance == null)
            {
                instance = new ForgeRecipeManager();
            }

            return instance;
        }

        private ForgeRecipeManager()
        {

        }

        public static ForgeRecipe CreateRecipe(Mod mod)
        {
            ForgeRecipe newRecipe = new ForgeRecipe(mod);
            return newRecipe;
        }

        public bool validRecipe(Item[] ingredient)
        {
            for (int i1 = 0; i1 < forgeRecipeList.Count; i1++)
            {
                ForgeRecipe i = forgeRecipeList[i1];
                if (i.checkItem(ingredient) && i.checkQuantity(ingredient))
                {
                    currentRecipe = i;
                    return true;
                }
            }
            return false;
        }

        

        public void AddRecipe(ForgeRecipe r)
        {
            forgeRecipeList.Add(r);
        }

        public ForgeRecipe GetRecipe()
        {
            return currentRecipe;
        }
    }
}
