using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaUltraApocalypse.API.TerraEnergy.Block;
using TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI;

namespace TerrariaUltraApocalypse.API.TerraEnergy.TileEntities
{
    class CapacitorEntity : StorageEntity
    {
        public List<EnergyCollectorEntity> storedCollector = new List<EnergyCollectorEntity>();

        public List<StorageEntity> storedFurnace = new List<StorageEntity>();
        public int maxTransferRate;

        public void addCollector(EnergyCollectorEntity collector)
        {
            storedCollector.Add(collector);
        }

        public void addStorageEntity(StorageEntity furnace)
        {
            storedFurnace.Add(furnace);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            //Main.NewText((tile.active() && (tile.type == mod.TileType<BasicTECapacitor>() || tile.type == mod.TileType<BasicTECapacitor>()) && tile.frameX == 0 && tile.frameY == 0));
            return tile.active() && (tile.type == mod.TileType<Capacitor>()) && tile.frameX == 0 && tile.frameY == 0;
        }

        public override void Update()
        {
            if (energy == null)
            {
                energy = new Core(maxEnergy);
            }
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            return Place(i - 1, j - 1);
        }
    }
}
