using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BiomeLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Dimlibs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using TerrariaUltraApocalypse.API;
using TerrariaUltraApocalypse.CustomScreenShader;
using TerrariaUltraApocalypse.Dimension.MicroBiome;
using TerrariaUltraApocalypse.NPCs.NewBiome.Wasteland.MutatedMass;
using TerrariaUltraApocalypse.Structure.hellalt;

namespace TerrariaUltraApocalypse
{
    class TUAWorld : ModWorld
    {
        public static bool apocalypseMoon = false;
        public static bool EoADowned;
        public static bool ApoMoonDowned;
        public static bool UltraMode;
        public static bool HallowAlt;
        public static bool Wasteland;
        public static bool EoCPostML;

        public static int heartX = (Main.maxTilesX / 2) * 16;
        public static int heartY = (Main.maxTilesY - 100) * 16;
        public static int apocalypseMoonPoint = 0;
        public static int EoCDeath = 0;

        public static bool RealisticTimeMode = false;


        public override void Initialize()
        {
            RegisterPlagues();
            RegisterMeteoridon();
            RegisterSolar();
            RegisterStardust();
            RegisterNebula();
            RegisterVortex();
        }

        private void RegisterPlagues()
        {
            BiomeLibs.RegisterNewBiome("Plagues", 50, mod);
            BiomeLibs.AddBlockInBiome("Plagues", new String[] { "ApocalypseDirt" });
        }

        private void RegisterMeteoridon()
        {
            Func<bool> c = () => Main.hardMode;
            BiomeLibs.RegisterNewBiome("Meteoridon", 50, mod);
            BiomeLibs.AddBlockInBiome("Meteoridon", new String[] { "MeteoridonStone", "MeteoridonGrass", "BrownIce" });
            BiomeLibs.addHallowAltBiome("Meteoridon", "The world is getting fiery...");
            BiomeLibs.SetCondition("Meteoridon", c);
        }

        private void RegisterSolar()
        {
            Func<bool> c = () => Dimlibs.Dimlibs.getPlayerDim() == "solar";
            BiomeLibs.RegisterNewBiome("Solar", 200, mod);
            BiomeLibs.AddBlockInBiome("Solar", new String[] { "SolarDirt", "SolarRock" });
            BiomeLibs.SetCondition("solar", c);
        }

        private void RegisterVortex()
        {
            BiomeLibs.RegisterNewBiome("Vortex", 200, mod);
        }

        private void RegisterNebula()
        {
            BiomeLibs.RegisterNewBiome("Nebula", 200, mod);
        }

        private void RegisterStardust()
        {
            Func<bool> c = () => Dimlibs.Dimlibs.getPlayerDim() == "stardust";
            BiomeLibs.RegisterNewBiome("Stardust", 200, mod);
            BiomeLibs.AddBlockInBiome("stardust", new String[] { "StardustRock" });
            BiomeLibs.SetCondition("stardust", c);
        }

        public override TagCompound Save()
        {
            TagCompound tc = new TagCompound();
            tc.Add("UltraMode", UltraMode);
            tc.Add("EoADowned", EoADowned);
            tc.Add("UltraEoCDowned", EoCDeath);
            tc.Add("hellAlt", Wasteland);
            tc.Add("RealisticTimeMode", RealisticTimeMode);
            //tc.Add("apocalypseMoon", apocalypseMoon);
            return tc;
        }

