using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.World.Generation;

namespace TUA
{
	public class SolarWorldGen : Dimlibs.API.DimGenerator
	{
	    public SolarWorldGen(string dimensionName) : base("Solar")
	    {
	    }

        public static bool GetMudwall()
		{
			FieldInfo info = typeof(WorldGen).GetField("mudWall", BindingFlags.Static | BindingFlags.NonPublic);
			return (bool)info.GetValue(null);
		}

		public override void ModifyGenerationPass(int seed, GenerationProgress customProgressObject)
        {
            

            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                int maxtilesX = 8400;
                int maxtilesY = 1800;

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


                //Main.tile = new Tile[Main.maxTilesX, Main.maxTilesY];

                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Main.tile[x, y] = new Tile();
                        
                    }
                }

                Main.Map = new Terraria.Map.WorldMap(maxtilesX, maxtilesY);

                int mapSizeX = maxtilesX / Main.textureMaxWidth + 1;
                int mapSizeY = maxtilesY / Main.textureMaxHeight + 1;

                Main.mapTargetX = mapSizeX;
                Main.mapTargetY = mapSizeY;

                Main.instance.mapTarget = new Microsoft.Xna.Framework.Graphics.RenderTarget2D[mapSizeX, mapSizeY];
                Main.initMap = new bool[mapSizeX, mapSizeY];
                Main.mapWasContentLost = new bool[mapSizeX, mapSizeY];

                MethodInfo methodInfo = typeof(Main).GetMethod("InitMap", BindingFlags.NonPublic | BindingFlags.Instance);
                methodInfo.Invoke(Main.instance, null);

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
            AddGenerationPass("Terrain", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Terrain";
                int num = 0;
                int num2 = 0;
                worldSurface = (double)Main.maxTilesY * 0.3;
                worldSurface *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
                rockLayer = worldSurface + (double)Main.maxTilesY * 0.2;
                rockLayer *= (double)WorldGen.genRand.Next(90, 110) * 0.01;
                WorldGen.worldSurfaceLow = worldSurface;
                worldSurfaceHigh = worldSurface;
                rockLayerLow = rockLayer;
                rockLayerHigh = rockLayer;
                for (int k = 0; k < Main.maxTilesX; k++)
                {
                    float value = (float)k / (float)Main.maxTilesX;
                    progress.Set(value);
                    if (worldSurface < WorldGen.worldSurfaceLow)
                    {
                        WorldGen.worldSurfaceLow = worldSurface;
                    }
                    if (worldSurface > worldSurfaceHigh)
                    {
                        worldSurfaceHigh = worldSurface;
                    }
                    if (rockLayer < rockLayerLow)
                    {
                        rockLayerLow = rockLayer;
                    }
                    if (rockLayer > rockLayerHigh)
                    {
                        rockLayerHigh = rockLayer;
                    }
                    if (num2 <= 0)
                    {
                        num = WorldGen.genRand.Next(0, 5);
                        num2 = WorldGen.genRand.Next(5, 40);
                        if (num == 0)
                        {
                            num2 *= (int)((double)WorldGen.genRand.Next(5, 30) * 0.2);
                        }
                    }
                    num2--;
                    if ((double)k > (double)Main.maxTilesX * 0.43 && (double)k < (double)Main.maxTilesX * 0.57 && num >= 3)
                    {
                        num = WorldGen.genRand.Next(3);
                    }
                    if ((double)k > (double)Main.maxTilesX * 0.47 && (double)k < (double)Main.maxTilesX * 0.53)
                    {
                        num = 0;
                    }
                    if (num == 0)
                    {
                        while (WorldGen.genRand.Next(0, 7) == 0)
                        {
                            worldSurface += (double)WorldGen.genRand.Next(-1, 2);
                        }
                    }
                    else if (num == 1)
                    {
                        while (WorldGen.genRand.Next(0, 4) == 0)
                        {
                            worldSurface -= 1.0;
                        }
                        while (WorldGen.genRand.Next(0, 10) == 0)
                        {
                            worldSurface += 1.0;
                        }
                    }
                    else if (num == 2)
                    {
                        while (WorldGen.genRand.Next(0, 4) == 0)
                        {
                            worldSurface += 1.0;
                        }
                        while (WorldGen.genRand.Next(0, 10) == 0)
                        {
                            worldSurface -= 1.0;
                        }
                    }
                    else if (num == 3)
                    {
                        while (WorldGen.genRand.Next(0, 2) == 0)
                        {
                            worldSurface -= 1.0;
                        }
                        while (WorldGen.genRand.Next(0, 6) == 0)
                        {
                            worldSurface += 1.0;
                        }
                    }
                    else if (num == 4)
                    {
                        while (WorldGen.genRand.Next(0, 2) == 0)
                        {
                            worldSurface += 1.0;
                        }
                        while (WorldGen.genRand.Next(0, 5) == 0)
                        {
                            worldSurface -= 1.0;
                        }
                    }
                    if (worldSurface < (double)Main.maxTilesY * 0.17)
                    {
                        worldSurface = (double)Main.maxTilesY * 0.17;
                        num2 = 0;
                    }
                    else if (worldSurface > (double)Main.maxTilesY * 0.3)
                    {
                        worldSurface = (double)Main.maxTilesY * 0.3;
                        num2 = 0;
                    }
                    if ((k < 275 || k > Main.maxTilesX - 275) && worldSurface > (double)Main.maxTilesY * 0.25)
                    {
                        worldSurface = (double)Main.maxTilesY * 0.25;
                        num2 = 1;
                    }
                    while (WorldGen.genRand.Next(0, 3) == 0)
                    {
                        rockLayer += (double)WorldGen.genRand.Next(-2, 3);
                    }
                    if (rockLayer < worldSurface + (double)Main.maxTilesY * 0.05)
                    {
                        rockLayer += 1.0;
                    }
                    if (rockLayer > worldSurface + (double)Main.maxTilesY * 0.35)
                    {
                        rockLayer -= 1.0;
                    }
                    int num3 = 0;
                    while ((double)num3 < worldSurface)
                    {
                        Main.tile[k, num3].active(false);
                        Main.tile[k, num3].frameX = -1;
                        Main.tile[k, num3].frameY = -1;
                        num3++;
                    }
                    for (int l = (int)worldSurface; l < Main.maxTilesY; l++)
                    {
                        if ((double)l < rockLayer)
                        {
                            Main.tile[k, l].active(true);
                            Main.tile[k, l].type = (ushort)ModLoader.GetMod("TUA").TileType("SolarDirt");
                            Main.tile[k, l].frameX = -1;
                            Main.tile[k, l].frameY = -1;
                        }
                        else
                        {
                            Main.tile[k, l].active(true);
                            Main.tile[k, l].type = (ushort)ModLoader.GetMod("TUA").TileType("SolarRock");
                            Main.tile[k, l].frameX = -1;
                            Main.tile[k, l].frameY = -1;
                        }
                    }
                }
                Main.worldSurface = worldSurfaceHigh + 25.0;
                Main.rockLayer = rockLayerHigh;
                double num4 = (double)((int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6);
                Main.rockLayer = Main.worldSurface + num4;
                WorldGen.waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2;
                WorldGen.waterLine += WorldGen.genRand.Next(-100, 20);
                WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next(50, 80);
            });
            AddGenerationPass("Tunnels", delegate (GenerationProgress progress)
            {
                for (int k = 0; k < (int)((double)Main.maxTilesX * 0.0015); k++)
                {
                    int[] array = new int[10];
                    int[] array2 = new int[10];
                    int num = WorldGen.genRand.Next(450, Main.maxTilesX - 450);
                    while ((float)num > (float)Main.maxTilesX * 0.45f && (float)num < (float)Main.maxTilesX * 0.55f)
                    {
                        num = WorldGen.genRand.Next(0, Main.maxTilesX);
                    }
                    int num2 = 0;
                    for (int l = 0; l < 10; l++)
                    {
                        num %= Main.maxTilesX;
                        while (!Main.tile[num, num2].active())
                        {
                            num2++;
                        }
                        array[l] = num;
                        array2[l] = num2 - WorldGen.genRand.Next(11, 16);
                        num += WorldGen.genRand.Next(5, 11);
                    }
                    for (int m = 0; m < 10; m++)
                    {
                        SolarWorldGen.TileRunner(array[m], array2[m], (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), ModLoader.GetMod("TUA").TileType("SolarDirt"), true, -2f, -0.3f, false, true);
                        SolarWorldGen.TileRunner(array[m], array2[m], (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), ModLoader.GetMod("TUA").TileType("SolarDirt"), true, 2f, -0.3f, false, true);
                    }
                }
            });
            AddGenerationPass("Dirt Wall Backgrounds", delegate (GenerationProgress progress)
            {
                progress.Message = Lang.gen[3].Value;
                for (int k = 1; k < Main.maxTilesX - 1; k++)
                {
                    byte wall = 2;
                    float value = (float)k / (float)Main.maxTilesX;
                    progress.Set(value);
                    bool flag2 = false;
                    howFar += WorldGen.genRand.Next(-1, 2);
                    if (howFar < 0)
                    {
                        howFar = 0;
                    }
                    if (howFar > 10)
                    {
                        howFar = 10;
                    }
                    int num = 0;
                    while ((double)num < Main.worldSurface + 10.0 && (double)num <= Main.worldSurface + (double)howFar)
                    {
                        if (Main.tile[k, num].active())
                        {
                            if (Main.tile[k, num].type == 147)
                            {
                                wall = 40;
                            }
                            else
                            {
                                wall = 2;
                            }
                        }
                        if (flag2 && Main.tile[k, num].wall != 64)
                        {
                            Main.tile[k, num].wall = wall;
                        }
                        if (Main.tile[k, num].active() && Main.tile[k - 1, num].active() && Main.tile[k + 1, num].active() && Main.tile[k, num + 1].active() && Main.tile[k - 1, num + 1].active() && Main.tile[k + 1, num + 1].active())
                        {
                            flag2 = true;
                        }
                        num++;
                    }
                }
            });


