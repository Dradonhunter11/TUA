using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TerrariaUltraApocalypse.API.CustomInventory;
using TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI;
using TerrariaUltraApocalypse.API.TerraEnergy.Items;
using TerrariaUltraApocalypse.API.TerraEnergy.MachineRecipe.Furnace;
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;
using TerrariaUltraApocalypse.API.TerraEnergy.UI;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock
{
    class TerraFurnace : TUABlock
    {
        public override void SetDefaults()
        {

            //TileObjectData.newTile.CoordinateHeights = new int[] { 60, 60, 42 };
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Origin = new Point16(3, 2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TerraFurnaceEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void HitWire(int i, int j)
        {

        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];


            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = mod.GetTileEntity<TerraFurnaceEntity>().Find(left, top);

            Main.NewText("X " + i + " Y " + j);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }

            StorageEntity se = (StorageEntity)TileEntity.ByID[index];
            if (currentSelectedItem.type == mod.ItemType("TerraMeter"))
            {
                se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.getEnergy().getCurrentEnergyLevel() + " / " + se.getEnergy().getMaxEnergyLevel() + " TE");
                return;
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking"))
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                se = (StorageEntity)TileEntity.ByID[index];
                it.saveCollectorLocation(se);
                Main.NewText("Terra Furnace succesfully linked, now right click on a capacitor to unlink");
                return;
            }

            TerraFurnaceEntity tfe = (TerraFurnaceEntity)TileEntity.ByID[index];
            tfe.Activate();


        }

    }

    class TerraFurnaceEntity : StorageEntity
    {
        private FurnaceUI furnaceUi;

        private CapacitorEntity boundCapacitor;

        public ExtraSlot InputSlot;
        public ExtraSlot OutputSlot;

        private int checkTimer = 20; //Maybe will reduce lag

        private int progression = 0;

        private FurnaceRecipe currentRecipe;

        public void Activate()
        {
            if (InputSlot == null)
            {
                InputSlot = new ExtraSlot();
            }
            if (OutputSlot == null)
            {
                OutputSlot = new ExtraSlot();
            }

            if (energy == null)
            {
                energy = new Core(50000);
            }
            if (furnaceUi == null)
            {
                furnaceUi = new FurnaceUI(InputSlot, OutputSlot, energy, "Terra Furnace");
            }

            Main.playerInventory = true;
            TerrariaUltraApocalypse.machineInterface.SetState(furnaceUi);
            TerrariaUltraApocalypse.machineInterface.IsVisible = true;
        }

        public override void LoadEntity(TagCompound tag)
        {
            InputSlot = new ExtraSlot();
            OutputSlot = new ExtraSlot();

            maxEnergy = 50000;
            energy = new Core(maxEnergy);

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

        public void linkToCapacitor(CapacitorEntity capacitor)
        {
            boundCapacitor = capacitor;
        }

        public TerraFurnaceEntity()
        {
            maxEnergy = 50000;
            InputSlot = new ExtraSlot();
            OutputSlot = new ExtraSlot();
        }

        public void setItem(Item i)
        {
            InputSlot.setItem(ref i);
        }



        public override void SaveEntity(TagCompound tag)
        {
            tag.Add("inputSlot", InputSlot.getItem(true));
            tag.Add("outputSlot", OutputSlot.getItem(true));
        }

        public override void Update()
        {
            if (energy == null)
            {
                energy = new Core(maxEnergy);
            }



            if (currentRecipe == null && checkTimer <= 0)
            {
                FurnaceRecipe recipe = getRecipe();
                if (recipe != null &&
                    (OutputSlot.isEmpty() || OutputSlot.getItem(false).type == recipe.getResult().type))
                {
                    currentRecipe = recipe;
                }

                checkTimer = 20;
            }

            if (currentRecipe != null)
            {
                updateItem();
                progression++;

            }

            if (boundCapacitor != null)
            {
                energy.addEnergy(boundCapacitor.energy.consumeEnergy(boundCapacitor.maxTransferRate));
            }
            checkTimer--;

        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            furnaceUi = new FurnaceUI(InputSlot, OutputSlot, energy, "Terra Furnace");
            return Place(i - 3, j - 2);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            Main.NewText((tile.active() && tile.type == mod.TileType<TerraFurnace>() && tile.frameX == 0 && tile.frameY == 0));
            return tile.active() && (tile.type == mod.TileType<TerraFurnace>()) && tile.frameX == 0 && tile.frameY == 0;
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
            if (progression >= currentRecipe.getCookTime() && energy.consumeEnergy(50) == 50)
            {
                InputSlot.manipulateCurrentStack(-currentRecipe.getIngredientStack());

                Item result = currentRecipe.getResult().Clone();

                if (OutputSlot.isEmpty())
                {
                    OutputSlot.setItem(ref result);
                }
                else
                {
                    OutputSlot.manipulateCurrentStack(1);
                }

                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