        public override void Load(TagCompound tag)
        {
            UltraMode = tag.GetBool("UltraMode");
            EoADowned = tag.GetBool("EoADowned");
            EoCDeath = tag.GetInt("UltraEoCDowned");
            Wasteland = tag.GetBool("hellAlt");
            RealisticTimeMode = tag.GetBool("RealisticTimeMode");

            if (!Main.ActiveWorldFileData.HasCorruption)
            {
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int hellIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Underworld"));
            int hellForgeIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Hellforge"));
            int guideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Guide"));
            tasks[hellIndex] = new PassLegacy("Underworld", newUnderWorldGen);
            tasks[hellForgeIndex] = new PassLegacy("Hellforge", newForgeGen);
        }


        public override void PostUpdate()
        {
            
            if (Main.netMode != 1)
            {
                for (int k = 0; k < Main.maxTilesX * Main.maxTilesY * 3E-05 * Main.worldRate; k++)
                {
                    int x = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int y = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1);
                    if (Main.tile[x, y] != null && Main.tile[x, y].liquid <= 32 && Main.tile[x, y].nactive())
                    {
                        UpdateTile(x, y);
                    }

                }
                for (int k = 0; k < Main.maxTilesX * Main.maxTilesY * 1.5E-05 * Main.worldRate; k++)
                {
                    int x = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int y = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    if (Main.tile[x, y] != null && Main.tile[x, y].liquid <= 32 && Main.tile[x, y].nactive())
                    {
                        UpdateTile(x, y);
                    }
                }

            }
        }

        private void UpdateTile(int x, int y)
        {
            Tile tile = Main.tile[x, y];

            if (!tile.inActive() && (tile.type == (ushort)mod.TileType("ApocalypseDirt") && WorldGen.genRand.Next(4) == 0))
            {
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    int toX = x + WorldGen.genRand.Next(-3, 4);
                    int toY = y + WorldGen.genRand.Next(-3, 4);
                    bool tileChanged = false;
                    int targetType = Main.tile[toX, toY].type;

                    if (targetType == 0)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("ApocalypseDirt");
                        tileChanged = true;
                    }
                    else if (targetType == 2)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("ApocalypseDirt");
                        tileChanged = true;
                    }

