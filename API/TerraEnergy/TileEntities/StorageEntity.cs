using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse.API.TerraEnergy.TileEntities
{
    abstract class StorageEntity : ModTileEntity
    {
        internal Core energy;
        public int maxEnergy;
        protected TagCompound tag = new TagCompound();


        public override void Load(TagCompound tag)
        {
            energy.addEnergy(tag.GetAsInt("energy"));
        }

        public override TagCompound Save()
        {

            tag.Add("energy", energy.getCurrentEnergyLevel());
            return tag;

        }

        public Core getEnergy()
        {
            if (energy == null)
            {
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
