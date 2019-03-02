using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;
using TUA.API.Injection;
using TUA.API.LiquidAPI.LiquidMod;
using TUA.API.LiquidAPI.Swap;

namespace TUA.API.LiquidAPI
{
    static class LiquidExtension
    {
        public static void UpdateLiquid()
        {
            FieldInfo info = typeof(Liquid).GetField("wetCounter", BindingFlags.Static | BindingFlags.NonPublic);
            int wetCounter = (int)info.GetValue(null);
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
                        QuickWater(0, Liquid.panicY, Liquid.panicY);
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
            if (wetCounter >= Liquid.cycles)
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
                    AddModdedLiquidAround(Main.liquidBuffer[0].x, Main.liquidBuffer[0].y);
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
            /*
            if (!WorldGen.gen && Main.netMode == 2 && Liquid._netChangeSet.Count > 0)
            {
                Utils.Swap<HashSet<int>>(ref Liquid._netChangeSet, ref Liquid._swapNetChangeSet);
                NetManager.Instance.Broadcast(NetLiquidModule.Serialize(Liquid._swapNetChangeSet), -1);
                Liquid._swapNetChangeSet.Clear();
            }*/
        }

        /// <summary>
        /// tile.nactive() mean check if the tile is actuated
        /// </summary>
        /// <param name="self"></param>

