using Terraria;
using Terraria.ModLoader;

namespace TUA.API.TerraEnergy.MachineRecipe.Furnace
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

        public void SetCostAndCookTime(int energyRequired)
        {
            this.energyRequired = energyRequired;
        }

        public int GetCookTime() => energyRequired;

        public void AddIngredient(int itemID, int quantity = 1)
        {
            ingredient = new Item
            {
                type = itemID,
                stack = quantity
            };
            ingrediantName = ingredient.Name;
        }

        public void SetResult(int itemID, int quantity = 1)
        {

            result = new Item();
            result.SetDefaults(itemID);
            result.stack = quantity;
            resultName = result.Name;
        }

        public void AddRecipe() => FurnaceRecipeManager.addRecipe(this);

        internal bool CheckQuantity(int i) => i >= ingredient.stack;

        internal bool CheckItem(Item item) => ingredient.type == item.type;

        internal Item GetResult() => result.Clone();

        internal int GetIngredientStack() => ingredient.stack;
    }
}