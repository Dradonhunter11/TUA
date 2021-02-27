using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Inventory;

namespace TUA.API.TerraEnergy.EnergyAPI
{
    abstract class StorageEntity : ModTileEntity
    {
        public EnergyCore energy;
        public int maxEnergy;

        internal int worldID => this.ID;

        public virtual List<ExtraSlot> GetSlot()
        {
            List<ExtraSlot> dumpList = new List<ExtraSlot>();
            return dumpList;
        }

        public sealed override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            SaveEntity(tag);
            tag.Add("energy", energy.getCurrentEnergyLevel());
            return tag;
        }

        public sealed override void Load(TagCompound tag)
        {
            energy = new EnergyCore(0);
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
            energy = new EnergyCore(tag.GetAsInt("maxEnergy"));
            energy.addEnergy(tag.GetAsInt("energy"));
        }

        public virtual void SaveEntity(TagCompound tag)
        {
        }

        public virtual void LoadEntity(TagCompound tag)
        {
        }

        public EnergyCore GetEnergy()
        {
            if (energy == null)
            {
                energy = new EnergyCore(maxEnergy);
            }
            return energy;
        }

        public int GetMaxEnergyStored()
        {
            if (energy == null)
            {
                energy = new EnergyCore(maxEnergy);
            }
            return energy.getMaxEnergyLevel();
        }

        public new int Place(int i, int j)
        {
            ModTileEntity modTileEntity = ModTileEntity.ConstructFromBase(this);
            modTileEntity.Position = new Point16(i, j);
            modTileEntity.ID = TileEntity.AssignNewID();
            modTileEntity.type = (byte)this.Type;
            TileEntity.ByID[modTileEntity.ID] = (TileEntity)modTileEntity;
            TileEntity.ByPosition[modTileEntity.Position] = (TileEntity)modTileEntity;
            energy = new EnergyCore(maxEnergy);
            return modTileEntity.ID;
        }
    }
}
