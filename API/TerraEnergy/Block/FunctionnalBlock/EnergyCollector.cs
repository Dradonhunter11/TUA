using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TerrariaUltraApocalypse.API.TerraEnergy.Items;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock
{
    class EnergyCollector : TUABlock
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<EnergyCollectorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }

        public override void RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = mod.GetTileEntity<EnergyCollectorEntity>().Find(left, top);

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
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking")) {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                it.saveCollectorLocation(se);
                Main.NewText("Succesfully linked to a collector, now right click on a capacitor to unlink");
            }
        }
    }

    class EnergyCollectorEntity : StorageEntity
    {
        private int drainRange = 50;
        private int maxEnergy = 100000;
        private CapacitorEntity boundCapacitor;

        public override void Load(TagCompound tag)
        {
            maxEnergy = 100000;
            energy = new Core(maxEnergy);
            base.Load(tag);
        }

        public void linkToCapacitor(CapacitorEntity capacitor)
        {
            boundCapacitor = capacitor;
        }

        public override void Update()
        {
            int i = Position.X + Main.rand.Next(-drainRange, drainRange);
            int j = Position.Y + Main.rand.Next(-drainRange, drainRange);

            Tile tile = Main.tile[i, j];

            if (energy == null)
            {
                energy = new Core(maxEnergy);
            }

            if (boundCapacitor != null) {
                boundCapacitor.energy.addEnergy(energy.consumeEnergy(boundCapacitor.maxTransferRate));
            }

            if (tile != null && tile.type != mod.TileType("EnergyCollector") || tile.type != mod.TileType("BasicTECapacitor") || tile.type != mod.TileType("TerraWaste") || tile.type != mod.TileType("TerraFurnace") && !energy.isFull()) {
                if (Main.tile[i, j].type == TileID.LunarOre) {
                    energy.addEnergy(50);
                    Main.tile[i, j].type = (ushort)mod.TileType("TerraWaste");
                }

                if (Main.tile[i, j].active() && !(Main.tile[i, j].type == (ushort)mod.TileType("TerraWaste"))) {
                    
                    energy.addEnergy(5);
                    if (Main.rand.Next(0, 1000) == 5) {
                        Main.tile[i, j].type = (ushort)mod.TileType("TerraWaste");
                    }
                } 
            }
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<EnergyCollector>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            energy = new Core(maxEnergy);
            Main.NewText("X " + i + " Y " + j);

            return Place(i - 1, j - 2);
        }
    }
}