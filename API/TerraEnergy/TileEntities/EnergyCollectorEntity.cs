using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.API.TerraEnergy.EnergyAPI;

namespace TUA.API.TerraEnergy.TileEntities
{

    class EnergyCollectorEntity : StorageEntity
    {
        private int drainRange = 50;
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
                boundCapacitor.energy.addEnergy(energy.consumeEnergy(boundCapacitor.maxTransferRate));
            }

            if (tile != null && tile.type != mod.TileType("EnergyCollector") || tile.type != mod.TileType("BasicTECapacitor") || tile.type != mod.TileType("TerraWaste") || tile.type != mod.TileType("TerraFurnace") && !energy.isFull())
            {
                if (Main.tile[i, j].type == TileID.LunarOre)
                {
                    energy.addEnergy(50);
                    Main.tile[i, j].type = (ushort)mod.TileType("TerraWaste");
                }

                if (Main.tile[i, j].active() && !(Main.tile[i, j].type == (ushort)mod.TileType("TerraWaste")))
                {

                    energy.addEnergy(5);
                    if (Main.rand.Next(0, 1000) == 5)
                    {
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
            return Place(i - 1, j - 2);
        }
    }
}
