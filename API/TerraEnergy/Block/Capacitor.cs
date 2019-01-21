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
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.API.TerraEnergy.Items;
using TUA.API.TerraEnergy.TileEntities;

namespace TUA.API.TerraEnergy.Block
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

       

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            mod.GetTileEntity<CapacitorEntity>().Kill(i, j);
        }
    }

}