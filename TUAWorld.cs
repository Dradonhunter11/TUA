using BiomeLibrary.API;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using TUA.CustomScreenShader;
using TUA.NPCs.NewBiome.Wasteland.MutatedMass;
using TUA.Structure.hellalt;

namespace TUA
{
    class TUAWorld : ModWorld
    {
        public static bool apocalypseMoon = false;
        public static bool EoADowned;
        public static bool ApoMoonDowned;
        public static bool UltraMode;
        public static bool Wasteland;
        public static bool EoCPostML;

        public static int apocalypseMoonPoint = 0;
        public static int EoCDeath = 0;

        public static bool RealisticTimeMode = false;

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "UltraMode", UltraMode },
                { "EoADowned", EoADowned },
                { "UltraEoCDowned", EoCDeath },
                { "hellAlt", Wasteland },
                { "RealisticTimeMode", RealisticTimeMode }
            };
            //tc.Add("apocalypseMoon", apocalypseMoon);
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
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16 + 444, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
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

        public override void PreUpdate()
        {
            Main.bottomWorld = Main.maxTilesY * 16 + 400;

            if (NPC.CountNPCS(mod.NPCType<HeartOfTheWasteland>()) == 0 && Main.ActiveWorldFileData.HasCrimson)
            {
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16 + 444, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
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


        public void newUnderWorldGen(GenerationProgress progress)
        {
            
            if (WorldGen.crimson)
            {
                Wasteland = true;
                wastelandGeneration(progress);
                NPC.NewNPC((Main.maxTilesX / 2) * 16, (Main.maxTilesY - 100) * 16 - 444, mod.NPCType("HeartOfTheWasteland"), 0, 0f, 0f, 0f, 0f, 255);
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
            bool generateLiquid = false;
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
                if (WorldGen.genRand.Next(50) == 0)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), mod.TileType("WastelandOre"), false, 0f, 0f, false, true);
                }
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
            string text = "";
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
            if (mod.GetBiome("Meteoridon").InBiome() && Main.netMode == 0)
            {
                ScreenFog.Draw(TerrariaUltraApocalypse.SolarFog, 0.3f, 0.1f);
            }
        }
    }
}
