using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.CustomInventory;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.API.TerraEnergy.UI;
using TUA.Utilities;

namespace TUA.API.FurnaceRework.TileEntity
{
    abstract class BaseFurnaceEntity : ModTileEntity
    {
        private BudgetCore fuel;
        private FurnaceUI furnaceUi;

        public ExtraSlot InputSlot;
        public ExtraSlot OutputSlot;
        public ExtraSlot FuelSlot;

        private int checkTimer = 20; //Maybe will reduce lag

        private int progression = 0;
        private string furnaceName = "";

        private FurnaceRecipe currentRecipe;

        private int maxFuel;
        protected int cookTimer;

        public abstract void SetValue(ref int maxEnergy, ref int cookTimer, ref string furnaceName);

        public void Activate()
        {
            SetValue(ref maxFuel, ref cookTimer, ref furnaceName);
            if (fuel == null)
            {
                fuel = new BudgetCore(maxFuel);
            }
            if (InputSlot == null)
            {
                InputSlot = new ExtraSlot();
            }
            if (OutputSlot == null)
            {
                OutputSlot = new ExtraSlot();
            }

            if (FuelSlot == null)
            {
                FuelSlot = new ExtraSlot();
            }
            
            if (furnaceUi == null)
            {
                furnaceUi = new FurnaceUI(InputSlot, OutputSlot, FuelSlot, fuel, furnaceName);
            }

            Main.playerInventory = true;
            UIManager.OpenMachineUI(furnaceUi);
        }

        public sealed override void Load(TagCompound tag)
        {
            SetValue(ref maxFuel, ref cookTimer, ref furnaceName);
            InputSlot = new ExtraSlot();
            OutputSlot = new ExtraSlot();

            maxFuel = 50000;
            fuel = new BudgetCore(maxFuel);

            Item temp = tag.Get<Item>("inputSlot");
            Item temp2 = tag.Get<Item>("outputSlot");

            SetAir(ref temp);
            SetAir(ref temp2);

            InputSlot.SetItem(ref temp);
            OutputSlot.SetItem(ref temp2);
        }

        public void SetAir(ref Item item)
        {
            if (item.Name == "Unloaded Item")
            {
                item.TurnToAir();
            }
        }

        public BaseFurnaceEntity()
        {
            SetValue(ref maxFuel, ref cookTimer, ref furnaceName);
            InputSlot = new ExtraSlot();
            OutputSlot = new ExtraSlot();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["inputSlot"] = InputSlot.GetItem(),
                ["outputSlot"] = OutputSlot.GetItem(),
                ["fuelLevel"] = fuel.getCurrentEnergyLevel()
            };
        }

        public override void Update()
        {
            if (fuel == null)
            {
                fuel = new BudgetCore(maxFuel);
            }

            if (currentRecipe == null && checkTimer <= 0)
            {
                FurnaceRecipe recipe = InputSlot.IsEmpty ? null
                    : FurnaceRecipeManager.getInstance().validRecipe(InputSlot.GetItem()) 
                        ? FurnaceRecipeManager.getInstance().GetRecipe() 
                        : null;
                if (recipe != null &&
                    (OutputSlot.IsEmpty || OutputSlot.GetItem().type == recipe.GetResult().type))
                {
                    currentRecipe = recipe;
                }

                checkTimer = 5;
            }

            if (currentRecipe != null)
            {
                UpdateItem();
                progression++;
            }
            checkTimer--;

        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            furnaceUi = new FurnaceUI(InputSlot, OutputSlot, fuel, furnaceName);
            return Place(i - 1, j - 1);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && (tile.type == TileID.Furnaces || tile.type == TileID.Hellforge || tile.type == TileID.AdamantiteForge || (tile.type == TileID.AdamantiteForge && tile.frameX >= 54));
        }



        /*****************************************************************/
        /*                         TIME FOR FUN :D                       */
        /*****************************************************************/

        private void UpdateItem()
        {
            if (progression >= cookTimer)
            {
                InputSlot.ManipulateCurrentStack(currentRecipe.GetIngredientStack());

                Item result = currentRecipe.GetResult().Clone();

                if (OutputSlot.IsEmpty)
                {
                    OutputSlot.SetItem(ref result);
                }
                else
                {
                    OutputSlot.ManipulateCurrentStack(1);
                }

                fuel.consumeEnergy(1);
                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