        public static void ModdedLiquidUpdate(this Liquid self)
        {
            Main.tileSolid[TileID.Bubble] = true; //bubble

            LiquidRef liquidLeft = LiquidCore.grid[self.x - 1, self.y];
            LiquidRef liquidRight = LiquidCore.grid[self.x + 1, self.y];
            LiquidRef liquidUp = LiquidCore.grid[self.x, self.y - 1];
            LiquidRef liquidDown = LiquidCore.grid[self.x, self.y + 1];
            LiquidRef liquidSelf = LiquidCore.grid[self.x, self.y];


            if (liquidSelf.tile.nactive() && Main.tileSolid[(int) liquidSelf.tile.type] &&
                !Main.tileSolidTop[(int) liquidSelf.tile.type])
            {

                self.kill = 9;
            }
            else
            {
                byte liquidAmount = liquidSelf.tile.liquid;
                //liquidSelf.liquidAmount = 0;
                
                if (liquidSelf.tile.liquid <= (byte) 0)
                {
                    self.kill = 9;
                }
                else
                {
                    if (liquidSelf.tile.lava())
                    {
                        Liquid.LavaCheck(self.x, self.y);
                        if (!Liquid.quickFall)
                        {
                            if (self.delay < 5)
                            {
                                ++self.delay;
                                return;
                            }
                            self.delay = 0;
                        }
                    }
                    else
                    {
                        if (liquidLeft.tile.lava())
                            AddWater(self.x - 1, self.y);
                        if (liquidRight.tile.lava())
                            AddWater(self.x + 1, self.y);
                        if (liquidUp.tile.lava())
                            AddWater(self.x, self.y - 1);
                        if (liquidDown.tile.lava())
                            AddWater(self.x, self.y + 1);
                        if (liquidSelf.tile.honey())
                        {
                            Liquid.HoneyCheck(self.x, self.y);
                            if (!Liquid.quickFall)
                            {
                                if (self.delay < 10)
                                {
                                    ++self.delay;
                                    return;
                                }
                                self.delay = 0;
                            }
                        }
                        else
                        {
                            if (liquidLeft.tile.honey())
                                Liquid.AddWater(self.x - 1, self.y);
                            if (liquidRight.tile.honey())
                                Liquid.AddWater(self.x + 1, self.y);
                            if (liquidUp.tile.honey())
                                Liquid.AddWater(self.x, self.y - 1);
                            if (liquidDown.tile.honey())
                                Liquid.AddWater(self.x, self.y + 1);
                        }
                    }
                    
                    if ((!liquidDown.tile.nactive() || !Main.tileSolid[(int) liquidDown.tile.type] ||
                         Main.tileSolidTop[(int) liquidDown.tile.type]) &&
                        ((liquidDown.tile.liquid <= (byte) 0 ||
                          (int) liquidDown.tile.liquidType() == (int) liquidSelf.tile.liquidType()) &&
                         liquidDown.tile.liquid < byte.MaxValue))
                    {
                        float num = (float) ((int) byte.MaxValue - (int) liquidDown.tile.liquid);
                        if ((double) num > (double) liquidSelf.tile.liquid)
                            num = (float) liquidSelf.tile.liquid;
                        liquidSelf.tile.liquid -= (byte) num;
                        liquidDown.tile.liquid += (byte) num;
                        liquidDown.tile.liquid = liquidSelf.tile.liquid;
                        AddWater(self.x, self.y + 1);
                        liquidDown.tile.skipLiquid(true);
                        liquidSelf.tile.skipLiquid(true);
                        if (liquidSelf.tile.liquid > (byte) 250)
                        {
                            liquidSelf.tile.liquid = byte.MaxValue;
                        }
                        else
                        {
                            AddWater(self.x - 1, self.y);
                            AddWater(self.x + 1, self.y);
                        }
                    }

                    if (liquidSelf.tile.liquid > (byte) 0)
                    {
                        bool canReceiveLiquidLeft = true;
                        bool canReceiveLiquidRight = true;
                        for (int i = liquidSelf.x - 1; i > liquidSelf.x - 4; i--)
                        {
                            canReceiveLiquidLeft = true;
                            if (LiquidCore.grid[self.x - i, self.y].tile.nactive()
                                && Main.tileSolid[(int) liquidLeft.tile.type]
                                && !Main.tileSolidTop[(int) liquidLeft.tile.type]
                                && liquidLeft.tile.liquid > (byte) 0
                                && (int) liquidLeft.tile.liquidType() != (int) liquidSelf.tile.liquidType())
                            {
                                canReceiveLiquidLeft = false;
                            }

                            if (canReceiveLiquidLeft)
                            {
                                byte newLiquidAmount =
                                    (byte) Math.Floor(
                                        (double) (LiquidCore.grid[self.x - i + 1, self.y].liquidAmount / 2.0));
                                AddWater(self.x - i, self.y);
                                LiquidCore.grid[self.x - i, self.y].liquidAmount = newLiquidAmount;
                                LiquidCore.grid[self.x - i, self.y].liquidType =
                                    LiquidCore.grid[self.x - i + 1, self.y].liquidType;
                            }
                        }

                        for (int i = liquidSelf.x + 1; i < liquidSelf.x + 4; i++)
                        {
                            canReceiveLiquidRight = true;
                            if (LiquidCore.grid[self.x + i, self.y].tile.nactive()
                                && Main.tileSolid[(int) liquidLeft.tile.type]
                                && !Main.tileSolidTop[(int) liquidLeft.tile.type]
                                && liquidLeft.tile.liquid > (byte) 0
                                && (int) liquidLeft.tile.liquidType() != (int) liquidSelf.tile.liquidType())
                            {
                                canReceiveLiquidRight = false;
                            }

                            if (canReceiveLiquidRight)
                            {
                                byte newLiquidAmount =
                                    (byte) Math.Round(
                                        (double) (LiquidCore.grid[self.x + i - 1, self.y].liquidAmount / 2.0));
                                AddWater(self.x + i, self.y);
                                LiquidCore.grid[self.x + i, self.y].liquidAmount = newLiquidAmount;
                                LiquidCore.grid[self.x + i, self.y].liquidType =
                                    LiquidCore.grid[self.x + i - 1, self.y].liquidType;
                            }
                        }
                        if ((int) liquidSelf.tile.liquid == (int) liquidAmount)
                            ++self.kill;
                        else if (liquidSelf.tile.liquid == (byte) 254 && liquidAmount == byte.MaxValue)
                        {
                            liquidSelf.tile.liquid = byte.MaxValue;
                            ++self.kill;
                        }
                        else
                        {
                            AddModdedLiquidAround(self.x, self.y - 1);
                            self.kill = 9;
                        }
                    }
                }
            }
        }

