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
using TerrariaUltraApocalypse.API.Injection;
using TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod;
using TerrariaUltraApocalypse.API.LiquidAPI.Swap;

namespace TerrariaUltraApocalypse.API.LiquidAPI
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

        public static void ModdedLiquidUpdate(this Liquid self)
        {
            Main.tileSolid[379] = true;

            Tile tile1 = Main.tile[self.x - 1, self.y];
            Tile tile2 = Main.tile[self.x + 1, self.y];
            Tile tile3 = Main.tile[self.x, self.y - 1];
            Tile tile4 = Main.tile[self.x, self.y + 1];
            Tile tile5 = Main.tile[self.x, self.y];
            LiquidRef tile1r = LiquidCore.grid[self.x - 1, self.y];
            LiquidRef tile2r = LiquidCore.grid[self.x + 1, self.y];
            LiquidRef tile3r = LiquidCore.grid[self.x, self.y - 1];
            LiquidRef tile4r = LiquidCore.grid[self.x, self.y + 1];
            LiquidRef tile5r = LiquidCore.grid[self.x, self.y];

            if (tile5.nactive() && Main.tileSolid[(int)tile5.type] && !Main.tileSolidTop[(int)tile5.type])
            {
                
                self.kill = 9;
            }
            else
            {
                byte liquid = tile5.liquid;
                
                if (tile5r.tile.liquid == (byte)0)
                {
                    self.kill = 9;
                }
                else
                {
                    if ((!tile4.nactive() || !Main.tileSolid[(int)tile4.type] || Main.tileSolidTop[(int)tile4.type]) && ((tile4.liquid <= (byte)0 || (int)tile4.liquidType() == (int)tile5.liquidType()) && tile4.liquid < byte.MaxValue))
                    {
                        float num = (float)((int)byte.MaxValue - (int)tile4.liquid);
                        if ((double)num > (double)tile5.liquid)
                            num = (float)tile5.liquid;
                        tile5r.tile.liquid -= (byte)num;
                        tile4r.tile.liquid += (byte)num;
                        tile4r.tile.liquid = tile5r.tile.liquid;
                        AddModdedLiquidAround(self.x, self.y + 1);
                        tile4.skipLiquid(true);
                        tile5.skipLiquid(true);
                        if (tile5.liquid > (byte)250)
                        {
                            tile5.liquid = byte.MaxValue;
                        }
                        else
                        {
                            AddModdedLiquidAround(self.x - 1, self.y);
                            AddModdedLiquidAround(self.x + 1, self.y);
                        }
                    }
                    if (tile5.liquid > (byte)0)
                    {
                        bool flag1 = true;
                        bool flag2 = true;
                        bool flag3 = true;
                        bool flag4 = true;
                        if (tile1.nactive() && Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type])
                            flag1 = false;
                        else if (tile1.liquid > (byte)0 && (int)tile1.liquidType() != (int)tile5.liquidType())
                            flag1 = false;
                        else if (Main.tile[self.x - 2, self.y].nactive() && Main.tileSolid[(int)Main.tile[self.x - 2, self.y].type] && !Main.tileSolidTop[(int)Main.tile[self.x - 2, self.y].type])
                            flag3 = false;
                        else if (Main.tile[self.x - 2, self.y].liquid == (byte)0)
                            flag3 = false;
                        else if (Main.tile[self.x - 2, self.y].liquid > (byte)0 && (int)Main.tile[self.x - 2, self.y].liquidType() != (int)tile5.liquidType())
                            flag3 = false;
                        if (tile2.nactive() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type])
                            flag2 = false;
                        else if (tile2.liquid > (byte)0 && (int)tile2.liquidType() != (int)tile5.liquidType())
                            flag2 = false;
                        else if (Main.tile[self.x + 2, self.y].nactive() && Main.tileSolid[(int)Main.tile[self.x + 2, self.y].type] && !Main.tileSolidTop[(int)Main.tile[self.x + 2, self.y].type])
                            flag4 = false;
                        else if (Main.tile[self.x + 2, self.y].liquid == (byte)0)
                            flag4 = false;
                        else if (Main.tile[self.x + 2, self.y].liquid > (byte)0 && (int)Main.tile[self.x + 2, self.y].liquidType() != (int)tile5.liquidType())
                            flag4 = false;
                        int num1 = 0;
                        if (tile5.liquid < (byte)3)
                            num1 = -1;
                        if (flag1 & flag2)
                        {
                            if (flag3 & flag4)
                            {
                                bool flag5 = true;
                                bool flag6 = true;
                                if (Main.tile[self.x - 3, self.y].nactive() && Main.tileSolid[(int)Main.tile[self.x - 3, self.y].type] && !Main.tileSolidTop[(int)Main.tile[self.x - 3, self.y].type])
                                    flag5 = false;
                                else if (LiquidCore.grid[self.x -3, self.y].liquid == 0)
                                    flag5 = false;
                                else if (LiquidCore.grid[self.x - 3, self.y].liquid != tile5r.liquid)
                                    flag5 = false;
                                if (Main.tile[self.x + 3, self.y].nactive() && Main.tileSolid[(int)Main.tile[self.x + 3, self.y].type] && !Main.tileSolidTop[(int)Main.tile[self.x + 3, self.y].type])
                                    flag6 = false;
                                else if (LiquidCore.grid[self.x + 3, self.y].liquid == 0)
                                    flag6 = false;
                                else if (LiquidCore.grid[self.x + 3, self.y].liquid != tile5r.liquid)
                                    flag6 = false;
                                if (flag5 & flag6)
                                {
                                    float num2 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Main.tile[self.x - 2, self.y].liquid + (int)Main.tile[self.x + 2, self.y].liquid + (int)Main.tile[self.x - 3, self.y].liquid + (int)Main.tile[self.x + 3, self.y].liquid + (int)tile5.liquid + num1) / 7.0);
                                    int num3 = 0;
                                    tile1r.liquid = tile5.liquid;
                                    if ((int)tile1.liquid != (int)(byte)num2)
                                    {
                                        tile1.liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x - 1, self.y);
                                    }
                                    else
                                        ++num3;
                                    tile2r.liquid = tile5.liquid;
                                    if ((int)tile2.liquid != (int)(byte)num2)
                                    {
                                        tile2.liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x + 1, self.y);
                                    }
                                    else
                                        ++num3;
                                    LiquidCore.grid[self.x - 2, self.y].liquid = tile5.liquid;
                                    if (LiquidCore.grid[self.x - 2, self.y].liquid != (int)(byte)num2)
                                    {
                                        Main.tile[self.x - 2, self.y].liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x - 2, self.y);
                                    }
                                    else
                                        ++num3;
                                    LiquidCore.grid[self.x + 2, self.y].liquid = tile5.liquid;
                                    if (LiquidCore.grid[self.x + 2, self.y].liquid != (int)(byte)num2)
                                    {
                                        Main.tile[self.x + 2, self.y].liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x + 2, self.y);
                                    }
                                    else
                                        ++num3;
                                    LiquidCore.grid[self.x - 3, self.y].liquid = tile5.liquid;
                                    if (LiquidCore.grid[self.x - 3, self.y].liquid != (int)(byte)num2)
                                    {
                                        Main.tile[self.x - 3, self.y].liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x - 3, self.y);
                                    }
                                    else
                                        ++num3;
                                    LiquidCore.grid[self.x + 3, self.y].liquid = tile5.liquid;
                                    if ((int)Main.tile[self.x + 3, self.y].liquid != (int)(byte)num2)
                                    {
                                        Main.tile[self.x + 3, self.y].liquid = (byte)num2;
                                        AddModdedLiquidAround(self.x + 3, self.y);
                                    }
                                    else
                                        ++num3;
                                    if ((int)tile1.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x - 1, self.y);
                                    if ((int)tile2.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x + 1, self.y);
                                    if ((int)Main.tile[self.x - 2, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x - 2, self.y);
                                    if ((int)Main.tile[self.x + 2, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x + 2, self.y);
                                    if ((int)Main.tile[self.x - 3, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x - 3, self.y);
                                    if ((int)Main.tile[self.x + 3, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                        AddModdedLiquidAround(self.x + 3, self.y);
                                    if (num3 != 6 || tile3.liquid <= (byte)0)
                                        tile5.liquid = (byte)num2;
                                }
                                else
                                {
                                    int num2 = 0;
                                    float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Main.tile[self.x - 2, self.y].liquid + (int)Main.tile[self.x + 2, self.y].liquid + (int)tile5.liquid + num1) / 5.0);
                                    tile1r.liquid = tile5r.liquid;
                                    if ((int)tile1.liquid != (int)(byte)num3)
                                    {
                                        tile1.liquid = (byte)num3;
                                        AddModdedLiquidAround(self.x - 1, self.y);
                                    }
                                    else
                                        ++num2;
                                    tile2r.liquid = tile5r.liquid;
                                    if ((int)tile2.liquid != (int)(byte)num3)
                                    {
                                        tile2.liquid = (byte)num3;
                                        AddModdedLiquidAround(self.x + 1, self.y);
                                    }
                                    else
                                        ++num2;
                                    LiquidCore.grid[self.x - 2, self.y].liquid = tile5r.liquid;
                                    if ((int)Main.tile[self.x - 2, self.y].liquid != (int)(byte)num3)
                                    {
                                        Main.tile[self.x - 2, self.y].liquid = (byte)num3;
                                        AddModdedLiquidAround(self.x - 2, self.y);
                                    }
                                    else
                                        ++num2;
                                    LiquidCore.grid[self.x + 2, self.y].liquid = tile5r.liquid;
                                    if ((int)Main.tile[self.x + 2, self.y].liquid != (int)(byte)num3)
                                    {
                                        Main.tile[self.x + 2, self.y].liquid = (byte)num3;
                                        AddModdedLiquidAround(self.x + 2, self.y);
                                    }
                                    else
                                        ++num2;
                                    if ((int)tile1.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        AddModdedLiquidAround(self.x - 1, self.y);
                                    if ((int)tile2.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        AddModdedLiquidAround(self.x + 1, self.y);
                                    if ((int)Main.tile[self.x - 2, self.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        AddModdedLiquidAround(self.x - 2, self.y);
                                    if ((int)Main.tile[self.x + 2, self.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        AddModdedLiquidAround(self.x + 2, self.y);
                                    if (num2 != 4 || tile3.liquid <= (byte)0)
                                        tile5.liquid = (byte)num3;
                                }
                            }
                            else if (flag3)
                            {
                                float num2 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Main.tile[self.x - 2, self.y].liquid + (int)tile5.liquid + num1) / 4.0 + 0.001);
                                tile1.liquidType((int)tile5.liquidType());
                                if ((int)tile1.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    tile1.liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x - 1, self.y);
                                }
                                tile2.liquidType((int)tile5.liquidType());
                                if ((int)tile2.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    tile2.liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x + 1, self.y);
                                }
                                Main.tile[self.x - 2, self.y].liquidType((int)tile5.liquidType());
                                if ((int)Main.tile[self.x - 2, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    Main.tile[self.x - 2, self.y].liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x - 2, self.y);
                                }
                                tile5.liquid = (byte)num2;
                            }
                            else if (flag4)
                            {
                                float num2 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Main.tile[self.x + 2, self.y].liquid + (int)tile5.liquid + num1) / 4.0 + 0.001);
                                tile1.liquidType((int)tile5.liquidType());
                                if ((int)tile1.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    tile1.liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x - 1, self.y);
                                }
                                tile2.liquidType((int)tile5.liquidType());
                                if ((int)tile2.liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    tile2.liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x + 1, self.y);
                                }
                                LiquidCore.grid[self.x + 2, self.y].liquid = (byte)tile5r.liquid;
                                if ((int)Main.tile[self.x + 2, self.y].liquid != (int)(byte)num2 || (int)tile5.liquid != (int)(byte)num2)
                                {
                                    Main.tile[self.x + 2, self.y].liquid = (byte)num2;
                                    AddModdedLiquidAround(self.x + 2, self.y);
                                }
                                tile5.liquid = (byte)num2;
                            }
                            else
                            {
                                float num2 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)tile5.liquid + num1) / 3.0 + 0.001);
                                tile1r.liquid = tile5r.liquid;
                                if ((int)tile1.liquid != (int)(byte)num2)
                                    tile1.liquid = (byte)num2;
                                if ((int)tile5.liquid != (int)(byte)num2 || (int)tile1.liquid != (int)(byte)num2)
                                    AddModdedLiquidAround(self.x - 1, self.y);
                                tile2r.liquid = tile5r.liquid;
                                if ((int)tile2.liquid != (int)(byte)num2)
                                    tile2.liquid = (byte)num2;
                                if ((int)tile5.liquid != (int)(byte)num2 || (int)tile2.liquid != (int)(byte)num2)
                                    AddModdedLiquidAround(self.x + 1, self.y);
                                tile5.liquid = (byte)num2;
                            }
                        }
                        else if (flag1)
                        {
                            float num2 = (float)Math.Round((double)((int)tile1.liquid + (int)tile5.liquid + num1) / 2.0 + 0.001);
                            if ((int)tile1.liquid != (int)(byte)num2)
                                tile1.liquid = (byte)num2;
                            tile1.liquidType((int)tile5.liquidType());
                            if ((int)tile5.liquid != (int)(byte)num2 || (int)tile1.liquid != (int)(byte)num2)
                                AddModdedLiquidAround(self.x - 1, self.y);
                            tile5.liquid = (byte)num2;
                        }
                        else if (flag2)
                        {
                            float num2 = (float)Math.Round((double)((int)tile2.liquid + (int)tile5.liquid + num1) / 2.0 + 0.001);
                            if ((int)tile2.liquid != (int)(byte)num2)
                                tile2.liquid = (byte)num2;
                            tile2.liquidType((int)tile5.liquidType());
                            if ((int)tile5.liquid != (int)(byte)num2 || (int)tile2.liquid != (int)(byte)num2)
                                AddModdedLiquidAround(self.x + 1, self.y);
                            tile5.liquid = (byte)num2;
                        }
                    }
                    if ((int)tile5.liquid == (int)liquid)
                        ++self.kill;
                    else if (tile5.liquid == (byte)254 && liquid == byte.MaxValue)
                    {
                        tile5.liquid = byte.MaxValue;
                        ++self.kill;
                    }
                    else
                    {
                        AddModdedLiquidAround(self.x, self.y - 1);
                        self.kill = 0;
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
            for (int index1 = maxY; index1 >= minY; --index1)
            {
                if (verbose > 0)
                {
                    float num2 = (float)(maxY - index1) / (float)(maxY - minY + 1) / (float)verbose;
                    Main.statusText = Lang.gen[27].Value + " " + (object)(int)((double)num2 * 100.0 + 1.0) + "%";
                }
                else if (verbose < 0)
                {
                    float num2 = (float)(maxY - index1) / (float)(maxY - minY + 1) / -(float)verbose;
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
                    int index3 = num2;
                    while (index3 != num3)
                    {
                        Tile tile = Main.tile[index3, index1];
                        LiquidRef liquids = LiquidCore.grid[index3, index1];
                        if (tile.liquid > (byte)0)
                        {
                            int num5 = -num4;
                            bool flag1 = false;
                            int x = index3;
                            int y = index1;
                            byte num6 = liquids.liquid;
                            byte liquid = liquids.getLiquidLayer();
                            tile.liquid = (byte)0;
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
                                if (Main.tile[x, y + 1].liquid > (byte)0 && Main.tile[x, y + 1].liquid < byte.MaxValue && (int)Main.tile[x, y + 1].liquidType() == (int)num6)
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
                            if (index3 != x && index1 != y)
                                ++num1;

                            LiquidCore.grid[x,y].setLiquidsState(num6, true);
                            LiquidCore.grid[x, y].tile.liquid = liquid;
                        }
                        index3 += num4;
                    }
                }
            }
            return (double)num1;
        }

        public static void AddModdedLiquidAround(int x, int y)
        {
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
                bool flag = false;
                
                WorldGen.KillTile(x, y, false, false, false);
                if (Main.netMode != 2)
                    return;
                NetMessage.SendData(17, -1, -1, (NetworkText)null, 0, (float)x, (float)y, 0.0f, 0, 0, 0);
            }
        }
    }
}
