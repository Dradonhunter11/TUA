using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.TerraEnergy.Block;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.Tiles;

namespace TUA.API.TerraEnergy.TileEntities
{

    class EnergyCollectorEntity : StorageEntity
    {
        private readonly int drainRange = 50;
        private int maxEnergy = 100000;
        private CapacitorEntity boundCapacitor;

        public override void LoadEntity(TagCompound tag)
        {
            maxEnergy = 100000;
            energy = new Core(maxEnergy);
  
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

            if (boundCapacitor != null)
            {
                boundCapacitor.energy.addEnergy(energy.ConsumeEnergy(boundCapacitor.maxTransferRate));
            }

            if (tile != null && tile.type != ModContent.TileType<EnergyCollector>() || tile.type != ModContent.TileType<BasicTECapacitor>() || tile.type != ModContent.TileType<TerraWaste>() || tile.type != ModContent.TileType<TerraFurnace>() && !energy.isFull())
            {
                if (Main.tile[i, j].type == TileID.LunarOre)
                {
                    energy.addEnergy(50);
                    Main.tile[i, j].type = (ushort)ModContent.TileType<TerraWaste>();
                }

                if (Main.tile[i, j].active() && Main.tile[i, j].type != (ushort)ModContent.TileType<TerraWaste>())
                {

                    energy.addEnergy(5);
                    if (Main.rand.Next(0, 1000) == 5)
                    {
                        Main.tile[i, j].type = (ushort)ModContent.TileType<TerraWaste>();
                    }
                }
            }
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == ModContent.TileType<EnergyCollector>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            return Place(i - 1, j - 2);
        }
    }
}
