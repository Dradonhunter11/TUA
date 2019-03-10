using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA
{
    class RecipeUtils
    {
        private static List<Recipe> removedRecipes = new List<Recipe>();

        public static void removeRecipe(int itemID)
        {
            RecipeFinder rf = new RecipeFinder();
            rf.SetResult(itemID);

            List<Recipe> list = rf.SearchRecipes();
            for (int i = 0; i < list.Count; i++)
            {
                Recipe r = list[i];
                RecipeEditor re = new RecipeEditor(r);
                re.DeleteRecipe();
            }

        }
        public static void AddRecipe(Mod mod, List<Tuple<int, int>> ingredient, Tuple<int, int> result)
        {
            ModRecipe r = new ModRecipe(mod);
            for (int i = 0; i < ingredient.Count; i++)
            {
                Tuple<int, int> tuple = ingredient[i];
                r.AddIngredient(tuple.Item1, tuple.Item2);
            }
            r.SetResult(result.Item1, result.Item2);
            r.AddRecipe();
        }

        public static void GetAllRecipeByIngredientAndReplace(int ingredientToReplace, int replacingIngredient)
        {
            RecipeFinder rf = new RecipeFinder();
            rf.AddIngredient(ingredientToReplace);

            List<Recipe> list = rf.SearchRecipes();
            for (int i = 0; i < list.Count; i++)
            {
                Recipe r = list[i];
                Recipe recipe = r;
                RecipeEditor re = new RecipeEditor(recipe);
                
                if (re.DeleteIngredient(ingredientToReplace))
                {
                    re.AddIngredient(replacingIngredient);
                    Main.recipe[Recipe.numRecipes] = r;
                    Recipe.numRecipes++;
                }
            }
        }


        public static void setAllFurnaceRecipeSystem()
        {
            RecipeFinder rf = new RecipeFinder();
            rf.AddTile(TileID.Furnaces);

            List<Recipe> list = rf.SearchRecipes();
            for (int i = 0; i < list.Count; i++)
            {
                Recipe r = list[i];
                Recipe recipe = r;
                if (recipe.requiredItem.Length == 1)
                {
                    TUA.instance.addFurnaceRecipe(recipe.requiredItem[0].type, recipe.createItem.type, 20);
                    removedRecipes.Add(r);
                    RecipeEditor re = new RecipeEditor(r);
                    re.DeleteRecipe();
                }
            }
        }

        /*
        public static void SetBackAllRecipe()
        {
            for (int i = 0; i < removedRecipes.Count; i++)
            {
                
            }
        }
        */
    }
}
