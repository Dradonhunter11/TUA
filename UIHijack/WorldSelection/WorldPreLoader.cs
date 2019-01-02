using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace TerrariaUltraApocalypse.UIHijack.WorldSelection
{
    class WorldPreLoader
    {
        public static void loadWorld(string path, bool loadFromCloud, Dictionary<string, object> dictionary, WorldFileData data)
        {


            WorldFile.IsWorldOnCloud = loadFromCloud;
            Main.checkXMas();
            Main.checkHalloween();
            bool flag = loadFromCloud && SocialAPI.Cloud != null;
            //patch file: flag
            byte[] buffer = FileUtilities.ReadAllBytes(path, flag);
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    try
                    {
                        WorldGen.loadFailed = false;
                        WorldGen.loadSuccess = false;
                        int num = binaryReader.ReadInt32();
                        WorldFile.versionNumber = num;
                        int num2;
                        if (num <= 87)
                        {
                            //num2 = WorldFile.LoadWorld_Version1(binaryReader);
                        }
                        else
                        {
                            num2 = LoadWorld_Version2(data, binaryReader, dictionary);
                        }
                    }
                    catch
                    {
                        WorldGen.loadFailed = true;
                        WorldGen.loadSuccess = false;
                        try
                        {
                            binaryReader.Close();
                            memoryStream.Close();
                        }
                        catch
                        {
                        }
                        return;
                    }
                }
            }

        }

        private static void LoadHeader(BinaryReader reader, Dictionary<string, object> dictionary)
        {
            int num = WorldFile.versionNumber;
            dictionary.Add("WorldName", reader.ReadString());
            if (num >= 179)
            {
                string seed;
                if (num == 179)
                {
                    seed = reader.ReadInt32().ToString();
                }
                else
                {
                    seed = reader.ReadString();
                }
                Main.ActiveWorldFileData.SetSeed(seed);
                Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
            }
            if (num >= 181)
            {
                Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
            }
            else
            {
                Main.ActiveWorldFileData.UniqueId = Guid.NewGuid();
            }
            Main.worldID = reader.ReadInt32();
            dictionary.Add("WorldLeft", reader.ReadInt32());
            dictionary.Add("WorldRight", reader.ReadInt32());
            dictionary.Add("WorldTop", reader.ReadInt32());
            dictionary.Add("WorldBottom", reader.ReadInt32());
            dictionary.Add("WorldMaxTileY", reader.ReadInt32());
            dictionary.Add("WorldMaxTileX", reader.ReadInt32());
            if (num >= 112)
            {
                Main.expertMode = reader.ReadBoolean();
            }
            else
            {
                Main.expertMode = false;
            }
            if (num >= 141)
            {
                //Main.ActiveWorldFileData.CreationTime = DateTime.FromBinary(reader.ReadInt64());
                reader.ReadInt64();
            }
            reader.ReadByte(); //Main moon type
            skipReadInt32Block(reader, 17);
            dictionary.Add("WorldSpawnTileX", reader.ReadInt32());
            dictionary.Add("WorldSpawnTileY", reader.ReadInt32());
            skipReadDoubleBlock(reader, 3);
            reader.ReadBoolean();
            reader.ReadInt32();
            skipReadBoolBlock(reader, 2);
            dictionary.Add("DungeonX", reader.ReadInt32());
            dictionary.Add("DungeonY", reader.ReadInt32());
            dictionary.Add("Crimson", reader.ReadBoolean());
            skipReadBoolBlock(reader, 10);
            if (num >= 118)
            {
                reader.ReadBoolean();
            }
            skipReadBoolBlock(reader, 9);
            reader.ReadByte();
            reader.ReadInt32();
            dictionary.Add("Hardmode", reader.ReadBoolean());
        }

        public static int LoadWorld_Version2(WorldFileData data, BinaryReader reader, Dictionary<string, object> dictionary)
        {
            reader.BaseStream.Position = 0L;
            bool[] importance;
            int[] array;
            if (LoadFileFormatHeader(reader, out importance, out array))
            {
                //return 5;
            }
            /*if (reader.BaseStream.Position != (long)array[0])
            {
                return 5;
            }*/
            LoadHeader(reader, dictionary);
            readBiomeLibsData(data, dictionary);
            if (reader.BaseStream.Position != (long)array[1])
            {
                return 5;
            }

            

            return 5;
        }

        private static void readBiomeLibsData(WorldFileData data, Dictionary<string, object> dictionary)
        {
            try
            {
                string path = Path.ChangeExtension(data.Path, ".twld");
                if (File.Exists(path))
                {
                    var buf = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
                    var tag = TagIO.FromStream(new MemoryStream(buf));
                    TagCompound biomeLibsWorldData = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "BiomeLibrary" && m.Get<string>("name") == "BiomeWorld");
                    if (biomeLibsWorldData != null)
                    {
                        if (biomeLibsWorldData.ContainsKey("64bit"))
                        {
                            dictionary.Add("Biomelibs 64bit world : ", biomeLibsWorldData.GetBool("64bit"));
                        }

                        dictionary.Add("Biomelibs chunk check : ", false);
                        if (biomeLibsWorldData.ContainsKey("chunked"))
                        {
                            dictionary["Biomelibs chunk check : "] = biomeLibsWorldData.GetBool("chunked");
                        }

                        dictionary.Add("Biomelibs infinite/Expended world : ", false);
                        if (biomeLibsWorldData.ContainsKey("infinite"))
                        {
                            dictionary["Biomelibs infinite/Expended world : "] = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Log(e.Message);
            }
        }

        private static bool LoadFileFormatHeader(BinaryReader reader, out bool[] importance, out int[] positions)
        {
            importance = null;
            positions = null;
            int num = reader.ReadInt32();
            WorldFile.versionNumber = num;
            if (num >= 135)
            {
                try
                {
                    Main.WorldFileMetadata = FileMetadata.Read(reader, FileType.World);
                    goto IL_54;
                }
                catch (Exception value)
                {
                    Console.WriteLine(Language.GetTextValue("Error.UnableToLoadWorld"));
                    Console.WriteLine(value);
                    return false;
                }
            }
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
            IL_54:
            short num2 = reader.ReadInt16();
            positions = new int[(int)num2];
            for (int i = 0; i < (int)num2; i++)
            {
                positions[i] = reader.ReadInt32();
            }
            short num3 = reader.ReadInt16();
            importance = new bool[(int)num3];
            byte b = 0;
            byte b2 = 128;
            for (int i = 0; i < (int)num3; i++)
            {
                if (b2 == 128)
                {
                    b = reader.ReadByte();
                    b2 = 1;
                }
                else
                {
                    b2 = (byte)(b2 << 1);
                }
                if ((b & b2) == b2)
                {
                    importance[i] = true;
                }
            }
            return true;
        }

        private static void skipReadInt32Block(BinaryReader reader, int count)
        {
            for (int i = 0; i < count; i++)
            {
                reader.ReadInt32();
            }
        }

        private static void skipReadDoubleBlock(BinaryReader reader, int count)
        {
            for (int i = 0; i < count; i++)
            {
                reader.ReadDouble();
            }
        }

        private static void skipReadBoolBlock(BinaryReader reader, int count)
        {
            for (int i = 0; i < count; i++)
            {
                reader.ReadBoolean();
            }
        }
    }

}
