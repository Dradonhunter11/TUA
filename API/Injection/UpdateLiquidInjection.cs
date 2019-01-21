using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Net;
using TUA.API.LiquidAPI;
using TUA.API.LiquidAPI.Swap;

namespace TUA.API.Injection
{
    class UpdateLiquidInjection : ModWorld
    {
        public override void PreUpdate()
        {
            
        }

        public static void UpdateLiquid()
        {
            FieldInfo wetCounterInfo =
                typeof(Liquid).GetField("wetCounter", BindingFlags.NonPublic | BindingFlags.Static);
            int wetCounter = (int)wetCounterInfo.GetValue(null);


            int arg_07_0 = Main.netMode;
            if (!WorldGen.gen)
            {
                if (!Liquid.panicMode)
                {
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 4000)
                    {
                        Liquid.panicCounter++;
                        if (Liquid.panicCounter > 1800 || Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 13500)
                        {
                            Liquid.StartPanic();
                        }
                    }
                    else
                    {
                        Liquid.panicCounter = 0;
                    }
                }
                if (Liquid.panicMode)
                {
                    int num = 0;
                    while (Liquid.panicY >= 3 && num < 5)
                    {
                        num++;
                        LiquidExtension.QuickWater(0, Liquid.panicY, Liquid.panicY);
                        Liquid.panicY--;
                        if (Liquid.panicY < 3)
                        {
                            Console.WriteLine(Language.GetTextValue("Misc.WaterSettled"));
                            Liquid.panicCounter = 0;
                            Liquid.panicMode = false;
                            WorldGen.WaterCheck();
                            if (Main.netMode == 2)
                            {
                                for (int i = 0; i < 255; i++)
                                {
                                    for (int j = 0; j < Main.maxSectionsX; j++)
                                    {
                                        for (int k = 0; k < Main.maxSectionsY; k++)
                                        {
                                            Netplay.Clients[i].TileSections[j, k] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
            if (Liquid.quickSettle || Liquid.numLiquid > 2000)
            {
                Liquid.quickFall = true;
            }
            else
            {
                Liquid.quickFall = false;
            }
            wetCounter++;
            int num2 = Liquid.maxLiquid / Liquid.cycles;
            int num3 = num2 * (wetCounter - 1);
            int num4 = num2 * wetCounter;
            if (wetCounter == Liquid.cycles)
            {
                num4 = Liquid.numLiquid;
            }
            if (num4 > Liquid.numLiquid)
            {
                num4 = Liquid.numLiquid;
                int arg_190_0 = Main.netMode;
                wetCounter = Liquid.cycles;
            }
            if (Liquid.quickFall)
            {
                for (int l = num3; l < num4; l++)
                {
                    Main.liquid[l].delay = 10;
                    Main.liquid[l].ModdedLiquidUpdate();
                    Main.tile[Main.liquid[l].x, Main.liquid[l].y].skipLiquid(false);
                }
            }
            else
            {
                for (int m = num3; m < num4; m++)
                {
                    if (!Main.tile[Main.liquid[m].x, Main.liquid[m].y].skipLiquid())
                    {
                        Main.liquid[m].ModdedLiquidUpdate();
                    }
                    else
                    {
                        Main.tile[Main.liquid[m].x, Main.liquid[m].y].skipLiquid(false);
                    }
                }
            }
            if (wetCounter >= Liquid.cycles - 1)
            {
                wetCounter = 0;
                for (int n = Liquid.numLiquid - 1; n >= 0; n--)
                {
                    if (Main.liquid[n].kill > 4)
                    {
                        Liquid.DelWater(n);
                    }
                }
                int num5 = Liquid.maxLiquid - (Liquid.maxLiquid - Liquid.numLiquid);
                if (num5 > LiquidBuffer.numLiquidBuffer)
                {
                    num5 = LiquidBuffer.numLiquidBuffer;
                }
                for (int num6 = 0; num6 < num5; num6++)
                {
                    Main.tile[Main.liquidBuffer[0].x, Main.liquidBuffer[0].y].checkingLiquid(false);
                    Liquid.AddWater(Main.liquidBuffer[0].x, Main.liquidBuffer[0].y);
                    LiquidBuffer.DelBuffer(0);
                }
                if (Liquid.numLiquid > 0 && Liquid.numLiquid > Liquid.stuckAmount - 50 && Liquid.numLiquid < Liquid.stuckAmount + 50)
                {
                    Liquid.stuckCount++;
                    if (Liquid.stuckCount >= 10000)
                    {
                        Liquid.stuck = true;
                        for (int num7 = Liquid.numLiquid - 1; num7 >= 0; num7--)
                        {
                           Liquid.DelWater(num7);
                        }
                        Liquid.stuck = false;
                        Liquid.stuckCount = 0;
                    }
                }
                else
                {
                    Liquid.stuckCount = 0;
                    Liquid.stuckAmount = Liquid.numLiquid;
                }
            }

            FieldInfo _netChangeSetInfo =
                typeof(Liquid).GetField("_netChangeSet", BindingFlags.NonPublic | BindingFlags.Static);
            HashSet<int> _netChangeSet = (HashSet<int>) _netChangeSetInfo.GetValue(null);

            FieldInfo _swapNetChangeSetInfo =
                typeof(Liquid).GetField("_swapNetChangeSet", BindingFlags.NonPublic | BindingFlags.Static);
            HashSet<int> _swapNetChangeSet = (HashSet<int>)_netChangeSetInfo.GetValue(null);
            


            if (!WorldGen.gen && Main.netMode == 2 && _netChangeSet.Count > 0)
            {
                Utils.Swap<HashSet<int>>(ref _netChangeSet, ref _swapNetChangeSet);
                NetManager.Instance.Broadcast(NetLiquidModule.Serialize(_swapNetChangeSet), -1);
                _swapNetChangeSet.Clear();
            }

            _netChangeSetInfo.SetValue(null, _netChangeSet);
            _swapNetChangeSetInfo.SetValue(null, _swapNetChangeSet);
            wetCounterInfo.SetValue(null, wetCounter);
        }

        

        public static void LavaCheck(int x, int y)
        {
            Tile tile = Main.tile[x - 1, y];
            Tile tile2 = Main.tile[x + 1, y];
            Tile tile3 = Main.tile[x, y - 1];
            Tile tile4 = Main.tile[x, y + 1];
            Tile tile5 = Main.tile[x, y];
            if ((tile.liquid > 0 && !tile.lava()) || (tile2.liquid > 0 && !tile2.lava()) || (tile3.liquid > 0 && !tile3.lava()))
            {
                int num = 0;
                int num2 = TileID.LunarOre; //56
                if (!tile.lava())
                {
                    num += (int)tile.liquid;
                    tile.liquid = 0;
                }
                if (!tile2.lava())
                {
                    num += (int)tile2.liquid;
                    tile2.liquid = 0;
                }
                if (!tile3.lava())
                {
                    num += (int)tile3.liquid;
                    tile3.liquid = 0;
                }
                if (tile.honey() || tile2.honey() || tile3.honey())
                {
                    num2 = 230;
                }
                if (num >= 24)
                {
                    if (tile5.active() && Main.tileObsidianKill[(int)tile5.type])
                    {
                        WorldGen.KillTile(x, y, false, false, false);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)y, 0f, 0, 0, 0);
                        }
                    }
                    if (!tile5.active())
                    {
                        tile5.liquid = 0;
                        tile5.lava(false);
                        if (num2 == 56)
                        {
                            Main.PlaySound(SoundID.LiquidsWaterLava, new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)));
                        }
                        else
                        {
                            Main.PlaySound(SoundID.LiquidsHoneyLava, new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)));
                        }
                        WorldGen.PlaceTile(x, y, num2, true, true, -1, 0);
                        WorldGen.SquareTileFrame(x, y, true);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, x - 1, y - 1, 3, (num2 == 56) ? TileChangeType.LavaWater : TileChangeType.HoneyLava);
                            return;
                        }
                    }
                }
            }
            else if (tile4.liquid > 0 && !tile4.lava())
            {
                bool flag = false;
                if (tile5.active() && TileID.Sets.ForceObsidianKill[(int)tile5.type] && !TileID.Sets.ForceObsidianKill[(int)tile4.type])
                {
                    flag = true;
                }
                if (Main.tileCut[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)(y + 1), 0f, 0, 0, 0);
                    }
                }
                else if (tile4.active() && Main.tileObsidianKill[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(17, -1, -1, null, 0, (float)x, (float)(y + 1), 0f, 0, 0, 0);
                    }
                }
                if (!tile4.active() || flag)
                {
                    if (tile5.liquid < 24)
                    {
                        tile5.liquid = 0;
                        tile5.liquidType(0);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, x - 1, y, 3, TileChangeType.None);
                            return;
                        }
                    }
                    else
                    {
                        int num3 = 56;
                        if (tile4.honey())
                        {
                            num3 = TileID.LunarOre; //230
                        }
                        tile5.liquid = 0;
                        tile5.lava(false);
                        tile4.liquid = 0;
                        if (num3 == 56)
                        {
                            Main.PlaySound(SoundID.LiquidsWaterLava, new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)));
                        }
                        else
                        {
                            Main.PlaySound(SoundID.LiquidsHoneyLava, new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)));
                        }
                        WorldGen.PlaceTile(x, y + 1, num3, true, true, -1, 0);
                        WorldGen.SquareTileFrame(x, y + 1, true);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, x - 1, y, 3, (num3 == 56) ? TileChangeType.LavaWater : TileChangeType.HoneyLava);
                        }
                    }
                }
            }
        }

        public static void DelWater(int l)
        {
            int num = Main.liquid[l].x;
            int num2 = Main.liquid[l].y;
            Tile tile = Main.tile[num - 1, num2];
            Tile tile2 = Main.tile[num + 1, num2];
            Tile tile3 = Main.tile[num, num2 + 1];
            Tile tile4 = Main.tile[num, num2];
            byte b = 2;
            if (tile4.liquid < b)
            {
                tile4.liquid = 0;
                if (tile.liquid < b)
                {
                    tile.liquid = 0;
                }
                else
                {
                    Liquid.AddWater(num - 1, num2);
                }
                if (tile2.liquid < b)
                {
                    tile2.liquid = 0;
                }
                else
                {
                    Liquid.AddWater(num + 1, num2);
                }
            }
            else if (tile4.liquid < 20)
            {
                if ((tile.liquid < tile4.liquid && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type])) || (tile2.liquid < tile4.liquid && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type])) || (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type])))
                {
                    tile4.liquid = 0;
                }
            }
            else if (tile3.liquid < 255 && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type]) && !Liquid.stuck)
            {
                Main.liquid[l].kill = 0;
                return;
            }
            if (tile4.liquid < 250 && Main.tile[num, num2 - 1].liquid > 0)
            {
                Liquid.AddWater(num, num2 - 1);
            }
            if (tile4.liquid == 0)
            {
                tile4.liquidType(0);
            }
            else
            {
                if ((tile2.liquid > 0 && Main.tile[num + 1, num2 + 1].liquid < 250 && !Main.tile[num + 1, num2 + 1].active()) || (tile.liquid > 0 && Main.tile[num - 1, num2 + 1].liquid < 250 && !Main.tile[num - 1, num2 + 1].active()))
                {
                    Liquid.AddWater(num - 1, num2);
                    Liquid.AddWater(num + 1, num2);
                }
                if (tile4.lava())
                {
                    LavaCheck(num, num2);
                    for (int i = num - 1; i <= num + 1; i++)
                    {
                        for (int j = num2 - 1; j <= num2 + 1; j++)
                        {
                            Tile tile5 = Main.tile[i, j];
                            if (tile5.active())
                            {
                                if (tile5.type == 2 || tile5.type == 23 || tile5.type == 109 || tile5.type == 199)
                                {
                                    tile5.type = 0;
                                    WorldGen.SquareTileFrame(i, j, true);
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
                                    }
                                }
                                else if (tile5.type == 60 || tile5.type == 70)
                                {
                                    tile5.type = 59;
                                    WorldGen.SquareTileFrame(i, j, true);
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendTileSquare(-1, num, num2, 3, TileChangeType.None);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (tile4.honey())
                {
                    Liquid.HoneyCheck(num, num2);
                }
            }
            if (Main.netMode == 2)
            {
                Liquid.NetSendLiquid(num, num2);
            }
            Liquid.numLiquid--;
            Main.tile[Main.liquid[l].x, Main.liquid[l].y].checkingLiquid(false);
            Main.liquid[l].x = Main.liquid[Liquid.numLiquid].x;
            Main.liquid[l].y = Main.liquid[Liquid.numLiquid].y;
            Main.liquid[l].kill = Main.liquid[Liquid.numLiquid].kill;
            if (Main.tileAlch[(int)tile4.type])
            {
                WorldGen.CheckAlch(num, num2);
            }
        }

    }
}
