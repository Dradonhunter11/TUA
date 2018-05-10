using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Utilities;
using TerrariaUltraApocalypse.Dimension.LunarDimension;

namespace TerrariaUltraApocalypse.Dimension
{
    class TUADimFile
    {
        private static object padlock = new object();


        public static void saveWorld()
        {
            bool useCloudSaving = false;
            lock (TUADimFile.padlock)
            {
                try
                {
                    Directory.CreateDirectory(Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/");
                }
                catch
                {
                }
                byte[] local_1 = (byte[])null;
                int local_2 = 0;
                using (MemoryStream resource_1 = new MemoryStream(7000000))
                {
                    using (BinaryWriter resource_0 = new BinaryWriter((Stream)resource_1))
                        TUADimFile.SaveWorld_Version2(resource_0);
                    local_1 = resource_1.ToArray();
                    local_2 = local_1.Length;
                }
                if (local_1 == null)
                    return;
                byte[] local_5 = (byte[])null;
                if (FileUtilities.Exists(Main.worldPathName + Main.ActiveWorldFileData.Name + "/dim/solar.dim", useCloudSaving))
                    local_5 = FileUtilities.ReadAllBytes(Main.worldPathName + Main.ActiveWorldFileData.Name + "/dim/solar.dim", useCloudSaving);
                FileUtilities.Write(Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/solar.dim", local_1, local_2, useCloudSaving);
                byte[] local_1_1 = FileUtilities.ReadAllBytes(Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/solar.dim", useCloudSaving);
                string local_6 = (string)null;
                using (MemoryStream resource_3 = new MemoryStream(local_1_1, 0, local_2, false))
                {
                    using (BinaryReader resource_2 = new BinaryReader((Stream)resource_3))
                    {
                        local_6 = Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/solar.dim";
                    }
                }
                if (local_6 != null && local_5 != null)
                    FileUtilities.WriteAllBytes(local_6, local_5, useCloudSaving);
                WorldGen.saveLock = false;
            }
            Main.serverGenLock = false;
        }

        public static void SaveWorld_Version2(BinaryWriter writer)
        {
            int[] pointers = new int[8] { TUADimFile.SaveDimFileFormatHeader(writer), TUADimFile.SaveWorldHeader(writer), TUADimFile.SaveWorldTiles(writer), /*TUADimFile.SaveChests(writer), TUADimFile.SaveSigns(writer),*/ TUADimFile.SaveNPCs(writer), TUADimFile.SaveTileEntities(writer), TUADimFile.SaveWeightedPressurePlates(writer), 0, 0 };
            TUADimFile.SaveFooter(writer);
            TUADimFile.SaveHeaderPointers(writer, pointers);
        }

        private static int SaveDimFileFormatHeader(BinaryWriter writer) {
            short num1 = (short)400;
            short num2 = 10;
            writer.Write(188);
            Main.WorldFileMetadata.IncrementAndWrite(writer);
            writer.Write((short)10);
            for (int index = 0; index < (int)num2; ++index)
                writer.Write(0);
            writer.Write(num1);
            byte num3 = 0;
            byte num4 = 1;
            for (int index = 0; index < (int)num1; ++index)
            {
                if (Main.tileFrameImportant[index])
                    num3 |= num4;
                if ((int)num4 == 128)
                {
                    writer.Write(num3);
                    num3 = (byte)0;
                    num4 = (byte)1;
                }
                else
                    num4 <<= 1;
            }
            if ((int)num4 != 1)
                writer.Write(num3);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveHeaderPointers(BinaryWriter writer, int[] pointers)
        {
            writer.BaseStream.Position = 0L;
            writer.Write(188);
            writer.BaseStream.Position += 20L;
            writer.Write((short)pointers.Length);
            for (int index = 0; index < pointers.Length; ++index)
                writer.Write(pointers[index]);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveWorldHeader(BinaryWriter writer)
        {
            writer.Write("solar");
            writer.Write(Main.ActiveWorldFileData.SeedText);
            writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
            writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
            writer.Write(Main.worldID);
            writer.Write((int)Main.leftWorld);
            writer.Write((int)Main.rightWorld);
            writer.Write((int)Main.topWorld);
            writer.Write((int)Main.bottomWorld);
            writer.Write(Solar.maxTilesY);
            writer.Write(Solar.maxTilesX);
            writer.Write(Main.expertMode);
            writer.Write(Main.ActiveWorldFileData.CreationTime.ToBinary());
            writer.Write((byte)Main.moonType);
            writer.Write(Main.treeX[0]);
            writer.Write(Main.treeX[1]);
            writer.Write(Main.treeX[2]);
            writer.Write(Main.treeStyle[0]);
            writer.Write(Main.treeStyle[1]);
            writer.Write(Main.treeStyle[2]);
            writer.Write(Main.treeStyle[3]);
            writer.Write(Main.caveBackX[0]);
            writer.Write(Main.caveBackX[1]);
            writer.Write(Main.caveBackX[2]);
            writer.Write(Main.caveBackStyle[0]);
            writer.Write(Main.caveBackStyle[1]);
            writer.Write(Main.caveBackStyle[2]);
            writer.Write(Main.caveBackStyle[3]);
            writer.Write(Main.iceBackStyle);
            writer.Write(Main.jungleBackStyle);
            writer.Write(Main.hellBackStyle);
            writer.Write(Main.spawnTileX);
            writer.Write(Main.spawnTileY);
            writer.Write(Main.worldSurface);
            writer.Write(Main.rockLayer);
            writer.Write(Main.dungeonX);
            writer.Write(Main.dungeonY);
            writer.Write(WorldGen.crimson);
            writer.Write(NPC.downedBoss1);
            writer.Write(NPC.downedBoss2);
            writer.Write(NPC.downedBoss3);
            writer.Write(NPC.downedQueenBee);
            writer.Write(NPC.downedMechBoss1);
            writer.Write(NPC.downedMechBoss2);
            writer.Write(NPC.downedMechBoss3);
            writer.Write(NPC.downedMechBossAny);
            writer.Write(NPC.downedPlantBoss);
            writer.Write(NPC.downedGolemBoss);
            writer.Write(NPC.downedSlimeKing);
            writer.Write(NPC.savedGoblin);
            writer.Write(NPC.savedWizard);
            writer.Write(NPC.savedMech);
            writer.Write(NPC.downedGoblins);
            writer.Write(NPC.downedClown);
            writer.Write(NPC.downedFrost);
            writer.Write(NPC.downedPirates);
            writer.Write(WorldGen.shadowOrbSmashed);
            writer.Write(WorldGen.spawnMeteor);
            writer.Write((byte)WorldGen.shadowOrbCount);
            writer.Write(WorldGen.altarCount);
            writer.Write(Main.hardMode);
            writer.Write(Main.invasionDelay);
            writer.Write(Main.invasionSize);
            writer.Write(Main.invasionType);
            writer.Write(Main.invasionX);
            writer.Write(Main.slimeRainTime);
            writer.Write((byte)Main.sundialCooldown);
            writer.Write(WorldGen.oreTier1);
            writer.Write(WorldGen.oreTier2);
            writer.Write(WorldGen.oreTier3);
            writer.Write((byte)WorldGen.treeBG);
            writer.Write((byte)WorldGen.corruptBG);
            writer.Write((byte)WorldGen.jungleBG);
            writer.Write((byte)WorldGen.snowBG);
            writer.Write((byte)WorldGen.hallowBG);
            writer.Write((byte)WorldGen.crimsonBG);
            writer.Write((byte)WorldGen.desertBG);
            writer.Write((byte)WorldGen.oceanBG);
            writer.Write((int)Main.cloudBGActive);
            writer.Write((short)Main.numClouds);
            writer.Write(Main.windSpeedSet);
            writer.Write(Main.anglerWhoFinishedToday.Count);
            for (int index = 0; index < Main.anglerWhoFinishedToday.Count; ++index)
                writer.Write(Main.anglerWhoFinishedToday[index]);
            writer.Write(NPC.savedAngler);
            writer.Write(Main.anglerQuest);
            writer.Write(NPC.savedStylist);
            writer.Write(NPC.savedTaxCollector);
            writer.Write(Main.invasionSizeStart);
            writer.Write((short)580);
            for (int index = 0; index < 580; ++index)
                writer.Write(NPC.killCount[index]);
            writer.Write(Main.fastForwardTime);
            writer.Write(NPC.downedFishron);
            writer.Write(NPC.downedMartians);
            writer.Write(NPC.downedAncientCultist);
            writer.Write(NPC.downedMoonlord);
            writer.Write(NPC.downedHalloweenKing);
            writer.Write(NPC.downedHalloweenTree);
            writer.Write(NPC.downedChristmasIceQueen);
            writer.Write(NPC.downedChristmasSantank);
            writer.Write(NPC.downedChristmasTree);
            writer.Write(NPC.downedTowerSolar);
            writer.Write(NPC.downedTowerVortex);
            writer.Write(NPC.downedTowerNebula);
            writer.Write(NPC.downedTowerStardust);
            writer.Write(NPC.TowerActiveSolar);
            writer.Write(NPC.TowerActiveVortex);
            writer.Write(NPC.TowerActiveNebula);
            writer.Write(NPC.TowerActiveStardust);
            writer.Write(NPC.LunarApocalypseIsUp);
            writer.Write(NPC.savedBartender);
            DD2Event.Save(writer);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveWorldTiles(BinaryWriter writer)
        {
            byte[] buffer = new byte[13];
            for (int i = 0; i < Solar.maxTilesX; ++i)
            {
                float num1 = (float)i / (float)Solar.maxTilesX;
                Main.statusText = Lang.gen[49] + " " + (object)(int)((double)num1 * 100.0 + 1.0) + "%";
                int num2;
                for (int j = 0; j < 2400; ++j)
                {
                    Tile tile = Solar.tile[i, j];
                    int index1 = 3;
                    int num3 = 0;
                    byte num4 = 0;
                    byte num5 = 0;
                    byte num6 = (byte)num3;
                    bool flag = false;

                    if (tile.active())
                    {
                        flag = true;
                        if ((int)tile.type == (int)sbyte.MaxValue)
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (!tile.active())
                            {
                                flag = false;
                                //if (Main.netMode != 0)
                                    //NetMessage.SendData(17, -1, -1, "", 0, (float)i, (float)j, 0.0f, 0, 0, 0);
                            }
                        }
                    }
                    if (flag)
                    {
                        num6 |= (byte)2;
                        if ((int)tile.type == (int)sbyte.MaxValue)
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            //if (!tile.active() && Main.netMode != 0)
                                //NetMessage.SendData(17, -1, -1, "", 0, (float)i, (float)j, 0.0f, 0, 0, 0);
                        }
                        buffer[index1] = (byte)tile.type;
                        ++index1;
                        if ((int)tile.type > (int)byte.MaxValue)
                        {
                            buffer[index1] = (byte)((uint)tile.type >> 8);
                            ++index1;
                            num6 |= (byte)32;
                        }
                        if (Main.tileFrameImportant[(int)tile.type])
                        {
                            buffer[index1] = (byte)((uint)tile.frameX & (uint)byte.MaxValue);
                            int index2 = index1 + 1;
                            buffer[index2] = (byte)(((int)tile.frameX & 65280) >> 8);
                            int index3 = index2 + 1;
                            buffer[index3] = (byte)((uint)tile.frameY & (uint)byte.MaxValue);
                            int index4 = index3 + 1;
                            buffer[index4] = (byte)(((int)tile.frameY & 65280) >> 8);
                            index1 = index4 + 1;
                        }
                        if ((int)tile.color() != 0)
                        {
                            num4 |= (byte)8;
                            buffer[index1] = tile.color();
                            ++index1;
                        }
                    }
                    if ((int)tile.wall != 0)
                    {
                        num6 |= (byte)4;
                        buffer[index1] = (byte)tile.wall;
                        ++index1;
                        if ((int)tile.wallColor() != 0)
                        {
                            num4 |= (byte)16;
                            buffer[index1] = tile.wallColor();
                            ++index1;
                        }
                    }
                    if ((int)tile.liquid != 0)
                    {
                        if (tile.lava())
                            num6 |= (byte)16;
                        else if (tile.honey())
                            num6 |= (byte)24;
                        else
                            num6 |= (byte)8;
                        buffer[index1] = tile.liquid;
                        ++index1;
                    }
                    if (tile.wire())
                        num5 |= (byte)2;
                    if (tile.wire2())
                        num5 |= (byte)4;
                    if (tile.wire3())
                        num5 |= (byte)8;
                    int num7 = !tile.halfBrick() ? ((int)tile.slope() == 0 ? 0 : (int)tile.slope() + 1 << 4) : 16;
                    byte num8 = (byte)((uint)num5 | (uint)(byte)num7);
                    if (tile.actuator())
                        num4 |= (byte)2;
                    if (tile.inActive())
                        num4 |= (byte)4;
                    if (tile.wire4())
                        num4 |= (byte)32;
                    int index5 = 2;
                    if ((int)num4 != 0)
                    {
                        num8 |= (byte)1;
                        buffer[index5] = num4;
                        --index5;
                    }
                    if ((int)num8 != 0)
                    {
                        num6 |= (byte)1;
                        buffer[index5] = num8;
                        --index5;
                    }
                    short num9 = 0;
                    int index6 = j + 1;
                    for (int index2 = Solar.maxTilesY - j - 1; index2 > 0 && tile.isTheSameAs(Solar.tile[i, index6]); ++index6)
                    {
                        ++num9;
                        --index2;
                    }
                    num2 = j + (int)num9;
                    if ((int)num9 > 0)
                    {
                        buffer[index1] = (byte)((uint)num9 & (uint)byte.MaxValue);
                        ++index1;
                        if ((int)num9 > (int)byte.MaxValue)
                        {
                            num6 |= (byte)128;
                            buffer[index1] = (byte)(((int)num9 & 65280) >> 8);
                            ++index1;
                        }
                        else
                            num6 |= (byte)64;
                    }
                    buffer[index5] = num6;
                    writer.Write(buffer, index5, index1 - index5);
                }
            }
            return (int)writer.BaseStream.Position;
        }

        /*
        private static int SaveChests(BinaryWriter writer)
        {
            short num = 0;
            for (int index = 0; index < 1000; ++index)
            {
                Chest chest = Main.chest[index];
                if (chest != null)
                {
                    bool flag = false;
                    for (int x = chest.x; x <= chest.x + 1; ++x)
                    {
                        for (int y = chest.y; y <= chest.y + 1; ++y)
                        {
                            if (x >= 0 && y >= 0 && (x < Main.maxTilesX && y < Main.maxTilesY))
                            {
                                Tile tile = Main.tile[x, y];
                                if (!tile.active() || !Main.tileContainer[(int)tile.type])
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            else
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                        Main.chest[index] = (Chest)null;
                    else
                        ++num;
                }
            }
            writer.Write(num);
            writer.Write((short)40);
            for (int index1 = 0; index1 < 1000; ++index1)
            {
                Chest chest = Main.chest[index1];
                if (chest != null)
                {
                    writer.Write(chest.x);
                    writer.Write(chest.y);
                    writer.Write(chest.name);
                    for (int index2 = 0; index2 < 40; ++index2)
                    {
                        Item obj = chest.item[index2];
                        if (obj == null)
                        {
                            writer.Write((short)0);
                        }
                        else
                        {
                            if (obj.stack > obj.maxStack)
                                obj.stack = obj.maxStack;
                            if (obj.stack < 0)
                                obj.stack = 1;
                            writer.Write((short)obj.stack);
                            if (obj.stack > 0)
                            {
                                writer.Write(obj.netID);
                                writer.Write(obj.prefix);
                            }
                        }
                    }
                }
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveSigns(BinaryWriter writer)
        {
            short num = 0;
            for (int index = 0; index < 1000; ++index)
            {
                Sign sign = Main.sign[index];
                if (sign != null && sign.text != null)
                    ++num;
            }
            writer.Write(num);
            for (int index = 0; index < 1000; ++index)
            {
                Sign sign = Main.sign[index];
                if (sign != null && sign.text != null)
                {
                    writer.Write(sign.text);
                    writer.Write(sign.x);
                    writer.Write(sign.y);
                }
            }
            return (int)writer.BaseStream.Position;
        }
        */
        private static int SaveDummies(BinaryWriter writer)
        {
            int num = 0;
            for (int index = 0; index < 1000; ++index)
            {
                if (DeprecatedClassLeftInForLoading.dummies[index] != null)
                    ++num;
            }
            writer.Write(num);
            for (int index = 0; index < 1000; ++index)
            {
                DeprecatedClassLeftInForLoading dummy = DeprecatedClassLeftInForLoading.dummies[index];
                if (dummy != null)
                {
                    writer.Write(dummy.x);
                    writer.Write(dummy.y);
                }
            }
            return (int)writer.BaseStream.Position;
        }
        
        private static int SaveNPCs(BinaryWriter writer)
        {
            for (int index = 0; index < Main.npc.Length; ++index)
            {
                NPC npc = Main.npc[index];
                if (npc.active && npc.townNPC && npc.type != 368)
                {
                    writer.Write(npc.active);
                    writer.Write(npc.GivenName);
                    writer.Write(npc.FullName);
                    writer.Write(npc.position.X);
                    writer.Write(npc.position.Y);
                    writer.Write(npc.homeless);
                    writer.Write(npc.homeTileX);
                    writer.Write(npc.homeTileY);
                }
            }
            writer.Write(false);
            for (int index = 0; index < Main.npc.Length; ++index)
            {
                NPC npc = Main.npc[index];
                if (npc.active && NPCID.Sets.SavesAndLoads[npc.type])
                {
                    writer.Write(npc.active);
                    writer.Write(npc.GivenName);
                    writer.WriteVector2(npc.position);
                }
            }
            writer.Write(false);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveFooter(BinaryWriter writer)
        {
            writer.Write(true);
            writer.Write(Main.worldName);
            writer.Write(Main.worldID);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveTileEntities(BinaryWriter writer)
        {
            writer.Write(TileEntity.ByID.Count);
            foreach (KeyValuePair<int, TileEntity> keyValuePair in TileEntity.ByID)
                TileEntity.Write(writer, keyValuePair.Value, false);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveWeightedPressurePlates(BinaryWriter writer)
        {
            writer.Write(PressurePlateHelper.PressurePlatesPressed.Count);
            foreach (KeyValuePair<Point, bool[]> keyValuePair in PressurePlateHelper.PressurePlatesPressed)
            {
                writer.Write(keyValuePair.Key.X);
                writer.Write(keyValuePair.Key.Y);
            }
            return (int)writer.BaseStream.Position;
        }

    }
}