        public static double QuickWater(int verbose = 0, int minY = -1, int maxY = -1)
        {
            Main.tileSolid[379] = true;
            int num1 = 0;
            if (minY == -1)
                minY = 3;
            if (maxY == -1)
                maxY = Main.maxTilesY - 3;
            for (int tileYLocation = maxY; tileYLocation >= minY; --tileYLocation)
            {
                if (verbose > 0)
                {
                    float num2 = (float)(maxY - tileYLocation) / (float)(maxY - minY + 1) / (float)verbose;
                    Main.statusText = Lang.gen[27].Value + " " + (object)(int)((double)num2 * 100.0 + 1.0) + "%";
                }
                else if (verbose < 0)
                {
                    float num2 = (float)(maxY - tileYLocation) / (float)(maxY - minY + 1) / -(float)verbose;
                    Main.statusText = Lang.gen[18].Value + " " + (object)(int)((double)num2 * 100.0 + 1.0) + "%";
                }
                for (int index2 = 0; index2 < 2; ++index2)
                {
                    int num2 = 2;
                    int num3 = Main.maxTilesX - 2;
                    int num4 = 1;
                    if (index2 == 1)
                    {
                        num2 = Main.maxTilesX - 2;
                        num3 = 2;
                        num4 = -1;
                    }
                    int tileXLocation = num2;
                    while (tileXLocation != num3)
                    {

                        LiquidRef liquids = LiquidCore.grid[tileXLocation, tileYLocation];
                        if (liquids.liquidAmount > (byte)0)
                        {
                            int num5 = -num4;
                            bool flag1 = false;
                            int x = tileXLocation;
                            int y = tileYLocation;
                            byte liquidType = liquids.liquidType;
                            byte liquid = liquids.getLiquidAmount();
                            liquids.liquidAmount = (byte)0;
                            bool flag4 = true;
                            int num7 = 0;
                            while (flag4 && x > 3 && (x < Main.maxTilesX - 3 && y < Main.maxTilesY - 3))
                            {
                                flag4 = false;
                                while (Main.tile[x, y + 1].liquid == (byte)0 && y < Main.maxTilesY - 5 && (!Main.tile[x, y + 1].nactive() || !Main.tileSolid[(int)Main.tile[x, y + 1].type] || Main.tileSolidTop[(int)Main.tile[x, y + 1].type]))
                                {
                                    flag1 = true;
                                    num5 = num4;
                                    num7 = 0;
                                    flag4 = true;
                                    ++y;
                                    
                                }
                                if (Main.tile[x, y + 1].liquid > (byte)0 && Main.tile[x, y + 1].liquid < byte.MaxValue && (int)Main.tile[x, y + 1].liquidType() == (int)liquidType)
                                {
                                    int num8 = (int)byte.MaxValue - (int)Main.tile[x, y + 1].liquid;
                                    if (num8 > (int)liquid)
                                        num8 = (int)liquid;
                                    Main.tile[x, y + 1].liquid += (byte)num8;
                                    liquid -= (byte)num8;
                                    if (liquid == (byte)0)
                                    {
                                        ++num1;
                                        break;
                                    }
                                }
                                if (num7 == 0)
                                {
                                    if (Main.tile[x + num5, y].liquid == (byte)0 && (!Main.tile[x + num5, y].nactive() || !Main.tileSolid[(int)Main.tile[x + num5, y].type] || Main.tileSolidTop[(int)Main.tile[x + num5, y].type]))
                                        num7 = num5;
                                    else if (Main.tile[x - num5, y].liquid == (byte)0 && (!Main.tile[x - num5, y].nactive() || !Main.tileSolid[(int)Main.tile[x - num5, y].type] || Main.tileSolidTop[(int)Main.tile[x - num5, y].type]))
                                        num7 = -num5;
                                }
                                if (num7 != 0 && Main.tile[x + num7, y].liquid == (byte)0 && (!Main.tile[x + num7, y].nactive() || !Main.tileSolid[(int)Main.tile[x + num7, y].type] || Main.tileSolidTop[(int)Main.tile[x + num7, y].type]))
                                {
                                    flag4 = true;
                                    x += num7;
                                }
                                if (flag1 && !flag4)
                                {
                                    flag1 = false;
                                    flag4 = true;
                                    num5 = -num4;
                                    num7 = 0;
                                }
                            }
                            if (tileXLocation != x && tileYLocation != y)
                                ++num1;

                            LiquidCore.grid[x,y].setLiquidsState(liquidType, true);
                            LiquidCore.grid[x, y].tile.liquid = liquid;
                        }
                        tileXLocation += num4;
                    }
                }
            }
            return (double)num1;
        }

