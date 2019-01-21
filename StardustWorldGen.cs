using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace TUA
{
    class StardustWorldGen : Dimlibs.API.DimGenerator
    {
        public static double iceLayer;

        public StardustWorldGen() : base("Stardust")
        {
        }

        public override void ModifyGenerationPass(int seed, GenerationProgress customProgressObject)
        {
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                progress.Message = "Stardust world Gen : Adjusting the world";
                int maxtilesX = 7200;
                int maxtilesY = 2400;

                FieldInfo info = typeof(WorldFileData).GetField("WorldSizeX", BindingFlags.Instance | BindingFlags.Public);
                int get = (int)info.GetValue(Main.ActiveWorldFileData);
                info.SetValue(Main.ActiveWorldFileData, maxtilesX);

                info = typeof(WorldFileData).GetField("WorldSizeY", BindingFlags.Instance | BindingFlags.Public);
                get = (int)info.GetValue(Main.ActiveWorldFileData);
                info.SetValue(Main.ActiveWorldFileData, maxtilesY);

                info = typeof(WorldGen).GetField("lastMaxTilesX",
                    BindingFlags.Static | BindingFlags.NonPublic);
                get = (int)info.GetValue(null);
                info.SetValue(null, maxtilesX);

                info = typeof(WorldGen).GetField("lastMaxTilesY",
                    BindingFlags.Static | BindingFlags.NonPublic);
                get = (int)info.GetValue(null);
                info.SetValue(null, maxtilesY);

                Main.maxTilesX = maxtilesX;
                Main.maxTilesY = maxtilesY;

                Main.rightWorld = maxtilesX * 16;
                Main.bottomWorld = maxtilesY * 16;
                Main.maxSectionsX = Main.maxTilesX / 200;
                Main.maxSectionsY = Main.maxTilesY / 150;

                Main.tile = new Tile[Main.maxTilesX, Main.maxTilesY];

                WorldGen.clearWorld();

                Liquid.ReInit();
                WorldGen.noTileActions = true;
                progress.Message = "";
                WorldGen.SetupStatueList();
                WorldGen.RandomizeWeather();
                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                WorldFile.tempMaxRain = 0f;
                Main.raining = false;
                Main.checkXMas();
                Main.checkHalloween();
                WorldGen.gen = true;
                WorldGen.numLarva = 0;
                int num = 86400;
                Main.slimeRainTime = (double)(-(double)WorldGen.genRand.Next(num * 2, num * 3));
                Main.cloudBGActive = (float)(-(float)WorldGen.genRand.Next(8640, 86400));
                WorldGen.CopperTierOre = 7;
                WorldGen.IronTierOre = 6;
                WorldGen.SilverTierOre = 9;
                WorldGen.GoldTierOre = 8;
                WorldGen.copperBar = 20;
                WorldGen.ironBar = 22;
                WorldGen.silverBar = 21;
                WorldGen.goldBar = 19;
                Main.spawnTileX = Main.maxTilesX / 2;
                if (WorldGen.genRand.Next(2) == 0)
                {
                    copper = 166;
                    WorldGen.copperBar = 703;
                    WorldGen.CopperTierOre = 166;
                }
                if (WorldGen.genRand.Next(2) == 0)
                {
                    iron = 167;
                    WorldGen.ironBar = 704;
                    WorldGen.IronTierOre = 167;
                }
                if (WorldGen.genRand.Next(2) == 0)
                {
                    silver = 168;
                    WorldGen.silverBar = 705;
                    WorldGen.SilverTierOre = 168;

                }
                if (WorldGen.genRand.Next(2) == 0)
                {
                    gold = 169;
                    WorldGen.goldBar = 706;
                    WorldGen.GoldTierOre = 169;
                }
                WorldGen.crimson = (WorldGen.genRand.Next(2) == 0);
                if (WorldGen.WorldGenParam_Evil == 0)
                {
                    WorldGen.crimson = false;
                }
                if (WorldGen.WorldGenParam_Evil == 1)
                {
                    WorldGen.crimson = true;
                }
                if (jungleHut == 0)
                {
                    jungleHut = 119;
                }
                else if (jungleHut == 1)
                {
                    jungleHut = 120;
                }
                else if (jungleHut == 2)
                {
                    jungleHut = 158;
                }
                else if (jungleHut == 3)
                {
                    jungleHut = 175;
                }
                else if (jungleHut == 4)
                {
                    jungleHut = 45;
                }
                Main.worldID = WorldGen.genRand.Next(2147483647);
                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();
                dungeonSide = ((WorldGen.genRand.Next(2) == 0) ? -1 : 1);
            });

            AddGenerationPass("Terrain", initialShaping);
            AddGenerationPass("Carving the mountain", Shaping);
            AddGenerationPass("Shaping the spawn", shapeSpawn);
            AddGenerationPass("Shaping oceam floor", shapingTheFloor);
            AddGenerationPass("Filling the ocean", generateSea);
            AddGenerationPass("Settle liquid", settleLiquid);

            WorldGen.EveryTileFrame();
        }

        internal void initialShaping(GenerationProgress progress)
        {
            progress.Message = "Stardust world Gen : Shaping the land";
            int curveInfluence = 0;
            bool yLevelControl = false;

            worldSurface = (double)Main.maxTilesY * 0.70;
            worldSurface *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
            rockLayer = worldSurface + (double)Main.maxTilesY * 0.80;
            rockLayer *= (double)WorldGen.genRand.Next(150, 180) * 0.01;
            double GlacierHeight = worldSurface + WorldGen.genRand.Next(60, 80);
            double GlacierMiddle = worldSurface;
            double GlacierBottom = worldSurface - WorldGen.genRand.Next(30, 40);
            bool GlacierDown = false;
            WorldGen.worldSurfaceLow = worldSurface;
            worldSurfaceHigh = worldSurface;
            rockLayerLow = rockLayer;
            rockLayerHigh = rockLayer;

            for (int currentX = 0; currentX < Main.maxTilesX; currentX++)
            {

                if (rockLayer < rockLayerLow)
                {
                    rockLayerLow = rockLayer;
                }
                if (rockLayer > rockLayerHigh)
                {
                    rockLayerHigh = rockLayer;
                }


                if (currentX > 275 && currentX < Main.maxTilesX * 0.40)
                {

                    while (WorldGen.genRand.Next(0, 2) == 1)
                    {
                        worldSurface -= 1.0;
                    }
                }

                if ((currentX < Main.maxTilesX * 0.48 && currentX > Main.maxTilesX * 0.40))
                {
                    worldSurface -= WorldGen.genRand.Next(0, 1);
                }

                if ((currentX < Main.maxTilesX * 0.52 && currentX > Main.maxTilesX * 0.48))
                {
                    while (WorldGen.genRand.Next(0, 7) == 1)
                    {
                        worldSurface += WorldGen.genRand.Next(-2, 2);
                    }
                }


                while (WorldGen.genRand.Next(0, 3) == 1)
                {
                    rockLayer += (double)WorldGen.genRand.Next(-2, 1);
                }
                if (rockLayer < worldSurface + (double)Main.maxTilesY * 0.5)
                {
                    rockLayer += 1.0;
                }
                if (rockLayer > worldSurface + (double)Main.maxTilesY * 0.65)
                {
                    rockLayer -= 1.0;
                }

                if (worldSurface < (double)Main.maxTilesY * 0.21)
                {
                    worldSurface = (double)Main.maxTilesY * 0.21;
                }
                else if (worldSurface > (double)Main.maxTilesY * 0.4)
                {
                    worldSurface = (double)Main.maxTilesY * 0.4;
                }
                if ((currentX < 275 || currentX > Main.maxTilesX - 275) && worldSurface > (double)Main.maxTilesY * 0.14)
                {
                    worldSurface = (double)Main.maxTilesY * 0.14;
                }
                int yMin = 0;
                while ((double)yMin < worldSurface)
                {
                    Main.tile[currentX, yMin].active(false);
                    Main.tile[currentX, yMin].frameX = -1;
                    Main.tile[currentX, yMin].frameY = -1;
                    yMin++;
                }
                for (int currentY = (int)worldSurface; currentY < Main.maxTilesY; currentY++)
                {
                    if ((double)currentY < rockLayer)
                    {
                        Main.tile[currentX, currentY].active(true);
                        Main.tile[currentX, currentY].type = (ushort)ModLoader.GetMod("TUA").TileType("StardustIce");
                        Main.tile[currentX, currentY].frameX = -1;
                        Main.tile[currentX, currentY].frameY = -1;
                    }
                    else
                    {
                        Main.tile[currentX, currentY].active(true);
                        Main.tile[currentX, currentY].type = (ushort)ModLoader.GetMod("TUA").TileType("StardustRock");
                        Main.tile[currentX, currentY].frameX = -1;
                        Main.tile[currentX, currentY].frameY = -1;
                    }
                }
            }
            worldSurface = (double)Main.maxTilesY * 0.60;
            worldSurface *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
            Main.worldSurface = worldSurfaceHigh + 25.0;
            Main.rockLayer = rockLayerHigh;
            double num4 = (double)((int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6);
            Main.rockLayer = Main.worldSurface + num4;
            WorldGen.waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2;
            WorldGen.waterLine += WorldGen.genRand.Next(-100, 20);
            WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next(50, 80);
        }


        internal void Shaping(GenerationProgress progress)
        {
            progress.Message = "Stardust world Gen : Carving the ocean";
            int yLevel = (int)(Main.maxTilesY * 0.14);
            int yLimit = (int)(Main.maxTilesY * 0.19);
            

            double GlacierHeight = worldSurface + WorldGen.genRand.Next(60, 80);
            double GlacierMiddle = worldSurface;
            double GlacierBottom = worldSurface - WorldGen.genRand.Next(30, 40);

            for (int currentX = 0; currentX < Main.maxTilesX; currentX++)
            {
                //Dig the first side of ocean and left side of the the spawn platform
                if ((currentX > 275 && currentX < Main.maxTilesX * 0.15) ||
                    (currentX < Main.maxTilesX * 0.70 && currentX > Main.maxTilesX * 0.54))
                {

                    while (WorldGen.genRand.Next(0, 2) == 1)
                    {
                        yLevel++;
                    }
                }

                //Then come back to spawn platform and other side of ocean #2
                if ((currentX < Main.maxTilesX * 0.46 && currentX > Main.maxTilesX * 0.35))
                {
                    while (WorldGen.genRand.Next(0, 2) == 1)
                        yLevel--;
                }

                if ((currentX < Main.maxTilesX * 80 && currentX > Main.maxTilesX * 0.90))
                {
                    while (WorldGen.genRand.Next(0, 2) == 1)
                        yLevel--;
                }

                if ((currentX < Main.maxTilesX * 0.54 && currentX > Main.maxTilesX * 0.46))
                {
                    while (WorldGen.genRand.Next(0, 7) == 1)
                    {
                        yLevel += WorldGen.genRand.Next(-2, 2);
                    }

                    if (yLevel > yLimit)
                    {
                        yLevel -= 2;
                    }

                }

                

                if (yLevel < (double)Main.maxTilesY * 0.17)
                {
                    yLevel = (int)(Main.maxTilesY * 0.17);

                }
                else if (yLevel > (double)Main.maxTilesY * 0.4)
                {
                    yLevel = (int)(Main.maxTilesY * 0.4);
                }

                int yMin = 0;
                while ((double)yMin < yLevel)
                {

                    Main.tile[currentX, yMin].active(false);
                    Main.tile[currentX, yMin].frameX = -1;
                    Main.tile[currentX, yMin].frameY = -1;
                    yMin++;
                }
                for (int currentY = yLevel; currentY < Main.maxTilesY; currentY++)
                {
                    if ((double)currentY < rockLayer)
                    {
                        Main.tile[currentX, currentY].active(true);
                        Main.tile[currentX, currentY].type = (ushort)ModLoader.GetMod("TUA").TileType("StardustIce");
                        Main.tile[currentX, currentY].frameX = -1;
                        Main.tile[currentX, currentY].frameY = -1;
                    }
                    else
                    {
                        Main.tile[currentX, currentY].active(true);
                        Main.tile[currentX, currentY].type = (ushort)ModLoader.GetMod("TUA").TileType("StardustRock");
                        Main.tile[currentX, currentY].frameX = -1;
                        Main.tile[currentX, currentY].frameY = -1;
                    }
                }
            }
        }

        internal void shapeSpawn(GenerationProgress progress)
        {
            progress.Message = "Stardust world Gen : Shaping the spawn point";
            //start at 3888 and end at 3312
            int controlX = 0;
            bool halfWay = false;
            double seaLevel = Main.maxTilesY * 0.17;
            int yLimit = (int) (Main.maxTilesY * 0.17);
            Vector2 centerControl = new Vector2(Main.maxTilesX / 2, 0);
            Vector2 currentXVector;

            for (int currentX = (int) (Main.maxTilesX * 0.70); currentX > (int) (Main.maxTilesX * 0.35); currentX--)
            {
                halfWay = currentX > Main.maxTilesX / 2;
                currentXVector = new Vector2(currentX, 0);
                if (Vector2.Distance(currentXVector, centerControl) > 250)
                {
                    controlX = WorldGen.genRand.Next(0, 4);

                    if (Vector2.Distance(currentXVector, centerControl) > 100 && WorldGen.genRand.Next(0, 4) == 0)
                    {
                        controlX = WorldGen.genRand.Next(0, 2);
                    }
                }
                else
                {
                    controlX = 0;
                }

                if (!halfWay)
                {
                    if (controlX == 0)
                    {
                        while (WorldGen.genRand.Next(0, 3) == 1)
                            yLimit -= WorldGen.genRand.Next(0, 4); ;
                    }

                    if (controlX == 1)
                    {
                        while (WorldGen.genRand.Next(0, 8) == 1)
                            yLimit--;
                    }

                    if (controlX == 2)
                    {
                        while (WorldGen.genRand.Next(0, 9) == 1)
                            yLimit--;
                    }
                }
                else
                {
                    if (controlX == 0)
                    {
                        while (WorldGen.genRand.Next(0, 3) == 1)
                            yLimit += WorldGen.genRand.Next(0, 2);
                    }
                    if (controlX == 1)
                    {
                        while (WorldGen.genRand.Next(0, 7) == 1)
                            yLimit++;
                    }
                    if (controlX == 2)
                    {
                        while (WorldGen.genRand.Next(0, 8) == 1)
                            yLimit--;
                    }
                }


                if (yLimit < seaLevel)
                {
                    yLimit = (int) seaLevel;
                }

                int startingY = (int) (Main.maxTilesY * 0.4);
                // starting Y = 912, objective make it go at Y limit
                // 

                for (int currentY = startingY; currentY > yLimit; currentY--)
                {
                    
                    if (Main.tile[currentX, currentY].active())
                    {
                        Main.tile[currentX, currentY].active(false);
                        Main.tile[currentX, currentY].frameX = -1;
                        Main.tile[currentX, currentY].frameY = -1;
                    }
                }


            }
        }

        internal void shapingTheFloor(GenerationProgress progress)
        {
            progress.Message = "Stardust world Gen : Shaping the ocean floor";
            double currentLevel = Main.maxTilesY * 0.4;
            double seaLevel = Main.maxTilesY * 0.4;

            for (int currentX = (int) (Main.maxTilesX * 0.15); currentX < Main.maxTilesX * 0.90; currentX++)
            {
                int yControl = WorldGen.genRand.Next(0, 5);
                if (yControl == 1)
                {
                    currentLevel += 1.0;
                } else if (yControl == 2)
                {
                    currentLevel -= 1.0;
                }

                int yModification = (int)(seaLevel - currentLevel);
                if (yModification <= 0)
                {
                    
                    for (int currentY = (int) seaLevel; currentY > seaLevel + yModification; currentY--)
                    {
                        /*Main.tile[currentX, currentY].type =
                            (ushort)TUA.instance.TileType("StardustIce");*/
                        WorldGen.PlaceTile(currentX, currentY,
                            TerrariaUltraApocalypse.instance.TileType("StardustIce"));
                    }

                }
                if (yModification > 0)
                {
                    for (int currentY = (int) seaLevel; currentY < seaLevel + yModification; currentY++)
                    {
                        Main.tile[currentX, currentY].active(false);
                    }
                }
            }
        }


        internal void freezingTheSpawn(GenerationProgress progress)
        {
            

        }

        internal void generateSea(GenerationProgress progress)
        {
            progress.Message = "Stardust world Gen : Flooding the world!";
            for (int currentX = 0; currentX < Main.maxTilesX; currentX++)
            {
                for (int currentY = (int) (Main.maxTilesY * 0.4) + 45; currentY > Main.maxTilesY * 0.17; currentY--)
                {

                    if (!Main.tile[currentX, currentY].active())
                    {
                        Main.tile[currentX, currentY].liquid = 255;
                    }
                }
            }
        }

        internal void settleLiquid (GenerationProgress progress)
        {
            progress.Message = Lang.gen[27].Value;
            Liquid.QuickWater(3, -1, -1);
            WorldGen.WaterCheck();
            int k = 0;
            Liquid.quickSettle = true;
            while (k< 10)
            {
                int num = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                k++;
                float num2 = 0f;
                while (Liquid.numLiquid > 0)
                {
                    float num3 = (float)(num - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num;
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num)
                    {
                        num = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    }
                    if (num3 > num2)
                    {
                        num2 = num3;
                    }
                    else
                    {
                        num3 = num2;
                    }
                    if (k == 1)
                    {
                        progress.Set(num3 / 3f + 0.33f);
                    }
                    int num4 = 10;
                    if (k > num4)
                    {
                    }
                    Liquid.UpdateLiquid();
                }
                WorldGen.WaterCheck();
                progress.Set((float) k * 0.1f / 3f + 0.66f);
            }
            Liquid.quickSettle = false;
            Main.tileSolid[190] = true;
        }


    }
}
