using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.CustomInventory;
using TUA.API.TerraEnergy;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.API.TerraEnergy.TileEntities;
using TUA.API.TerraEnergy.UI;

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
            TUA.machineInterface.SetState(furnaceUi);
            TUA.machineInterface.IsVisible = true;
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

            InputSlot.setItem(ref temp);
            OutputSlot.setItem(ref temp2);
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

        public void setItem(Item i)
        {
            InputSlot.setItem(ref i);
        }


        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("inputSlot", InputSlot.getItem(true));
            tag.Add("outputSlot", OutputSlot.getItem(true));
            tag.Add("fuelLevel", fuel.getCurrentEnergyLevel());
            return tag;
        }

        public override void Update()
        {
            if (fuel == null)
            {
                fuel = new BudgetCore(maxFuel);
            }

            if (currentRecipe == null && checkTimer <= 0)
            {
                FurnaceRecipe recipe = getRecipe();
                if (recipe != null &&
                    (OutputSlot.isEmpty() || OutputSlot.getItem(false).type == recipe.getResult().type))
                {
                    currentRecipe = recipe;
                }

                checkTimer = 5;
            }

            if (currentRecipe != null)
            {
                updateItem();
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

        private FurnaceRecipe getRecipe()
        {
            if (!InputSlot.isEmpty())
            {
                if (FurnaceRecipeManager.getInstance().validRecipe(InputSlot.getItem(true)))
                {
                    return FurnaceRecipeManager.getInstance().GetRecipe();
                }
            }
            return null;
        }

        private void updateItem()
        {
            if (progression >= cookTimer)
            {
                InputSlot.manipulateCurrentStack(currentRecipe.getIngredientStack());

                Item result = currentRecipe.getResult().Clone();

                if (OutputSlot.isEmpty())
                {
                    OutputSlot.setItem(ref result);
                }
                else
                {
                    OutputSlot.manipulateCurrentStack(1);
                }

                fuel.consumeEnergy(1);
                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
