using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock;
using TerrariaUltraApocalypse.API.TerraEnergy.Items;
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Block
{
    class Capacitor : TUABlock
    {
        public int maxEnergyStorage;

        public ModTileEntity GetCapacitorEntity()
        {
            return mod.GetTileEntity<CapacitorEntity>();
            
        }

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);
        }

        public override void RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            Main.NewText("X " + i + " Y " + j);

            int index = mod.GetTileEntity<CapacitorEntity>().Find(left, top);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }
            if (currentSelectedItem.type == mod.ItemType("TerraMeter"))
            {
                
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.getEnergy().getCurrentEnergyLevel() + " / " + se.getEnergy().getMaxEnergyLevel() + " TE in this Capacitor");
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking"))
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                StorageEntity se = it.getEntity();

                if (se == null) {
                    Main.NewText("The rod of linking is vound to nothing");
                    return;
                }

                CapacitorEntity ce = (CapacitorEntity)TileEntity.ByID[index];

                if (se.type == mod.TileEntityType("EnergyCollectorEntity")) {
                    EnergyCollectorEntity ece = se as EnergyCollectorEntity;
                    ece.linkToCapacitor(ce);
                    
                } else if (se.type == mod.TileEntityType("TerraFurnaceEntity")) {
                    TerraFurnaceEntity tfe = se as TerraFurnaceEntity;
                    tfe.linkToCapacitor(ce);
                }
                Main.NewText("Succesfully linked to a capacitor, now transferring energy to it", Color.ForestGreen);

            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            mod.GetTileEntity<CapacitorEntity>().Kill(i, j);
        }
    }
    
}