            AddGenerationPass("Rocks In Dirt", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Placing dirt in rock";
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00015); k++)
                {
                    SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)WorldGen.worldSurfaceLow + 1), (double)WorldGen.genRand.Next(4, 15), WorldGen.genRand.Next(5, 40), ModLoader.GetMod("TUA").TileType("SolarRock"), false, 0f, 0f, false, true);
                }
                for (int l = 0; l < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0002); l++)
                {
                    int num = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int num2 = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)worldSurfaceHigh + 1);
                    if (!Main.tile[num, num2 - 10].active())
                    {
                        num2 = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)worldSurfaceHigh + 1);
                    }
                    SolarWorldGen.TileRunner(num, num2, (double)WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), ModLoader.GetMod("TUA").TileType("SolarRock"), false, 0f, 0f, false, true);
                }
                for (int m = 0; m < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0045); m++)
                {
                    SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)worldSurfaceHigh, (int)rockLayerHigh + 1), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 23), ModLoader.GetMod("TUA").TileType("SolarRock"), false, 0f, 0f, false, true);
                }
            });
            AddGenerationPass("Dirt In Rocks", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Placing dity in rock";
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.005); k++)
                {
                    SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)rockLayerLow, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), ModLoader.GetMod("TUA").TileType("SolarDirt"), false, 0f, 0f, false, true);
                }
            });

            int i2;
            AddGenerationPass("Small Holes", delegate (GenerationProgress progress)
            {
                i2 = 0;
                progress.Message = "Solar world Gen : Digging world";
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015); k++)
                {
                    float value = (float)((double)k / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015));
                    progress.Set(value);
                    int type = -1;
                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        type = -2;
                    }
                    SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)worldSurfaceHigh, Main.maxTilesY), (double)WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(2, 20), type, false, 0f, 0f, false, true);
                    SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)worldSurfaceHigh, Main.maxTilesY), (double)WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(7, 30), type, false, 0f, 0f, false, true);
                }
            });
            AddGenerationPass("Dirt Layer Caves", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Make a cave layer";
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05); k++)
                {
                    float value = (float)((double)k / ((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05));
                    progress.Set(value);
                    if (rockLayerHigh <= (double)Main.maxTilesY)
                    {
                        int type = -1;
                        if (WorldGen.genRand.Next(6) == 0)
                        {
                            type = -2;
                        }
                        SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)rockLayerHigh + 1), (double)WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(30, 200), type, false, 0f, 0f, false, true);
                    }
                }
            });
            AddGenerationPass("Rock Layer Caves", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Fill the cave with rock";
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013); k++)
                {
                    float value = (float)((double)k / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013));
                    progress.Set(value);
                    if (rockLayerHigh <= (double)Main.maxTilesY)
                    {
                        int type = -1;
                        if (WorldGen.genRand.Next(10) == 0)
                        {
                            type = -2;
                        }
                        SolarWorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)rockLayerHigh, Main.maxTilesY), (double)WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(50, 300), type, false, 0f, 0f, false, true);
                    }
                }
            });
            AddGenerationPass("Surface Caves", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : Scorching the surface";
                for (int k = 0; k < (int)((double)Main.maxTilesX * 0.002); k++)
                {
                    i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    while ((float)i2 > (float)Main.maxTilesX * 0.45f && (float)i2 < (float)Main.maxTilesX * 0.55f)
                    {
                        i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    }
                    int num = 0;
                    while ((double)num < worldSurfaceHigh)
                    {
                        if (Main.tile[i2, num].active())
                        {
                            SolarWorldGen.TileRunner(i2, num, (double)WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 1f, false, true);
                            break;
                        }
                        num++;
                    }
                }
                for (int l = 0; l < (int)((double)Main.maxTilesX * 0.0007); l++)
                {
                    i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    while ((float)i2 > (float)Main.maxTilesX * 0.43f && (float)i2 < (float)Main.maxTilesX * 0.57f)
                    {
                        i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    }
                    int num2 = 0;
                    while ((double)num2 < worldSurfaceHigh)
                    {
                        if (Main.tile[i2, num2].active())
                        {
                            SolarWorldGen.TileRunner(i2, num2, (double)WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                            break;
                        }
                        num2++;
                    }
                }
                for (int m = 0; m < (int)((double)Main.maxTilesX * 0.0003); m++)
                {
                    i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    while ((float)i2 > (float)Main.maxTilesX * 0.4f && (float)i2 < (float)Main.maxTilesX * 0.6f)
                    {
                        i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    }
                    int num3 = 0;
                    while ((double)num3 < worldSurfaceHigh)
                    {
                        if (Main.tile[i2, num3].active())
                        {
                            SolarWorldGen.TileRunner(i2, num3, (double)WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 4f, false, true);
                            SolarWorldGen.TileRunner(i2, num3, (double)WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                            SolarWorldGen.TileRunner(i2, num3, (double)WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                            break;
                        }
                        num3++;
                    }
                }
                for (int n = 0; n < (int)((double)Main.maxTilesX * 0.0004); n++)
                {
                    i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    while ((float)i2 > (float)Main.maxTilesX * 0.4f && (float)i2 < (float)Main.maxTilesX * 0.6f)
                    {
                        i2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    }
                    int num4 = 0;
                    while ((double)num4 < worldSurfaceHigh)
                    {
                        if (Main.tile[i2, num4].active())
                        {
                            SolarWorldGen.TileRunner(i2, num4, (double)WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, false, 0f, 1f, true, true);
                            break;
                        }
                        num4++;
                    }
                }
                float num5 = (float)(Main.maxTilesX / 4200);
                int num6 = 0;
                while ((float)num6 < 5f * num5)
                {
                    try
                    {
                        SolarWorldGen.Caverer(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 400));
                    }
                    catch
                    {
                    }
                    num6++;
                }
            });
            /*AddGenerationPass("Grass", delegate (GenerationProgress progress)
            {
                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.002); k++)
                {
                    int num = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
                    int num2 = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)worldSurfaceHigh);
                    if (num2 >= Main.maxTilesY)
                    {
                        num2 = Main.maxTilesY - 2;
                    }
                    if (Main.tile[num - 1, num2].active() && Main.tile[num - 1, num2].type == 0 && Main.tile[num + 1, num2].active() && Main.tile[num + 1, num2].type == 0 && Main.tile[num, num2 - 1].active() && Main.tile[num, num2 - 1].type == 0 && Main.tile[num, num2 + 1].active() && Main.tile[num, num2 + 1].type == 0)
                    {
                        Main.tile[num, num2].active(true);
                        Main.tile[num, num2].type = 2;
                    }
                    num = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
                    num2 = WorldGen.genRand.Next(0, (int)WorldGen.worldSurfaceLow);
                    if (num2 >= Main.maxTilesY)
                    {
                        num2 = Main.maxTilesY - 2;
                    }
                    if (Main.tile[num - 1, num2].active() && Main.tile[num - 1, num2].type == 0 && Main.tile[num + 1, num2].active() && Main.tile[num + 1, num2].type == 0 && Main.tile[num, num2 - 1].active() && Main.tile[num, num2 - 1].type == 0 && Main.tile[num, num2 + 1].active() && Main.tile[num, num2 + 1].type == 0)
                    {
                        Main.tile[num, num2].active(true);
                        Main.tile[num, num2].type = 2;
                    }
                }
            });*/

            // TODO: Your gen passes here

            AddGenerationPass("Smooth World", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : " + Lang.gen[60].Value;
                for (int k = 20; k < Main.maxTilesX - 20; k++)
                {
                    float value = (float)k / (float)Main.maxTilesX;
                    progress.Set(value);
                    for (int l = 20; l < Main.maxTilesY - 20; l++)
                    {
                        if (Main.tile[k, l].type != 48 && Main.tile[k, l].type != 137 && Main.tile[k, l].type != 232 && Main.tile[k, l].type != 191 && Main.tile[k, l].type != 151 && Main.tile[k, l].type != 274)
                        {
                            if (!Main.tile[k, l - 1].active())
                            {
                                if (WorldGen.SolidTile(k, l) && TileID.Sets.CanBeClearedDuringGeneration[(int)Main.tile[k, l].type])
                                {
                                    if (!Main.tile[k - 1, l].halfBrick() && !Main.tile[k + 1, l].halfBrick() && Main.tile[k - 1, l].slope() == 0 && Main.tile[k + 1, l].slope() == 0)
                                    {
                                        if (WorldGen.SolidTile(k, l + 1))
                                        {
                                            if (!WorldGen.SolidTile(k - 1, l) && !Main.tile[k - 1, l + 1].halfBrick() && WorldGen.SolidTile(k - 1, l + 1) && WorldGen.SolidTile(k + 1, l) && !Main.tile[k + 1, l - 1].active())
                                            {
                                                if (WorldGen.genRand.Next(2) == 0)
                                                {
                                                    WorldGen.SlopeTile(k, l, 2);
                                                }
                                                else
                                                {
                                                    WorldGen.PoundTile(k, l);
                                                }
                                            }
                                            else if (!WorldGen.SolidTile(k + 1, l) && !Main.tile[k + 1, l + 1].halfBrick() && WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l) && !Main.tile[k - 1, l - 1].active())
                                            {
                                                if (WorldGen.genRand.Next(2) == 0)
                                                {
                                                    WorldGen.SlopeTile(k, l, 1);
                                                }
                                                else
                                                {
                                                    WorldGen.PoundTile(k, l);
                                                }
                                            }
                                            else if (WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l + 1) && !Main.tile[k + 1, l].active() && !Main.tile[k - 1, l].active())
                                            {
                                                WorldGen.PoundTile(k, l);
                                            }
                                            if (WorldGen.SolidTile(k, l))
                                            {
                                                if (WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k + 1, l + 2) && !Main.tile[k + 1, l].active() && !Main.tile[k + 1, l + 1].active() && !Main.tile[k - 1, l - 1].active())
                                                {
                                                    WorldGen.KillTile(k, l, false, false, false);
                                                }
                                                else if (WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k - 1, l + 2) && !Main.tile[k - 1, l].active() && !Main.tile[k - 1, l + 1].active() && !Main.tile[k + 1, l - 1].active())
                                                {
                                                    WorldGen.KillTile(k, l, false, false, false);
                                                }
                                                else if (!Main.tile[k - 1, l + 1].active() && !Main.tile[k - 1, l].active() && WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k, l + 2))
                                                {
                                                    if (WorldGen.genRand.Next(5) == 0)
                                                    {
                                                        WorldGen.KillTile(k, l, false, false, false);
                                                    }
                                                    else if (WorldGen.genRand.Next(5) == 0)
                                                    {
                                                        WorldGen.PoundTile(k, l);
                                                    }
                                                    else
                                                    {
                                                        WorldGen.SlopeTile(k, l, 2);
                                                    }
                                                }
                                                else if (!Main.tile[k + 1, l + 1].active() && !Main.tile[k + 1, l].active() && WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k, l + 2))
                                                {
                                                    if (WorldGen.genRand.Next(5) == 0)
                                                    {
                                                        WorldGen.KillTile(k, l, false, false, false);
                                                    }
                                                    else if (WorldGen.genRand.Next(5) == 0)
                                                    {
                                                        WorldGen.PoundTile(k, l);
                                                    }
                                                    else
                                                    {
                                                        WorldGen.SlopeTile(k, l, 1);
                                                    }
                                                }
                                            }
                                        }
                                        if (WorldGen.SolidTile(k, l) && !Main.tile[k - 1, l].active() && !Main.tile[k + 1, l].active())
                                        {
                                            WorldGen.KillTile(k, l, false, false, false);
                                        }
                                    }
                                }
                                else if (!Main.tile[k, l].active() && Main.tile[k, l + 1].type != 151 && Main.tile[k, l + 1].type != 274)
                                {
                                    if (Main.tile[k + 1, l].type != 190 && Main.tile[k + 1, l].type != 48 && Main.tile[k + 1, l].type != 232 && WorldGen.SolidTile(k - 1, l + 1) && WorldGen.SolidTile(k + 1, l) && !Main.tile[k - 1, l].active() && !Main.tile[k + 1, l - 1].active())
                                    {
                                        WorldGen.PlaceTile(k, l, (int)Main.tile[k, l + 1].type, false, false, -1, 0);
                                        if (WorldGen.genRand.Next(2) == 0)
                                        {
                                            WorldGen.SlopeTile(k, l, 2);
                                        }
                                        else
                                        {
                                            WorldGen.PoundTile(k, l);
                                        }
                                    }
                                    if (Main.tile[k - 1, l].type != 190 && Main.tile[k - 1, l].type != 48 && Main.tile[k - 1, l].type != 232 && WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l) && !Main.tile[k + 1, l].active() && !Main.tile[k - 1, l - 1].active())
                                    {
                                        WorldGen.PlaceTile(k, l, (int)Main.tile[k, l + 1].type, false, false, -1, 0);
                                        if (WorldGen.genRand.Next(2) == 0)
                                        {
                                            WorldGen.SlopeTile(k, l, 1);
                                        }
                                        else
                                        {
                                            WorldGen.PoundTile(k, l);
                                        }
                                    }
                                }
                            }
                            else if (!Main.tile[k, l + 1].active() && WorldGen.genRand.Next(2) == 0 && WorldGen.SolidTile(k, l) && !Main.tile[k - 1, l].halfBrick() && !Main.tile[k + 1, l].halfBrick() && Main.tile[k - 1, l].slope() == 0 && Main.tile[k + 1, l].slope() == 0 && WorldGen.SolidTile(k, l - 1))
                            {
                                if (WorldGen.SolidTile(k - 1, l) && !WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k - 1, l - 1))
                                {
                                    WorldGen.SlopeTile(k, l, 3);
                                }
                                else if (WorldGen.SolidTile(k + 1, l) && !WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k + 1, l - 1))
                                {
                                    WorldGen.SlopeTile(k, l, 4);
                                }
                            }
                            if (TileID.Sets.Conversion.Sand[(int)Main.tile[k, l].type])
                            {
                                Tile.SmoothSlope(k, l, false);
                            }
                        }
                    }
                }
                for (int m = 20; m < Main.maxTilesX - 20; m++)
                {
                    for (int n = 20; n < Main.maxTilesY - 20; n++)
                    {
                        if (WorldGen.genRand.Next(2) == 0 && !Main.tile[m, n - 1].active() && Main.tile[m, n].type != 137 && Main.tile[m, n].type != 48 && Main.tile[m, n].type != 232 && Main.tile[m, n].type != 191 && Main.tile[m, n].type != 151 && Main.tile[m, n].type != 274 && Main.tile[m, n].type != 75 && Main.tile[m, n].type != 76 && WorldGen.SolidTile(m, n) && Main.tile[m - 1, n].type != 137 && Main.tile[m + 1, n].type != 137)
                        {
                            if (WorldGen.SolidTile(m, n + 1) && WorldGen.SolidTile(m + 1, n) && !Main.tile[m - 1, n].active())
                            {
                                WorldGen.SlopeTile(m, n, 2);
                            }
                            if (WorldGen.SolidTile(m, n + 1) && WorldGen.SolidTile(m - 1, n) && !Main.tile[m + 1, n].active())
                            {
                                WorldGen.SlopeTile(m, n, 1);
                            }
                        }
                        if (Main.tile[m, n].slope() == 1 && !WorldGen.SolidTile(m - 1, n))
                        {
                            WorldGen.SlopeTile(m, n, 0);
                            WorldGen.PoundTile(m, n);
                        }
                        if (Main.tile[m, n].slope() == 2 && !WorldGen.SolidTile(m + 1, n))
                        {
                            WorldGen.SlopeTile(m, n, 0);
                            WorldGen.PoundTile(m, n);
                        }
                    }
                }
                Main.tileSolid[137] = true;
                Main.tileSolid[190] = false;
                Main.tileSolid[192] = false;
            });
            /*AddGenerationPass("Settle Liquids", delegate (GenerationProgress progress)
            {
                progress.Message = Lang.gen[27].Value;
                //Liquid.QuickWater(3, -1, -1);
                //WorldGen.WaterCheck();
                int k = 0;
                Liquid.quickSettle = true;
                while (k < 10)
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
                    //WorldGen.WaterCheck();
                    progress.Set((float)k * 0.1f / 3f + 0.66f);
                }
                Liquid.quickSettle = false;
                Main.tileSolid[190] = true;
            });*/

            

            // TODO: More Simple Gen Stuff like Floating Island Houses/Jungle Trees etc

            AddGenerationPass("Quick Cleanup", delegate (GenerationProgress progress)
            {
                Main.tileSolid[137] = false;
                Main.tileSolid[130] = false;
                for (int k = 20; k < Main.maxTilesX - 20; k++)
                {
                    for (int l = 20; l < Main.maxTilesY - 20; l++)
                    {
                        if (Main.tile[k, l].type != 19 && TileID.Sets.CanBeClearedDuringGeneration[(int)Main.tile[k, l].type])
                        {
                            if (Main.tile[k, l].topSlope() || Main.tile[k, l].halfBrick())
                            {
                                if (!WorldGen.SolidTile(k, l + 1))
                                {
                                    Main.tile[k, l].active(false);
                                }
                                if (Main.tile[k + 1, l].type == TileID.Traps || Main.tile[k - 1, l].type == TileID.Traps)
                                {
                                    Main.tile[k, l].active(false);
                                }
                            }
                            else if (Main.tile[k, l].bottomSlope())
                            {
                                if (!WorldGen.SolidTile(k, l - 1))
                                {
                                    Main.tile[k, l].active(false);
                                }
                                if (Main.tile[k + 1, l].type == TileID.Traps || Main.tile[k - 1, l].type == TileID.Traps)
                                {
                                    Main.tile[k, l].active(false);
                                }
                            }
                        }
                    }
                }
            });

            // TODO: Gen stuff like Pots/Hellforge etc

            /*AddGenerationPass("Spreading Grass", delegate (GenerationProgress progress)
			{
				progress.Message = Lang.gen[37].Value;
				for (int k = 0; k < Main.maxTilesX; k++)
				{
					i2 = k;
					bool flag2 = true;
					int num = 0;
					while ((double)num < Main.worldSurface - 1.0)
					{
						if (Main.tile[i2, num].active())
						{
							if (flag2 && Main.tile[i2, num].type == 0)
							{
								try
								{
									WorldGen.grassSpread = 0;
									WorldGen.SpreadGrass(i2, num, 0, 2, true, 0);
								}
								catch
								{
									WorldGen.grassSpread = 0;
									WorldGen.SpreadGrass(i2, num, 0, 2, false, 0);
								}
							}
							if ((double)num > worldSurfaceHigh)
							{
								break;
							}
							flag2 = false;
						}
						else if (Main.tile[i2, num].wall == 0)
						{
							flag2 = true;
						}
						num++;
					}
				}
			});*/

            // Spawn Point
            AddGenerationPass("Spawn Point", delegate (GenerationProgress progress)
            {
                int num = 5;
                bool flag2 = true;
                while (flag2)
                {
                    int num2 = Main.maxTilesX / 2 + WorldGen.genRand.Next(-num, num + 1);
                    for (int k = 0; k < Main.maxTilesY; k++)
                    {
                        if (Main.tile[num2, k].active())
                        {
                            Main.spawnTileX = num2;
                            Main.spawnTileY = k;
                            break;
                        }
                    }
                    flag2 = false;
                    num++;
                    if ((double)Main.spawnTileY > Main.worldSurface)
                    {
                        flag2 = true;
                    }
                    if (Main.tile[Main.spawnTileX, Main.spawnTileY - 1].liquid > 0)
                    {
                        flag2 = true;
                    }
                }
                int num3 = 10;
                while ((double)Main.spawnTileY > Main.worldSurface)
                {
                    int num4 = WorldGen.genRand.Next(Main.maxTilesX / 2 - num3, Main.maxTilesX / 2 + num3);
                    for (int l = 0; l < Main.maxTilesY; l++)
                    {
                        if (Main.tile[num4, l].active())
                        {
                            Main.spawnTileX = num4;
                            Main.spawnTileY = l;
                            break;
                        }
                    }
                    num3++;
                }
            });
            /*AddGenerationPass("Grass Wall", delegate (GenerationProgress progress)
            {
                WorldGen.maxTileCount = 3500;
                for (int k = 50; k < Main.maxTilesX - 50; k++)
                {
                    int num = 0;
                    while ((double)num < Main.worldSurface - 10.0)
                    {
                        if (WorldGen.genRand.Next(4) == 0)
                        {
                            bool flag2 = false;
                            int num2 = -1;
                            int num3 = -1;
                            if (Main.tile[k, num].active() && Main.tile[k, num].type == 2 && (Main.tile[k, num].wall == 2 || Main.tile[k, num].wall == 63))
                            {
                                for (int l = k - 1; l <= k + 1; l++)
                                {
                                    for (int m = num - 1; m <= num + 1; m++)
                                    {
                                        if (Main.tile[l, m].wall == 0 && !WorldGen.SolidTile(l, m))
                                        {
                                            flag2 = true;
                                        }
                                    }
                                }
                                if (flag2)
                                {
                                    for (int n = k - 1; n <= k + 1; n++)
                                    {
                                        for (int num4 = num - 1; num4 <= num + 1; num4++)
                                        {
                                            if ((Main.tile[n, num4].wall == 2 || Main.tile[n, num4].wall == 15) && !WorldGen.SolidTile(n, num4))
                                            {
                                                num2 = n;
                                                num3 = num4;
                                            }
                                        }
                                    }
                                }
                            }
                            if (flag2 && num2 > -1 && num3 > -1)
                            {
                                int num5 = WorldGen.countDirtTiles(num2, num3);
                                if (num5 < WorldGen.maxTileCount)
                                {
                                    try
                                    {
                                        WorldGen.Spread.Wall2(num2, num3, 63);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        num++;
                    }
                }
                for (int num6 = 5; num6 < Main.maxTilesX - 5; num6++)
                {
                    int num7 = 10;
                    while ((double)num7 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[num6, num7].wall == 63 && WorldGen.genRand.Next(10) == 0)
                        {
                            Main.tile[num6, num7].wall = 65;
                        }
                        if (Main.tile[num6, num7].active() && Main.tile[num6, num7].type == 0)
                        {
                            bool flag3 = false;
                            for (int num8 = num6 - 1; num8 <= num6 + 1; num8++)
                            {
                                for (int num9 = num7 - 1; num9 <= num7 + 1; num9++)
                                {
                                    if (Main.tile[num6, num7].wall == 63 || Main.tile[num6, num7].wall == 65)
                                    {
                                        flag3 = true;
                                        break;
                                    }
                                }
                            }
                            if (flag3)
                            {
                                WorldGen.SpreadGrass(num6, num7, 0, 2, true, 0);
                            }
                        }
                        num7++;
                    }
                }
            });*/
            // TODO: Gen pre-spawned npcs like the Guide

            // TODO: Gen plants such as sunflowers/trees

            /*AddGenerationPass("Weeds", delegate (GenerationProgress progress)
            {
                progress.Message = Lang.gen[42].Value;
                if (Main.halloween)
                {
                    for (int k = 40; k < Main.maxTilesX - 40; k++)
                    {
                        int num = 50;
                        while ((double)num < Main.worldSurface)
                        {
                            if (Main.tile[k, num].active() && Main.tile[k, num].type == 2 && WorldGen.genRand.Next(15) == 0)
                            {
                                WorldGen.PlacePumpkin(k, num - 1);
                                int num2 = WorldGen.genRand.Next(5);
                                for (int l = 0; l < num2; l++)
                                {
                                    WorldGen.GrowPumpkin(k, num - 1, 254);
                                }
                            }
                            num++;
                        }
                    }
                }
                WorldGen.AddPlants();
            });*/

            /*AddGenerationPass("Vines", delegate (GenerationProgress progress)
			{
				progress.Message = Lang.gen[43].Value;
				for (int k = 0; k < Main.maxTilesX; k++)
				{
					int num = 0;
					int num2 = 0;
					while ((double)num2 < Main.worldSurface)
					{
						if (num > 0 && !Main.tile[k, num2].active())
						{
							Main.tile[k, num2].active(true);
							Main.tile[k, num2].type = 52;
							num--;
						}
						else
						{
							num = 0;
						}
						if (Main.tile[k, num2].active() && !Main.tile[k, num2].bottomSlope() && (Main.tile[k, num2].type == 2 || (Main.tile[k, num2].type == 192 && WorldGen.genRand.Next(4) == 0)) && WorldGen.genRand.Next(5) < 3)
						{
							num = WorldGen.genRand.Next(1, 10);
						}
						num2++;
					}
					num = 0;
					for (int l = 0; l < Main.maxTilesY; l++)
					{
						if (num > 0 && !Main.tile[k, l].active())
						{
							Main.tile[k, l].active(true);
							Main.tile[k, l].type = 62;
							num--;
						}
						else
						{
							num = 0;
						}
						if (Main.tile[k, l].active() && Main.tile[k, l].type == 60 && !Main.tile[k, l].bottomSlope())
						{
							if (k < Main.maxTilesX - 1 && l < Main.maxTilesY - 2 && Main.tile[k + 1, l].active() && Main.tile[k + 1, l].type == 60 && !Main.tile[k + 1, l].bottomSlope() && WorldGen.genRand.Next(40) == 0)
							{
								bool flag2 = true;
								for (int m = k; m < k + 2; m++)
								{
									for (int n = l + 1; n < l + 3; n++)
									{
										if (Main.tile[m, n].active() && (!Main.tileCut[(int)Main.tile[m, n].type] || Main.tile[m, n].type == 444))
										{
											flag2 = false;
											break;
										}
										if (Main.tile[m, n].liquid > 0 || Main.wallHouse[(int)Main.tile[m, n].wall])
										{
											flag2 = false;
											break;
										}
									}
									if (!flag2)
									{
										break;
									}
								}
								if (flag2 && WorldGen.CountNearBlocksTypes(k, l, 20, 1, new int[]
									{
											444
									}) > 0)
								{
									flag2 = false;
								}
								if (flag2)
								{
									for (int num3 = k; num3 < k + 2; num3++)
									{
										for (int num4 = l + 1; num4 < l + 3; num4++)
										{
											WorldGen.KillTile(num3, num4, false, false, false);
										}
									}
									for (int num5 = k; num5 < k + 2; num5++)
									{
										for (int num6 = l + 1; num6 < l + 3; num6++)
										{
											Main.tile[num5, num6].active(true);
											Main.tile[num5, num6].type = 444;
											Main.tile[num5, num6].frameX = (short)((num5 - k) * 18);
											Main.tile[num5, num6].frameY = (short)((num6 - l - 1) * 18);
										}
									}
									goto IL_3C1;
								}
							}
							if (WorldGen.genRand.Next(5) < 3)
							{
								num = WorldGen.genRand.Next(1, 10);
							}
						}
						IL_3C1:
						;
					}
					num = 0;
					for (int num7 = 0; num7 < Main.maxTilesY; num7++)
					{
						if (num > 0 && !Main.tile[k, num7].active())
						{
							Main.tile[k, num7].active(true);
							Main.tile[k, num7].type = 205;
							num--;
						}
						else
						{
							num = 0;
						}
						if (Main.tile[k, num7].active() && Main.tile[k, num7].type == 199 && WorldGen.genRand.Next(5) < 3)
						{
							num = WorldGen.genRand.Next(1, 10);
						}
					}
				}
			});*/

            /*AddGenerationPass("Settle Liquids Again", delegate (GenerationProgress progress)
            {
                progress.Message = "Solar world Gen : " + Lang.gen[27].Value;
                //Liquid.QuickWater(3, -1, -1);
                //WorldGen.WaterCheck();
                int k = 0;
                Liquid.quickSettle = true;
                while (k < 10)
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
                    //WorldGen.WaterCheck();
                    progress.Set((float)k * 0.1f / 3f + 0.66f);
                }
                Liquid.quickSettle = false;
            });*/
            AddGenerationPass("Tile Cleanup", delegate (GenerationProgress progress)
            {
                for (int k = 40; k < Main.maxTilesX - 40; k++)
                {
                    for (int l = 40; l < Main.maxTilesY - 40; l++)
                    {
                        if (!Main.tile[k, l].active() && Main.tile[k, l].liquid == 0 && WorldGen.genRand.Next(3) != 0 && WorldGen.SolidTile(k, l - 1))
                        {
                            int num = WorldGen.genRand.Next(15, 21);
                            for (int m = l - 2; m >= l - num; m--)
                            {
                                if (Main.tile[k, m].liquid >= 128)
                                {
                                    int num2 = TileID.WaterDrip;
	                                if (Main.tile[k, m].lava())
	                                {
		                                num2 = TileID.LavaDrip;
	                                } else if (Main.tile[k, m].honey())
                                    {
                                        num2 = TileID.HoneyDrip;
                                    }
                                    int maxValue = l - m;
                                    if (WorldGen.genRand.Next(maxValue) <= 1)
                                    {
                                        Main.tile[k, l].type = (ushort)num2;
                                        Main.tile[k, l].frameX = 0;
                                        Main.tile[k, l].frameY = 0;
                                        Main.tile[k, l].active(true);
                                        break;
                                    }
                                }
                            }
                            if (!Main.tile[k, l].active())
                            {
                                num = WorldGen.genRand.Next(3, 11);
                                for (int n = l + 1; n <= l + num; n++)
                                {
                                    if (Main.tile[k, n].liquid >= 200)
                                    {
                                        int num3 = TileID.WaterDrip;
                                        if (Main.tile[k, n].lava())
                                        {
                                            num3 = TileID.LavaDrip;
                                        }
                                        else if (Main.tile[k, n].honey())
                                        {
                                            num3 = TileID.HoneyDrip;
                                        }
                                        int num4 = n - l;
                                        if (WorldGen.genRand.Next(num4 * 3) <= 1)
                                        {
                                            Main.tile[k, l].type = (ushort)num3;
                                            Main.tile[k, l].frameX = 0;
                                            Main.tile[k, l].frameY = 0;
                                            Main.tile[k, l].active(true);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!Main.tile[k, l].active() && WorldGen.genRand.Next(3) != 0)
                            {
                                Tile tile = Main.tile[k, l - 1];
                                if (TileID.Sets.Conversion.Sandstone[(int)tile.type] || TileID.Sets.Conversion.HardenedSand[(int)tile.type])
                                {
                                    Main.tile[k, l].type = 461;
                                    Main.tile[k, l].frameX = 0;
                                    Main.tile[k, l].frameY = 0;
                                    Main.tile[k, l].active(true);
                                }
                            }
                        }
                        if (Main.tile[k, l].type == 137)
                        {
                            if (Main.tile[k, l].frameY <= 52)
                            {
                                int num5 = -1;
                                if (Main.tile[k, l].frameX >= 18)
                                {
                                    num5 = 1;
                                }
                                if (Main.tile[k + num5, l].halfBrick() || Main.tile[k + num5, l].slope() != 0)
                                {
                                    Main.tile[k + num5, l].active(false);
                                }
                            }
                        }
                        else if (Main.tile[k, l].type == 162 && Main.tile[k, l + 1].liquid == 0)
                        {
                            Main.tile[k, l].active(false);
                        }
                        if (Main.tile[k, l].wall == 13 || Main.tile[k, l].wall == 14)
                        {
                            Main.tile[k, l].liquid = 0;
                        }
                        if (Main.tile[k, l].type == 31)
                        {
                            int num6 = (int)(Main.tile[k, l].frameX / 18);
                            int num7 = 0;
                            int num8 = k;
                            num7 += num6 / 2;
                            num7 = (WorldGen.crimson ? 1 : 0);
                            num6 %= 2;
                            num8 -= num6;
                            int num9 = (int)(Main.tile[k, l].frameY / 18);
                            int num10 = 0;
                            int num11 = l;
                            num10 += num9 / 2;
                            num9 %= 2;
                            num11 -= num9;
                            for (int num12 = 0; num12 < 2; num12++)
                            {
                                for (int num13 = 0; num13 < 2; num13++)
                                {
                                    int num14 = num8 + num12;
                                    int num15 = num11 + num13;
                                    Main.tile[num14, num15].active(true);
                                    Main.tile[num14, num15].slope(0);
                                    Main.tile[num14, num15].halfBrick(false);
                                    Main.tile[num14, num15].type = 31;
                                    Main.tile[num14, num15].frameX = (short)(num12 * 18 + 36 * num7);
                                    Main.tile[num14, num15].frameY = (short)(num13 * 18 + 36 * num10);
                                }
                            }
                        }
                        if (Main.tile[k, l].type == 12)
                        {
                            int num16 = (int)(Main.tile[k, l].frameX / 18);
                            int num17 = 0;
                            int num18 = k;
                            num17 += num16 / 2;
                            num16 %= 2;
                            num18 -= num16;
                            int num19 = (int)(Main.tile[k, l].frameY / 18);
                            int num20 = 0;
                            int num21 = l;
                            num20 += num19 / 2;
                            num19 %= 2;
                            num21 -= num19;
                            for (int num22 = 0; num22 < 2; num22++)
                            {
                                for (int num23 = 0; num23 < 2; num23++)
                                {
                                    int num24 = num18 + num22;
                                    int num25 = num21 + num23;
                                    Main.tile[num24, num25].active(true);
                                    Main.tile[num24, num25].slope(0);
                                    Main.tile[num24, num25].halfBrick(false);
                                    Main.tile[num24, num25].type = 12;
                                    Main.tile[num24, num25].frameX = (short)(num22 * 18 + 36 * num17);
                                    Main.tile[num24, num25].frameY = (short)(num23 * 18 + 36 * num20);
                                }
                                if (!Main.tile[num22, l + 2].active())
                                {
                                    Main.tile[num22, l + 2].active(true);
                                    if (!Main.tileSolid[(int)Main.tile[num22, l + 2].type] || Main.tileSolidTop[(int)Main.tile[num22, l + 2].type])
                                    {
                                        Main.tile[num22, l + 2].type = 0;
                                    }
                                }
                                Main.tile[num22, l + 2].slope(0);
                                Main.tile[num22, l + 2].halfBrick(false);
                            }
                        }
                        if (TileID.Sets.BasicChest[(int)Main.tile[k, l].type] && Main.tile[k, l].type < TileID.Count)
                        {
                            int num26 = (int)(Main.tile[k, l].frameX / 18);
                            int num27 = 0;
                            int num28 = k;
                            int num29 = l - (int)(Main.tile[k, l].frameY / 18);
                            while (num26 >= 2)
                            {
                                num27++;
                                num26 -= 2;
                            }
                            num28 -= num26;
                            int num30 = Chest.FindChest(num28, num29);
                            if (num30 != -1)
                            {
                                int type = Main.chest[num30].item[0].type;
                                if (type != 1156)
                                {
                                    if (type != 1260)
                                    {
                                        switch (type)
                                        {
                                            case 1569:
                                                num27 = 25;
                                                break;
                                            case 1571:
                                                num27 = 24;
                                                break;
                                            case 1572:
                                                num27 = 27;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        num27 = 26;
                                    }
                                }
                                else
                                {
                                    num27 = 23;
                                }
                            }
                            for (int num31 = 0; num31 < 2; num31++)
                            {
                                for (int num32 = 0; num32 < 2; num32++)
                                {
                                    int num33 = num28 + num31;
                                    int num34 = num29 + num32;
                                    Main.tile[num33, num34].active(true);
                                    Main.tile[num33, num34].slope(0);
                                    Main.tile[num33, num34].halfBrick(false);
                                    Main.tile[num33, num34].type = 21;
                                    Main.tile[num33, num34].frameX = (short)(num31 * 18 + 36 * num27);
                                    Main.tile[num33, num34].frameY = (short)(num32 * 18);
                                }
                                if (!Main.tile[num31, l + 2].active())
                                {
                                    Main.tile[num31, l + 2].active(true);
                                    if (!Main.tileSolid[(int)Main.tile[num31, l + 2].type] || Main.tileSolidTop[(int)Main.tile[num31, l + 2].type])
                                    {
                                        Main.tile[num31, l + 2].type = 0;
                                    }
                                }
                                Main.tile[num31, l + 2].slope(0);
                                Main.tile[num31, l + 2].halfBrick(false);
                            }
                        }
                        if (Main.tile[k, l].type == 28)
                        {
                            int num35 = (int)(Main.tile[k, l].frameX / 18);
                            int num36 = 0;
                            int num37 = k;
                            while (num35 >= 2)
                            {
                                num36++;
                                num35 -= 2;
                            }
                            num37 -= num35;
                            int num38 = (int)(Main.tile[k, l].frameY / 18);
                            int num39 = 0;
                            int num40 = l;
                            while (num38 >= 2)
                            {
                                num39++;
                                num38 -= 2;
                            }
                            num40 -= num38;
                            for (int num41 = 0; num41 < 2; num41++)
                            {
                                for (int num42 = 0; num42 < 2; num42++)
                                {
                                    int num43 = num37 + num41;
                                    int num44 = num40 + num42;
                                    Main.tile[num43, num44].active(true);
                                    Main.tile[num43, num44].slope(0);
                                    Main.tile[num43, num44].halfBrick(false);
                                    Main.tile[num43, num44].type = 28;
                                    Main.tile[num43, num44].frameX = (short)(num41 * 18 + 36 * num36);
                                    Main.tile[num43, num44].frameY = (short)(num42 * 18 + 36 * num39);
                                }
                                if (!Main.tile[num41, l + 2].active())
                                {
                                    Main.tile[num41, l + 2].active(true);
                                    if (!Main.tileSolid[(int)Main.tile[num41, l + 2].type] || Main.tileSolidTop[(int)Main.tile[num41, l + 2].type])
                                    {
                                        Main.tile[num41, l + 2].type = 0;
                                    }
                                }
                                Main.tile[num41, l + 2].slope(0);
                                Main.tile[num41, l + 2].halfBrick(false);
                            }
                        }
                        if (Main.tile[k, l].type == 26)
                        {
                            int num45 = (int)(Main.tile[k, l].frameX / 18);
                            int num46 = 0;
                            int num47 = k;
                            int num48 = l - (int)(Main.tile[k, l].frameY / 18);
                            while (num45 >= 3)
                            {
                                num46++;
                                num45 -= 3;
                            }
                            num47 -= num45;
                            for (int num49 = 0; num49 < 3; num49++)
                            {
                                for (int num50 = 0; num50 < 2; num50++)
                                {
                                    int num51 = num47 + num49;
                                    int num52 = num48 + num50;
                                    Main.tile[num51, num52].active(true);
                                    Main.tile[num51, num52].slope(0);
                                    Main.tile[num51, num52].halfBrick(false);
                                    Main.tile[num51, num52].type = 26;
                                    Main.tile[num51, num52].frameX = (short)(num49 * 18 + 54 * num46);
                                    Main.tile[num51, num52].frameY = (short)(num50 * 18);
                                }
                                if (!Main.tile[num47 + num49, num48 + 2].active() || !Main.tileSolid[(int)Main.tile[num47 + num49, num48 + 2].type] || Main.tileSolidTop[(int)Main.tile[num47 + num49, num48 + 2].type])
                                {
                                    Main.tile[num47 + num49, num48 + 2].active(true);
                                    if (!TileID.Sets.Platforms[(int)Main.tile[num47 + num49, num48 + 2].type] && (!Main.tileSolid[(int)Main.tile[num47 + num49, num48 + 2].type] || Main.tileSolidTop[(int)Main.tile[num47 + num49, num48 + 2].type]))
                                    {
                                        Main.tile[num47 + num49, num48 + 2].type = 0;
                                    }
                                }
                                Main.tile[num47 + num49, num48 + 2].slope(0);
                                Main.tile[num47 + num49, num48 + 2].halfBrick(false);
                                if (Main.tile[num47 + num49, num48 + 3].type == 28 && Main.tile[num47 + num49, num48 + 3].frameY % 36 >= 18)
                                {
                                    Main.tile[num47 + num49, num48 + 3].type = 0;
                                    Main.tile[num47 + num49, num48 + 3].active(false);
                                }
                            }
                            for (int num53 = 0; num53 < 3; num53++)
                            {
                                if ((Main.tile[num47 - 1, num48 + num53].type == 28 || Main.tile[num47 - 1, num48 + num53].type == 12) && Main.tile[num47 - 1, num48 + num53].frameX % 36 < 18)
                                {
                                    Main.tile[num47 - 1, num48 + num53].type = 0;
                                    Main.tile[num47 - 1, num48 + num53].active(false);
                                }
                                if ((Main.tile[num47 + 3, num48 + num53].type == 28 || Main.tile[num47 + 3, num48 + num53].type == 12) && Main.tile[num47 + 3, num48 + num53].frameX % 36 >= 18)
                                {
                                    Main.tile[num47 + 3, num48 + num53].type = 0;
                                    Main.tile[num47 + 3, num48 + num53].active(false);
                                }
                            }
                        }
                        if (Main.tile[k, l].type == 237 && Main.tile[k, l + 1].type == 232)
                        {
                            Main.tile[k, l + 1].type = 226;
                        }
                    }
                }
            });

            // TODO: Gen Micro-Biomes

            AddGenerationPass("Volcano!!!", delegate (GenerationProgress progress)
            {
                //TUAWorld.solarWorldGen(ModLoader.GetMod("TUA"));
            });

            
            /*AddGenerationPass("Final Cleanup", delegate (GenerationProgress progress)
            {
                for (int k = 0; k < Main.maxTilesX; k++)
                {
                    for (int l = 0; l < Main.maxTilesY; l++)
                    {
                        if (Main.tile[k, l].active() && (!WorldGen.SolidTile(k, l + 1) || !WorldGen.SolidTile(k, l + 2)))
                        {
                            ushort type = Main.tile[k, l].type;
                            if (type <= 112)
                            {
                                if (type != 53)
                                {
                                    if (type == 112)
                                    {
                                        Main.tile[k, l].type = 398;
                                    }
                                }
                                else
                                {
                                    Main.tile[k, l].type = 397;
                                }
                            }
                            else if (type != 123)
                            {
                                if (type != 224)
                                {
                                    if (type == 234)
                                    {
                                        Main.tile[k, l].type = 399;
                                    }
                                }
                                else
                                {
                                    Main.tile[k, l].type = 147;
                                }
                            }
                            else
                            {
                                Main.tile[k, l].type = 1;
                            }
                        }
                    }
                }
                WorldGen.noTileActions = false;
                WorldGen.gen = false;
                Main.AnglerQuestSwap();
            });*/
        }

		public static void Caverer(int X, int Y)
		{
			int num = WorldGen.genRand.Next(2);
			if (num == 0)
			{
				int num2 = WorldGen.genRand.Next(7, 9);
				float num3 = (float)WorldGen.genRand.Next(100) * 0.01f;
				float num4 = 1f - num3;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num3 = -num3;
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					num4 = -num4;
				}
				Vector2 vector = new Vector2((float)X, (float)Y);
				for (int i = 0; i < num2; i++)
				{
					vector = SolarWorldGen.digTunnel(vector.X, vector.Y, num3, num4, WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(4, 9), false);
					num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
					num4 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
					if ((double)num3 < -1.5)
					{
						num3 = -1.5f;
					}
					if ((double)num3 > 1.5)
					{
						num3 = 1.5f;
					}
					if ((double)num4 < -1.5)
					{
						num4 = -1.5f;
					}
					if ((double)num4 > 1.5)
					{
						num4 = 1.5f;
					}
					float num5 = (float)WorldGen.genRand.Next(100) * 0.01f;
					float num6 = 1f - num5;
					if (WorldGen.genRand.Next(2) == 0)
					{
						num5 = -num5;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						num6 = -num6;
					}
					Vector2 vector2 = SolarWorldGen.digTunnel(vector.X, vector.Y, num5, num6, WorldGen.genRand.Next(30, 50), WorldGen.genRand.Next(3, 6), false);
					SolarWorldGen.TileRunner((int)vector2.X, (int)vector2.Y, (double)WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), -1, false, 0f, 0f, false, true);
				}
				return;
			}
			if (num == 1)
			{
				int num7 = WorldGen.genRand.Next(15, 30);
				float num8 = (float)WorldGen.genRand.Next(100) * 0.01f;
				float num9 = 1f - num8;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num8 = -num8;
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					num9 = -num9;
				}
				Vector2 vector3 = new Vector2((float)X, (float)Y);
				for (int j = 0; j < num7; j++)
				{
					vector3 = SolarWorldGen.digTunnel(vector3.X, vector3.Y, num8, num9, WorldGen.genRand.Next(5, 15),
						WorldGen.genRand.Next(2, 6), true);
					num8 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
					num9 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
					if ((double)num8 < -1.5)
					{
						num8 = -1.5f;
					}
					if ((double)num8 > 1.5)
					{
						num8 = 1.5f;
					}
					if ((double)num9 < -1.5)
					{
						num9 = -1.5f;
					}
					if ((double)num9 > 1.5)
					{
						num9 = 1.5f;
					}
				}
			}
		}

		public static Vector2 digTunnel(float X, float Y, float xDir, float yDir, int Steps, int Size, bool Wet = false)
		{
			float num = X;
			float num2 = Y;
			try
			{
				float num3 = 0f;
				float num4 = 0f;
				float num5 = (float)Size;
				num = MathHelper.Clamp(num, num5 + 1f, (float)Main.maxTilesX - num5 - 1f);
				num2 = MathHelper.Clamp(num2, num5 + 1f, (float)Main.maxTilesY - num5 - 1f);
				for (int i = 0; i < Steps; i++)
				{
					int num6 = (int)(num - num5);
					while ((float)num6 <= num + num5)
					{
						int num7 = (int)(num2 - num5);
						while ((float)num7 <= num2 + num5)
						{
							if ((double)(Math.Abs((float)num6 - num) + Math.Abs((float)num7 - num2)) < (double)num5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.005) && num6 >= 0 && num6 < Main.maxTilesX && num7 >= 0 && num7 < Main.maxTilesY)
							{
								Main.tile[num6, num7].active(false);
								if (Wet)
								{
									Main.tile[num6, num7].liquid = 255;
									Main.tile[num6, num7].lava(true); // Added for only lava
								}
							}
							num7++;
						}
						num6++;
					}
					num5 += (float)WorldGen.genRand.Next(-50, 51) * 0.03f;
					if ((double)num5 < (double)Size * 0.6)
					{
						num5 = (float)Size * 0.6f;
					}
					if (num5 > (float)(Size * 2))
					{
						num5 = (float)Size * 2f;
					}
					num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
					num4 += (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
					if (num3 < -1f)
					{
						num3 = -1f;
					}
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					if (num4 < -1f)
					{
						num4 = -1f;
					}
					if (num4 > 1f)
					{
						num4 = 1f;
					}
					num += (xDir + num3) * 0.6f;
					num2 += (yDir + num4) * 0.6f;
				}
			}
			catch
			{
			}
			return new Vector2(num, num2);
		}

		public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true)
		{
			double num = strength;
			float num2 = (float)steps;
			Vector2 value;
			value.X = (float)i;
			value.Y = (float)j;
			Vector2 value2;
			value2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			value2.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			if (speedX != 0f || speedY != 0f)
			{
				value2.X = speedX;
				value2.Y = speedY;
			}
			bool flag = type == 368;
			bool flag2 = type == 367;
			while (num > 0.0 && num2 > 0f)
			{
				if (value.Y < 0f && num2 > 0f && type == 59)
				{
					num2 = 0f;
				}
				num = strength * (double)(num2 / (float)steps);
				num2 -= 1f;
				int num3 = (int)((double)value.X - num * 0.5);
				int num4 = (int)((double)value.X + num * 0.5);
				int num5 = (int)((double)value.Y - num * 0.5);
				int num6 = (int)((double)value.Y + num * 0.5);
				if (num3 < 1)
				{
					num3 = 1;
				}
				if (num4 > Main.maxTilesX - 1)
				{
					num4 = Main.maxTilesX - 1;
				}
				if (num5 < 1)
				{
					num5 = 1;
				}
				if (num6 > Main.maxTilesY - 1)
				{
					num6 = Main.maxTilesY - 1;
				}
				for (int k = num3; k < num4; k++)
				{
					for (int l = num5; l < num6; l++)
					{
						if ((double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
						{
							if (GetMudwall() && (double)l > Main.worldSurface && Main.tile[k, l - 1].wall != 2 && l < Main.maxTilesY - 210 - WorldGen.genRand.Next(3))
							{
								if (l > WorldGen.lavaLine - WorldGen.genRand.Next(0, 4) - 50)
								{
									if (Main.tile[k, l - 1].wall != 64 && Main.tile[k, l + 1].wall != 64 && Main.tile[k - 1, l].wall != 64 && Main.tile[k, l + 1].wall != 64)
									{
										WorldGen.PlaceWall(k, l, 15, true);
									}
								}
								else if (Main.tile[k, l - 1].wall != 15 && Main.tile[k, l + 1].wall != 15 && Main.tile[k - 1, l].wall != 15 && Main.tile[k, l + 1].wall != 15)
								{
									WorldGen.PlaceWall(k, l, 64, true);
								}
							}
							if (type < 0)
							{
								if (type == -2 && Main.tile[k, l].active() && (l < WorldGen.waterLine || l > WorldGen.lavaLine))
								{
									Main.tile[k, l].liquid = 255;
									//if (l > WorldGen.lavaLine)
									//{
										Main.tile[k, l].lava(true);
									//}
								}
								Main.tile[k, l].active(false);
							}
							else
							{
								if (flag && (double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
								{
									WorldGen.PlaceWall(k, l, 180, true);
								}
								if (flag2 && (double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
								{
									WorldGen.PlaceWall(k, l, 178, true);
								}
								if (overRide || !Main.tile[k, l].active())
								{
									Tile tile = Main.tile[k, l];
									bool flag3 = Main.tileStone[type] && tile.type != 1;
									if (!TileID.Sets.CanBeClearedDuringGeneration[(int)tile.type])
									{
										flag3 = true;
									}
									ushort type2 = tile.type;
									if (type2 <= 147)
									{
										if (type2 <= 45)
										{
											if (type2 != 1)
											{
												if (type2 == 45)
												{
													goto IL_575;
												}
											}
											else if (type == 59 && (double)l < Main.worldSurface + (double)WorldGen.genRand.Next(-50, 50))
											{
												flag3 = true;
											}
										}
										else if (type2 != 53)
										{
											if (type2 == 147)
											{
												goto IL_575;
											}
										}
										else
										{
											if (type == 40)
											{
												flag3 = true;
											}
											if ((double)l < Main.worldSurface && type != 59)
											{
												flag3 = true;
											}
										}
									}
									else if (type2 <= 196)
									{
										switch (type2)
										{
											case 189:
											case 190:
												goto IL_575;
											default:
												if (type2 == 196)
												{
													goto IL_575;
												}
												break;
										}
									}
									else
									{
										switch (type2)
										{
											case 367:
											case 368:
												if (type == 59)
												{
													flag3 = true;
												}
												break;
											default:
												switch (type2)
												{
													case 396:
													case 397:
														flag3 = !TileID.Sets.Ore[type];
														break;
												}
												break;
										}
									}
									IL_5B7:
									if (!flag3)
									{
										tile.type = (ushort)type;
										goto IL_5C5;
									}
									goto IL_5C5;
									IL_575:
									flag3 = true;
									goto IL_5B7;
								}
								IL_5C5:
								if (addTile)
								{
									Main.tile[k, l].active(true);
									Main.tile[k, l].liquid = 0;
									Main.tile[k, l].lava(false);
								}
								if (noYChange && (double)l < Main.worldSurface && type != 59)
								{
									Main.tile[k, l].wall = 2;
								}
								if (type == 59 && l > WorldGen.waterLine && Main.tile[k, l].liquid > 0)
								{
									Main.tile[k, l].lava(false);
									Main.tile[k, l].liquid = 0;
								}
							}
						}
					}
				}
				value += value2;
				if (num > 50.0)
				{
					value += value2;
					num2 -= 1f;
					value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					if (num > 100.0)
					{
						value += value2;
						num2 -= 1f;
						value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						if (num > 150.0)
						{
							value += value2;
							num2 -= 1f;
							value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							if (num > 200.0)
							{
								value += value2;
								num2 -= 1f;
								value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								if (num > 250.0)
								{
									value += value2;
									num2 -= 1f;
									value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									if (num > 300.0)
									{
										value += value2;
										num2 -= 1f;
										value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										if (num > 400.0)
										{
											value += value2;
											num2 -= 1f;
											value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											if (num > 500.0)
											{
												value += value2;
												num2 -= 1f;
												value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												if (num > 600.0)
												{
													value += value2;
													num2 -= 1f;
													value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													if (num > 700.0)
													{
														value += value2;
														num2 -= 1f;
														value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														if (num > 800.0)
														{
															value += value2;
															num2 -= 1f;
															value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															if (num > 900.0)
															{
																value += value2;
																num2 -= 1f;
																value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
																value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				if (value2.X > 1f)
				{
					value2.X = 1f;
				}
				if (value2.X < -1f)
				{
					value2.X = -1f;
				}
				if (!noYChange)
				{
					value2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					if (value2.Y > 1f)
					{
						value2.Y = 1f;
					}
					if (value2.Y < -1f)
					{
						value2.Y = -1f;
					}
				}
				else if (type != 59 && num < 3.0)
				{
					if (value2.Y > 1f)
					{
						value2.Y = 1f;
					}
					if (value2.Y < -1f)
					{
						value2.Y = -1f;
					}
				}
				if (type == 59 && !noYChange)
				{
					if ((double)value2.Y > 0.5)
					{
						value2.Y = 0.5f;
					}
					if ((double)value2.Y < -0.5)
					{
						value2.Y = -0.5f;
					}
					if ((double)value.Y < Main.rockLayer + 100.0)
					{
						value2.Y = 1f;
					}
					if (value.Y > (float)(Main.maxTilesY - 300))
					{
						value2.Y = -1f;
					}
				}
			}
		}

	    
	}
}
