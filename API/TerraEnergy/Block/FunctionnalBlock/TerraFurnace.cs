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
            TileObjectData.newTile.CoordinateHeights = new int[] {  16 , 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TerraFurnaceEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
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

        public override void Load(TagCompound tag)
        {

            maxEnergy = 50000;
            energy = new Core(maxEnergy);
            base.Load(tag);
        }

        public void linkToCapacitor(CapacitorEntity capacitor) {
            boundCapacitor = capacitor;
        }

        public TerraFurnaceEntity() {
            maxEnergy = 50000;
        }

        public void setItem(Item i) {
            inventory[0] = i;
        }

        public Item sendItem(int i) {
            return inventory[i];
        }

        public void sendEntityToUI() {
            FurnaceUI.receiveFurnaceEntity(this);
        }

        public override void Update()
        {
            if (boundCapacitor != null) {
                Main.NewText("true");
                energy.addEnergy(boundCapacitor.energy.consumeEnergy(boundCapacitor.maxTransferRate));
            }
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
    }
}
