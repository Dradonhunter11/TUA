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
    abstract class StorageEntity : ModTileEntity
    {
        internal Core energy;
        public int maxEnergy;



        public override void Load(TagCompound tag)
        {
            energy.addEnergy(tag.GetAsInt("energy"));
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("energy", energy.getCurrentEnergyLevel());
            return tag;
        }

        public Core getEnergy()
        {
            if (energy == null) {
                energy = new Core(maxEnergy);
            }
            return energy;
        }

        public int getMaxEnergyStored()
        {
            if (energy == null)
            {
                energy = new Core(maxEnergy);
            }
            return energy.getMaxEnergyLevel();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            Main.NewText("X " + i + " Y " + j);

            return Place(i - 1, j - 2);
        }
    }
    
}