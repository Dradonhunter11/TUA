using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.CustomInventory;

namespace TUA.API.TerraEnergy.EnergyAPI
{
    abstract class StorageEntity : ModTileEntity
    {
        internal Core energy;
        public int maxEnergy;

        public virtual List<ExtraSlot> getSlot()
        {
            List<ExtraSlot> dumpList = new List<ExtraSlot>();
            return dumpList;
        }

        public sealed override void Load(TagCompound tag)
        {
            energy = new Core(0);
            LoadEntity(tag);
            energy.addEnergy(tag.GetAsInt("energy"));
            
        }

        public static byte[] CompressBytes(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (var deflateStream = new DeflateStream(output, CompressionMode.Compress, false))
            {
                deflateStream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] DecompressBytes(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (var deflateStream = new DeflateStream(input, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(output);
            }
            return output.ToArray();
        }

        public sealed override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            SaveEntity(tag);
            tag.Add("energy", energy.getCurrentEnergyLevel());
            return tag;

        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            TagCompound tag = new TagCompound();
            tag.Add("energy", energy.getCurrentEnergyLevel());
            tag.Add("maxEnergy", energy.getMaxEnergyLevel());
            TagIO.Write(tag, writer);
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            TagCompound tag = TagIO.Read(reader);
            energy = new Core(tag.GetAsInt("maxEnergy"));
            energy.addEnergy(tag.GetAsInt("energy"));
        }

        public virtual void SaveEntity(TagCompound tag)
        {
        }

        public virtual void LoadEntity(TagCompound tag)
        {
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

        public int Place(int i, int j)
        {
            ModTileEntity modTileEntity = ModTileEntity.ConstructFromBase(this);
            modTileEntity.Position = new Point16(i, j);
            modTileEntity.ID = TileEntity.AssignNewID();
            modTileEntity.type = (byte)this.Type;
            TileEntity.ByID[modTileEntity.ID] = (TileEntity)modTileEntity;
            TileEntity.ByPosition[modTileEntity.Position] = (TileEntity)modTileEntity;
            energy = new Core(maxEnergy);
            return modTileEntity.ID;
        }
    }
}