                    if (tileChanged)
                    {
                        if (WorldGen.genRand.Next(1000) == 0)
                        {
                            flag = true;
                        }
                        WorldGen.SquareTileFrame(toX, toY, true);
                        NetMessage.SendTileSquare(-1, toX, toY, 1);
                    }
                }
            }
            else if (!tile.inActive() && (tile.type == (ushort)mod.TileType("MeteoridonGrass") || tile.type == (ushort)mod.TileType("MeteoridonStone") || tile.type == (ushort)mod.TileType("BrownIce") && WorldGen.genRand.Next(4) == 0))
            {
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    int toX = x + WorldGen.genRand.Next(-3, 4);
                    int toY = y + WorldGen.genRand.Next(-3, 4);
                    bool tileChanged = false;
                    int targetType = Main.tile[toX, toY].type;

                    if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("MeteoridonStone");
                        tileChanged = true;
                    }
                    else if (targetType == 2)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("MeteoridonGrass");
                        tileChanged = true;
                    }
                    else if (targetType == 161)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("BrownIce");
                        tileChanged = true;
                    }
                    else if (targetType == 0 && Main.tile[toX, toY - 1] == null)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("MeteoridonGrass");
                        tileChanged = true;
                    }

                    if (tileChanged)
                    {
                        if (WorldGen.genRand.Next(1000) == 0)
                        {
                            flag = true;
                        }
                        WorldGen.SquareTileFrame(toX, toY, true);
                        NetMessage.SendTileSquare(-1, toX, toY, 1);
                    }
                }
            }
        }


        public override void PreUpdate()
        {
            if (!apocalypseMoon)
            {
                Main.moonTexture = TerrariaUltraApocalypse.originalMoon;
            }

            Main.bottomWorld = Main.maxTilesY * 16 + 400;

            if (NPC.CountNPCS(mod.NPCType<HeartOfTheWasteland>()) == 0)
            {
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
            }

            if (RealisticTimeMode)
            {
                RealTime();
            }
        }

        private static void RealTime()
        {
            double currentTimeInSecond = DateTime.Now.TimeOfDay.TotalSeconds;

            if (currentTimeInSecond > 70200)
            {
                Main.dayTime = false;
                currentTimeInSecond -= 70200;
            }

            if (currentTimeInSecond < 16200)
            {
                Main.dayTime = false;
                currentTimeInSecond += 16200;
            }

            if (currentTimeInSecond >= 16200 && currentTimeInSecond <= 70200)
            {
                Main.dayTime = true;
                currentTimeInSecond -= 16200;
            }

            Main.time = currentTimeInSecond;
        }

        public override void PostWorldGen()
        {
            Main.bottomWorld = Main.maxTilesY - 10;
        }

        private static void Volcano(Mod mod, int i, int j)
        {
            int x = i;
            int y = j;

            int volcanoBlock = mod.TileType("SolarRock");

            for (int yPos = j; yPos < j + Main.maxTilesY - 200; yPos += 4)
            {
                Main.tile[i, j].bottomSlope();
                WorldGen.TileRunner(x - 20, y + yPos, 35, 2, volcanoBlock, true, 5, 10, false);
                WorldGen.TileRunner(x + 20, y + yPos, 35, 2, volcanoBlock, true, 5, 10, false);
            }

            int baseY = -16;

            for (int xPos = 16; xPos < 128; xPos += 4)
            {
                for (int yPos = baseY; yPos < 120; yPos += 8)
                {
                    WorldGen.TileRunner(x - xPos, y + yPos, 30, 2, volcanoBlock, true, 5, 10, false);
                    WorldGen.TileRunner(x + xPos, y + yPos, 30, 2, volcanoBlock, true, 5, 10, false);
                }
                baseY += 4;
                if (Main.rand.NextBool())
                {
                    baseY += 4;
                }
            }


            for (int yPos = y - 24; yPos < y + Main.maxTilesY - 400; yPos++)
            {
                int clear = Main.rand.Next(14, 18);
                for (int xPos = x - clear; xPos < x + clear; xPos++)
                {
                    WorldGen.KillTile(xPos, yPos, false, false, true);

                }
            }


            for (int yPos = y + 6; yPos < y + Main.maxTilesY - 400; yPos++)
            {
                int clear = Main.rand.Next(14, 18);
                for (int xPos = x - clear; xPos < x + clear; xPos++)
                {
                    Main.tile[xPos, yPos].lava(true);
                    Main.tile[xPos, yPos].liquid = 255;

                }
            }

            for (int yPos = Main.maxTilesY - 200; yPos < Main.maxTilesY; yPos++)
            {
                for (int xPos = 0; xPos < Main.maxTilesX; xPos++)
                {
                    if (xPos > 0 && xPos < Main.maxTilesX && !Main.tile[xPos, yPos].lava())
                    {
                        Main.tile[xPos, yPos].lava(true);
                        Main.tile[xPos, yPos].liquid = 255;

                    }
                }
            }

            makeCore(x, y + 70, Main.rand.Next(35, 50));

        }

        private static void makeCore(int i, int j, int size)
        {
            int baseX = size - 2;

            for (int yCenter = j; yCenter < j + size; yCenter++)
            {
                for (int xCenter = i - baseX; xCenter <= i; xCenter++)
                {

                    if (xCenter > 0 && xCenter < Main.maxTilesX && yCenter > 0 && yCenter < Main.maxTilesY)
                    {
                        WorldGen.KillTile(xCenter, yCenter);
                        Main.tile[xCenter, yCenter].lava(true);
                        Main.tile[xCenter, yCenter].liquid = 255;
                    }
                }
                for (int xCenter = i + baseX; xCenter >= i; xCenter--)
                {

                    if (xCenter > 0 && xCenter < Main.maxTilesX && yCenter > 0 && yCenter < Main.maxTilesY)
                    {
                        WorldGen.KillTile(xCenter, yCenter);
                        Main.tile[xCenter, yCenter].lava(true);
                        Main.tile[xCenter, yCenter].liquid = 255;
                    }
                }
                if (Main.rand.NextBool())
                {
                    baseX--;
                }
            }

            baseX = size - 2;

            for (int yCenter = j; yCenter > j - size; yCenter--)
            {
                for (int xCenter = i - baseX; xCenter <= i; xCenter++)
                {

                    if (xCenter > 0 && xCenter < Main.maxTilesX && yCenter > 0 && yCenter < Main.maxTilesY)
                    {
                        WorldGen.KillTile(xCenter, yCenter);
                        Main.tile[xCenter, yCenter].lava(true);
                        Main.tile[xCenter, yCenter].liquid = 255;
                    }
                }
                for (int xCenter = i + baseX; xCenter >= i; xCenter--)
                {

                    if (xCenter > 0 && xCenter < Main.maxTilesX && yCenter > 0 && yCenter < Main.maxTilesY)
                    {
                        WorldGen.KillTile(xCenter, yCenter);
                        Main.tile[xCenter, yCenter].lava(true);
                        Main.tile[xCenter, yCenter].liquid = 255;
                    }
                }
                if (Main.rand.NextBool())
                {
                    baseX--;
                }
            }

        }


        public void newUnderWorldGen(GenerationProgress progress)
        {
            
            if (WorldGen.crimson)
            {
                Wasteland = true;
                wastelandGeneration(progress);
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
            }
            else
            {
                Wasteland = false;
                originalHell(progress);
            }
        }

        public void newForgeGen(GenerationProgress progress)
        {
            if (!Wasteland)
            {
                hellForge(progress);
            }
        }

        public void hellForge(GenerationProgress progress)
        {
            progress.Message = Lang.gen[36].Value;
            for (int k = 0; k < Main.maxTilesX / 200; k++)
            {
                float value = (float)(k / (Main.maxTilesX / 200));
                progress.Set(value);
                bool flag2 = false;
                int num = 0;
                while (!flag2)
                {
                    int num2 = WorldGen.genRand.Next(1, Main.maxTilesX);
                    int num3 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 5);
                    try
                    {
                        if (Main.tile[num2, num3].wall != 13)
                        {
                            if (Main.tile[num2, num3].wall != 14)
                            {
                                continue;
                            }
                        }
                        while (!Main.tile[num2, num3].active())
                        {
                            num3++;
                        }
                        num3--;
                        WorldGen.PlaceTile(num2, num3, 77, false, false, -1, 0);
                        if (Main.tile[num2, num3].type == 77)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            num++;
                            if (num >= 10000)
                            {
                                flag2 = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

        }

        public void wastelandGeneration(GenerationProgress progress)
        {
            progress.Message = "Eradiate the underworld";
            progress.Set(0f);

            int maxAmplitude = 15;
            int waveAmplitude = 10;
            int originY = 100;
            bool inverted = false;
            int startingY = 175;

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = Main.maxTilesY - 200; y < Main.maxTilesY; y++)
                {
                    
                    Main.tile[x, y].type = (ushort)mod.TileType("WastelandRock");
                    Main.tile[x, y].liquid = 0;
                    Main.tile[x, y].active(true);
                    Main.tile[x, y].wall = 0;
                    
                    if (y > Main.maxTilesY - startingY)
                    {
                        Main.tile[x, y].active(false);
                        
                    }
                    if (y > Main.maxTilesY - originY + waveAmplitude)
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = (ushort)mod.TileType("WastelandRock");
                    }
                }

                if (WorldGen.genRand.Next(5) == 0)
                {
                    if (!inverted)
                    {
                        waveAmplitude++;
                    }
                    else
                    {
                        waveAmplitude--;
                    }
                }


                if (waveAmplitude == 0)
                {
                    inverted = false;
                }

                if (waveAmplitude == maxAmplitude)
                {
                    inverted = true;
                    if (WorldGen.genRand.NextBool())
                    {
                        maxAmplitude += 3;
                    }
                    else if(maxAmplitude > 10)
                    {
                        maxAmplitude -= 3;
                    }
                }

                int modifyY = WorldGen.genRand.Next(10);
                if (modifyY == 0 && startingY > 160)
                {
                    startingY--;
                }
                else if (modifyY == 2 && startingY < 190)
                {
                    startingY++;
                }
            }
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), mod.TileType("WastelandOre"), false, 0f, 0f, false, true);
            }
            Biomes<TheHeartArena>.Place((int)Main.maxTilesX / 2, (int)Main.maxTilesY - 100, WorldGen.structures);
        }

        public void originalHell(GenerationProgress progress)
        {
            progress.Message = Lang.gen[18].Value;
            progress.Set(0f);
            int num = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
            for (int k = 0; k < Main.maxTilesX; k++)
            {
                num += WorldGen.genRand.Next(-3, 4);
                if (num < Main.maxTilesY - 190)
                {
                    num = Main.maxTilesY - 190;
                }
                if (num > Main.maxTilesY - 160)
                {
                    num = Main.maxTilesY - 160;
                }
                for (int l = num - 20 - WorldGen.genRand.Next(3); l < Main.maxTilesY; l++)
                {
                    if (l >= num)
                    {
                        Main.tile[k, l].active(false);
                        Main.tile[k, l].lava(false);
                        Main.tile[k, l].liquid = 0;
                    }
                    else
                    {
                        Main.tile[k, l].type = 57;
                    }
                }
            }
            int num2 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
            for (int m = 10; m < Main.maxTilesX - 10; m++)
            {
                num2 += WorldGen.genRand.Next(-10, 11);
                if (num2 > Main.maxTilesY - 60)
                {
                    num2 = Main.maxTilesY - 60;
                }
                if (num2 < Main.maxTilesY - 100)
                {
                    num2 = Main.maxTilesY - 120;
                }
                for (int n = num2; n < Main.maxTilesY - 10; n++)
                {
                    if (!Main.tile[m, n].active())
                    {
                        Main.tile[m, n].lava(true);
                        Main.tile[m, n].liquid = 255;
                    }
                }
            }
            for (int num3 = 0; num3 < Main.maxTilesX; num3++)
            {
                if (WorldGen.genRand.Next(50) == 0)
                {
                    int num4 = Main.maxTilesY - 65;
                    while (!Main.tile[num3, num4].active() && num4 > Main.maxTilesY - 135)
                    {
                        num4--;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num4 + WorldGen.genRand.Next(20, 50), (double)WorldGen.genRand.Next(15, 20), 1000, 57, true, 0f, (float)WorldGen.genRand.Next(1, 3), true, true);
                }
            }
            Liquid.QuickWater(-2, -1, -1);
            for (int num5 = 0; num5 < Main.maxTilesX; num5++)
            {
                float num6 = (float)num5 / (float)(Main.maxTilesX - 1);
                progress.Set(num6 / 2f + 0.5f);
                if (WorldGen.genRand.Next(13) == 0)
                {
                    int num7 = Main.maxTilesY - 65;
                    while ((Main.tile[num5, num7].liquid > 0 || Main.tile[num5, num7].active()) && num7 > Main.maxTilesY - 140)
                    {
                        num7--;
                    }
                    WorldGen.TileRunner(num5, num7 - WorldGen.genRand.Next(2, 5), (double)WorldGen.genRand.Next(5, 30), 1000, 57, true, 0f, (float)WorldGen.genRand.Next(1, 3), true, true);
                    float num8 = (float)WorldGen.genRand.Next(1, 3);
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        num8 *= 0.5f;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        WorldGen.TileRunner(num5, num7 - WorldGen.genRand.Next(2, 5), (double)((int)((float)WorldGen.genRand.Next(5, 15) * num8)), (int)((float)WorldGen.genRand.Next(10, 15) * num8), 57, true, 1f, 0.3f, false, true);
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        num8 = (float)WorldGen.genRand.Next(1, 3);
                        WorldGen.TileRunner(num5, num7 - WorldGen.genRand.Next(2, 5), (double)((int)((float)WorldGen.genRand.Next(5, 15) * num8)), (int)((float)WorldGen.genRand.Next(10, 15) * num8), 57, true, -1f, 0.3f, false, true);
                    }
                    WorldGen.TileRunner(num5 + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), (double)WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        WorldGen.TileRunner(num5 + WorldGen.genRand.Next(-10, 10), num7 + WorldGen.genRand.Next(-10, 10), (double)WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    }
                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        WorldGen.TileRunner(num5 + WorldGen.genRand.Next(-15, 15), num7 + WorldGen.genRand.Next(-15, 10), (double)WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, false, (float)WorldGen.genRand.Next(-1, 3), (float)WorldGen.genRand.Next(-1, 3), false, true);
                    }
                }
            }
            for (int num9 = 0; num9 < Main.maxTilesX; num9++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2, false, 0f, 0f, false, true);
            }
            for (int num10 = 0; num10 < Main.maxTilesX; num10++)
            {
                if (!Main.tile[num10, Main.maxTilesY - 145].active())
                {
                    Main.tile[num10, Main.maxTilesY - 145].liquid = 255;
                    Main.tile[num10, Main.maxTilesY - 145].lava(true);
                }
                if (!Main.tile[num10, Main.maxTilesY - 144].active())
                {
                    Main.tile[num10, Main.maxTilesY - 144].liquid = 255;
                    Main.tile[num10, Main.maxTilesY - 144].lava(true);
                }
            }
            for (int num11 = 0; num11 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num11++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), 58, false, 0f, 0f, false, true);
            }
            WorldGen.AddHellHouses();
        }

        /*
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            String text = "";
            Main.hardMode = true;


            for (int i = 0; i < list.Count; i++)
            {
                text += list[i].Name + "\n";
            }

            System.IO.File.WriteAllText(@"C:\TerrariaTag\worldTask.txt", text);


            if (WorldGen.crimson)
            {
                list[0] = (new PassLegacy("Hardmode Good", delegate
                {

                    float num = (float)WorldGen.genRand.Next(300, 400) * 0.001f;
                    float num2 = (float)WorldGen.genRand.Next(200, 300) * 0.001f;
                    int num3 = (int)((float)Main.maxTilesX * num);
                    int num4 = (int)((float)Main.maxTilesX * (1f - num));
                    int num5 = 1;
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        num4 = (int)((float)Main.maxTilesX * num);
                        num3 = (int)((float)Main.maxTilesX * (1f - num));
                        num5 = -1;
                    }
                    int num6 = 1;
                    if (WorldGen.dungeonX < Main.maxTilesX / 2)
                    {
                        num6 = -1;
                    }
                    if (num6 < 0)
                    {
                        if (num4 < num3)
                        {
                            num4 = (int)((float)Main.maxTilesX * num2);
                        }
                        else
                        {
                            num3 = (int)((float)Main.maxTilesX * num2);
                        }
                    }
                    else if (num4 > num3)
                    {
                        num4 = (int)((float)Main.maxTilesX * (1f - num2));
                    }
                    else
                    {
                        num3 = (int)((float)Main.maxTilesX * (1f - num2));
                    }
                    if (WorldGen.crimson)
                    {
                        Main.NewText("A strange thing landed on earth, is it a new form of of life?", 255, 69, 255);
                        MERunner(num3, 0, (float)(3 * num5), 5f);
                    }
                    else
                    {
                        WorldGen.GERunner(num4, 0, 3f * -(float)num5, 5f, false);
                    }
                }));
            }

            text = "";

            for (int i = 0; i < list.Count; i++)
            {
                text += list[i].Name + "\n";
            }

            System.IO.File.WriteAllText(@"C:\TerrariaTag\worldTaskafter.txt", text);
        }
        */

        

        public override void PostDrawTiles()
        {
            if (BiomeLibs.InBiome("Meteoridon") && Main.netMode == 0)
            {
                ScreenFog.Draw(TerrariaUltraApocalypse.SolarFog, 0.3f, 0.1f);
            }
        }
    }
}
