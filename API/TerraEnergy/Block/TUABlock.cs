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
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Block
{
    class TUABlock : ModTile
    {
        public virtual ModTileEntity GetTileEntity()
        {
            return mod.GetTileEntity<StorageEntity>();
        }

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Origin = new Point16(1, 2);
        }


        public override bool Autoload(ref string name, ref string texture)
        {
            if (name == "TUABlock" || name == "Capacitor")
            {
                return false;
            }

            return base.Autoload(ref name, ref texture);
        }
        
    }
}