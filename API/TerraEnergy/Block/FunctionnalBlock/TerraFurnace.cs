using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
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

        public override void RightClick(int i, int j)
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
            if (currentSelectedItem.type == mod.ItemType("TerraMeter"))
            {
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.getEnergy().getCurrentEnergyLevel() + " / " + se.getEnergy().getMaxEnergyLevel() + " TE");
                return;
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking"))
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                it.saveCollectorLocation(se);
                Main.NewText("Terra Furnace succesfully linked, now right click on a capacitor to unlink");
                return;
            }

            TerraFurnaceEntity tfe = (TerraFurnaceEntity)TileEntity.ByID[index];
            tfe.sendEntityToUI();
            FurnaceUI.visible = true;

        }

    }

    class TerraFurnaceEntity : StorageEntity
    {
        private CapacitorEntity boundCapacitor;

        public Item[] inventory = new Item[2];

        public bool UIActive = false;

        private int checkTimer = 20; //Maybe will reduce lag

        private int progression = 0;

        private FurnaceRecipe currentRecipe;

        public override void Load(TagCompound tag)
        {

            maxEnergy = 50000;
            energy = new Core(maxEnergy);

            inventory[0] = tag.Get<Item>("inputSlot");
            inventory[1] = tag.Get<Item>("outputSlot");
            base.Load(tag);
        }

        public void linkToCapacitor(CapacitorEntity capacitor)
        {
            boundCapacitor = capacitor;
        }

        public TerraFurnaceEntity()
        {
            maxEnergy = 50000;
            inventory[0] = null;
            inventory[1] = null;
        }

        public void setItem(Item i)
        {
            inventory[0] = i;
        }

        public void sendEntityToUI()
        {
            TerrariaUltraApocalypse.furnaceUI.receiveFurnaceEntity(this);
        }

        public override TagCompound Save()
        {
            tag = new TagCompound();
            tag.Add("inputSlot", inventory[0]);
            tag.Add("outputSlot", inventory[1]);
            return base.Save();
        }



        public override void Update()
        {
            if (currentRecipe == null && checkTimer <= 0 && inventory[1] == null)
            {
                currentRecipe = getRecipe();
                checkTimer = 20;
            }

            if (currentRecipe != null)
            {
                updateItem();
                progression++;

            }

            if (boundCapacitor != null)
            {
                Main.NewText("true");
                energy.addEnergy(boundCapacitor.energy.consumeEnergy(boundCapacitor.maxTransferRate));
            }
            checkTimer--;

        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            Main.NewText("X " + i + " Y " + j);
            return Place(i - 3, j - 2);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            Main.NewText("here");
            Main.NewText((tile.active() && tile.type == mod.TileType<TerraFurnace>() && tile.frameX == 0 && tile.frameY == 0));
            return tile.active() && (tile.type == mod.TileType<TerraFurnace>()) && tile.frameX == 0 && tile.frameY == 0;
        }



        /*****************************************************************/
        /*                         TIME FOR FUN :D                       */
        /*****************************************************************/

        private FurnaceRecipe getRecipe()
        {
            if (inventory[0] != null && inventory[0].Name != "")
            {
                if (FurnaceRecipeManager.getInstance().validRecipe(inventory[0]))
                {
                    return FurnaceRecipeManager.getInstance().GetRecipe();
                }
            }
            return null;
        }

        private void updateItem()
        {
            if (progression >= currentRecipe.getCookTime())
            {
                inventory[0].stack -= currentRecipe.getIngredientStack();

                inventory[1] = currentRecipe.getResult();
                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
