using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.Interface;
using TUA.API.TerraEnergy.TileEntities;
using TUA.Items;


namespace TUA.API.TerraEnergy.Block.FunctionnalBlock
{
    class EnergyCollector : TUATile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<EnergyCollectorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = ModContent.GetInstance<EnergyCollectorEntity>().Find(left, top);

            Main.NewText("X " + i + " Y " + j);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }
            if (currentSelectedItem.type == ModContent.ItemType<TerraMeter>())
            {
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.GetEnergy().getCurrentEnergyLevel() + " / " + se.GetEnergy().getMaxEnergyLevel() + " TE");
            }

            if (currentSelectedItem.type == ModContent.ItemType<RodOfLinking>())
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                StorageEntity se = (StorageEntity)TileEntity.ByID[index];

                var TE = TileEntity.ByID[index];


                if (TE is ITECapacitorLinkable)
                {
                    it.SaveLinkableEntityLocation(TE);
                }
                
                Main.NewText("Succesfully linked to a collector, now right click on a capacitor to unlink");
            }
        }
    }

}