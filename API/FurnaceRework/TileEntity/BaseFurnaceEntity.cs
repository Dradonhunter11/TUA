using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Inventory;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.API.TerraEnergy.UI;
using TUA.Utilities;

namespace TUA.API.FurnaceRework.TileEntity
{
    abstract class BaseFurnaceEntity : ModTileEntity
    {
        private FuelCore fuel;

        private Ref<Item> _inputItem;
        private Ref<Item> _outputItem;
        private Ref<Item> _fuelItem;

        public Item inputItem => _inputItem.Value;
        public Item outputItem => _outputItem.Value;
        public Item fuelItem => _fuelItem.Value;

        private int checkTimer = 20; //Maybe will reduce lag

        private int progression = 0;
        private string furnaceName = "";

        private FurnaceRecipe currentRecipe;

        private int maxFuel;
        protected int cookTimer;

        public abstract void SetValue(ref int maxEnergy, ref int cookTimer, ref string furnaceName);

        public BaseFurnaceEntity()
        {
            SetValue(ref maxFuel, ref cookTimer, ref furnaceName);
            _inputItem = new Ref<Item>(new Item());
            _outputItem = new Ref<Item>(new Item());
            _fuelItem = new Ref<Item>(new Item());
            inputItem.TurnToAir();
            outputItem.TurnToAir();
            fuelItem.TurnToAir();

            fuel = new FuelCore(maxFuel);
        }
        
        public void Activate()
        {
            Main.playerInventory = true;
            UIManager.OpenMachineUI(new FurnaceUI(_inputItem, _outputItem, _fuelItem, fuel, furnaceName));
        }

        public sealed override void Load(TagCompound tag)
        {
            SetValue(ref maxFuel, ref cookTimer, ref furnaceName);

            maxFuel = 50000;
            fuel = new FuelCore(maxFuel);

            Item temporaryInputItem = tag.Get<Item>("inputSlot");
            Item temporaryOutputItem = tag.Get<Item>("outputSlot");
            Item temporaryFuelItem = tag.Get<Item>("fuelSlot");

            SetAir(ref temporaryInputItem);
            SetAir(ref temporaryOutputItem);
            SetAir(ref temporaryFuelItem);
            
            _inputItem.Value = temporaryInputItem;
            _outputItem.Value = temporaryOutputItem;
            _fuelItem.Value = temporaryFuelItem;
        }

        public void SetAir(ref Item item)
        {
            if (item.Name == "Unloaded Item")
            {
                item.TurnToAir();
            }
        }
        
        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["inputSlot"] = inputItem,
                ["outputSlot"] = outputItem,
                ["fuelSlot"] = fuelItem,
                ["fuelLevel"] = fuel.getCurrentEnergyLevel()
            };
        }

        public override void Update()
        {
            if (fuel == null)
            {
                fuel = new FuelCore(maxFuel);
            }

            if (currentRecipe == null && checkTimer <= 0)
            {
                FurnaceRecipe recipe = inputItem.IsAir ? null
                    : FurnaceRecipeManager.Instance.IsValid(inputItem) 
                        ? FurnaceRecipeManager.Instance.Recipe : null;
                if (recipe != null &&
                    (outputItem.IsAir || outputItem.type == recipe.GetResult().type))
                {
                    currentRecipe = recipe;
                }

                checkTimer = 2;
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
            if (progression >= cookTimer && fuel.getCurrentEnergyLevel() > 0)
            {
                inputItem.stack -= currentRecipe.GetIngredientStack();

                Item result = currentRecipe.GetResult().Clone();

                if (outputItem.IsAir)
                {
                    _outputItem.Value = result;
                }
                else
                {
                    outputItem.stack++;
                }

                fuel.ConsumeEnergy(1);
                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
