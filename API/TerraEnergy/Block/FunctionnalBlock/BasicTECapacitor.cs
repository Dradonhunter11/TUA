using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.TileEntities;
using TUA.Items;

namespace TUA.API.TerraEnergy.Block.FunctionnalBlock
{
    class BasicTECapacitor : Capacitor
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<BasicTECapacitorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            Main.NewText("X " + i + " Y " + j);

            int index = ModContent.GetInstance<BasicTECapacitorEntity>().Find(left, top);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }
            if (currentSelectedItem.type == mod.ItemType("TerraMeter"))
            {

                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.getEnergy().getCurrentEnergyLevel() + " / " + se.getEnergy().getMaxEnergyLevel() + " TE in this Capacitor");
                return;
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking"))
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                StorageEntity se = it.getEntity();

                if (se == null)
                {
                    Main.NewText("The rod of linking is vound to nothing");
                    return;
                }

                CapacitorEntity ce = (CapacitorEntity)TileEntity.ByID[index];

                if (se.type == mod.TileEntityType("EnergyCollectorEntity"))
                {
                    EnergyCollectorEntity ece = se as EnergyCollectorEntity;
                    ece.linkToCapacitor(ce);

                }
                else if (se.type == mod.TileEntityType("TerraFurnaceEntity"))
                {
                    TerraFurnaceEntity tfe = se as TerraFurnaceEntity;
                    tfe.linkToCapacitor(ce);
                }
                Main.NewText("Succesfully linked to a capacitor, now transferring energy to it", Color.ForestGreen);
                return;
            }

            BasicTECapacitorEntity capacitorEntity = (BasicTECapacitorEntity) TileEntity.ByID[index];
            capacitorEntity.Activate();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<BasicTECapacitorEntity>().Kill(i, j);
        }
    }

    class BasicTECapacitorEntity : CapacitorEntity
    {
        public BasicTECapacitorEntity()
        {
            maxEnergy = 1000000;
            maxTransferRate = 50;
        }

        public override void LoadEntity(TagCompound tag)
        {
            energy = new Core(getMaxEnergyStored());
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            maxTransferRate = 2;
            return Place(i - 1, j - 1);
        }
    }
}
