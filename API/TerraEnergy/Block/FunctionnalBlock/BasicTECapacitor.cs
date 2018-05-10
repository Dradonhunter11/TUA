using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock
{
    class BasicTECapacitor : Capacitor
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<BasicTECapacitorEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            mod.GetTileEntity<BasicTECapacitorEntity>().Kill(i, j);
        }
    }

    class BasicTECapacitorEntity : CapacitorEntity
    {
        public BasicTECapacitorEntity() {
            maxEnergy = 1000000;
            maxTransferRate = 50;
        }

        public override void Load(TagCompound tag)
        {
            energy = new Core(getMaxEnergyStored());
            base.Load(tag);
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            energy = new Core(getMaxEnergyStored());
            maxTransferRate = 2;
            return base.Hook_AfterPlacement(i, j, type, style, direction);
        }
    }
}