        public static void AddModdedLiquidAround(int x, int y)
        {
            for (int i = LiquidBuffer.numLiquidBuffer; i > 0; i--)
            {
                LiquidBuffer.DelBuffer(i);
            }
            LiquidRef liquid = LiquidCore.grid[x, y];
            if (Main.tile[x, y] == null || liquid.checkingLiquid() || (x >= Main.maxTilesX - 5 || y >= Main.maxTilesY - 5) || (x < 5 || y < 5 || liquid.noLiquid()))
                return;
            if (Liquid.numLiquid >= Liquid.maxLiquid - 1)
            {
                LiquidBuffer.AddBuffer(x, y);
            }
            else
            {
                Main.liquid[Liquid.numLiquid].kill = 0;
                Main.liquid[Liquid.numLiquid].x = x;
                Main.liquid[Liquid.numLiquid].y = y;
                Main.liquid[Liquid.numLiquid].delay = 0;
                
                ++Liquid.numLiquid;
                if (Main.netMode == 2)
                    Liquid.NetSendLiquid(x, y);
                if (liquid.tile.active() || WorldGen.gen)
                    return;
                WorldGen.KillTile(x, y, false, false, false);
                if (Main.netMode != 2)
                    return;
                NetMessage.SendData(17, -1, -1, (NetworkText)null, 0, (float)x, (float)y, 0.0f, 0, 0, 0);
            }
        }

        public static void AddWater(int x, int y)
        {
            Tile checkTile = Main.tile[x, y];
            if (Main.tile[x, y] == null || checkTile.checkingLiquid() || (x >= Main.maxTilesX - 5 || y >= Main.maxTilesY - 5) || (x < 5 || y < 5 || checkTile.liquid == (byte)0))
                return;
            if (Liquid.numLiquid >= Liquid.maxLiquid - 1)
            {
                LiquidBuffer.AddBuffer(x, y);
            }
            else
            {
                checkTile.checkingLiquid(true);
                Main.liquid[Liquid.numLiquid].kill = 9;
                Main.liquid[Liquid.numLiquid].x = x;
                Main.liquid[Liquid.numLiquid].y = y;
                Main.liquid[Liquid.numLiquid].delay = 0;
                checkTile.skipLiquid(false);
                ++Liquid.numLiquid;
                if (Main.netMode == 2)
                    Liquid.NetSendLiquid(x, y);
                if (!checkTile.active() || WorldGen.gen)
                    return;
                bool flag = false;
                if (checkTile.lava())
                {
                    if (TileObjectData.CheckLavaDeath(checkTile))
                        flag = true;
                }
                else if (TileObjectData.CheckWaterDeath(checkTile))
                    flag = true;
                if (!flag)
                    return;
                WorldGen.KillTile(x, y, false, false, false);
                if (Main.netMode != 2)
                    return;
                NetMessage.SendData(17, -1, -1, (NetworkText)null, 0, (float)x, (float)y, 0.0f, 0, 0, 0);
            }
        }
    }
}
