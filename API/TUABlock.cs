using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TerrariaUltraApocalypse.API.Dev;
using TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI;
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;
using TerrariaUltraApocalypse.Items.Misc.Debug;

namespace TerrariaUltraApocalypse.API
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

        

        public sealed override void RightClick(int i, int j)
        {
            if (Main.LocalPlayer.HeldItem.type == mod.ItemType<DebugStick>())
            {
                readData(i, j);
            }
            NewRightClick(i, j);
            
        }

        public virtual void NewRightClick(int x, int y)
        {

        }

        public virtual void readData(int x, int y)
        {

        }

        

    }
}