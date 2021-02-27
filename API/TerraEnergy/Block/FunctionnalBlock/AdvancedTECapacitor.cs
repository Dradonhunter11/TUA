using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.Interface;
using TUA.API.TerraEnergy.TileEntities;
using TUA.Items;

namespace TUA.API.TerraEnergy.Block.FunctionnalBlock
{
    class AdvancedTECapacitor : TUATile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<AdvancedTECapacitorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = ModContent.GetInstance<AdvancedTECapacitorEntity>().Find(left, top);

            if (index == -1)
            {
                return;
            }
            if (currentSelectedItem.type == ModContent.ItemType<TerraMeter>())
            {

                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.GetEnergy().getCurrentEnergyLevel() + " / " + se.GetEnergy().getMaxEnergyLevel() + " TE in this Capacitor");
                return;
            }

            if (currentSelectedItem.type == ModContent.ItemType<RodOfLinking>())
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                int tileEntityID = it.GetEntity();

                if (tileEntityID == -1)
                {
                    Main.NewText("The rod of linking is bound to nothing");
                    return;
                }

                CapacitorEntity ce = (CapacitorEntity)TileEntity.ByID[index];

                if (ModTileEntity.GetTileEntity(it.GetStoredEntityType().type) is ITECapacitorLinkable terraEnergyCompatibleLinkable)
                {
                    terraEnergyCompatibleLinkable.LinkToCapacitor(ce);
                    Main.NewText("Succesfully linked to a capacitor, now transferring energy to it", Color.ForestGreen);
                }
                return;
            }

            AdvancedTECapacitorEntity capacitorEntity = (AdvancedTECapacitorEntity)TileEntity.ByID[index];
            capacitorEntity.Activate();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<AdvancedTECapacitorEntity>().Kill(i, j);
        }
    }

    class AdvancedTECapacitorEntity : CapacitorEntity
    {
        public AdvancedTECapacitorEntity()
        {
            maxEnergy = 2000000;
            maxTransferRate = 200;
        }

        public override void LoadEntity(TagCompound tag)
        {
            energy = new EnergyCore(maxEnergy);
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            energy = new EnergyCore(maxEnergy);
            maxTransferRate = 200;
            return Place(i - 1, j - 1);
        }
    }
}
