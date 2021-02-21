using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.TileEntities;

namespace TUA.API.TerraEnergy.Block
{
    class Capacitor : TUATile
    {
        public int maxEnergyStorage;

        public ModTileEntity GetCapacitorEntity()
        {
            return ModContent.GetInstance<CapacitorEntity>();

        }

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);
        }

       

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<CapacitorEntity>().Kill(i, j);
        }
    }

}