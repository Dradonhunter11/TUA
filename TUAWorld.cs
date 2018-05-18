using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using TerrariaUltraApocalypse.Dimension;

namespace TerrariaUltraApocalypse
{
    class TUAWorld : ModWorld
    {
        public static bool apocalypseMoon = false;
        public static int apocalypseMoonPoint = 0;
        public static bool Apocalypse;
        public static bool UltraMode;

        public override void Initialize()
        {
            
            Func<bool> c = () => Main.hardMode;
            BiomeLibs.RegisterNewBiome("Plagues", 50, mod);
            BiomeLibs.AddBlockInBiome("Plagues", new String[] { "ApocalypseDirt" });
            BiomeLibs.SetCondition("Plagues", c);


            BiomeLibs.RegisterNewBiome("Meteoridon", 50, mod);
            BiomeLibs.AddBlockInBiome("Meteoridon", new String[] { "MeteoridonStone", "MeteoridonGrass", "BrownIce" });
            BiomeLibs.addHallowAltBiome("Meteoridon", "The world is getting fiery...");
            BiomeLibs.SetCondition("Meteoridon", c);
            

            BiomeLibs.RegisterNewBiome("Nebula", 200, mod);
            BiomeLibs.RegisterNewBiome("Vortex", 200, mod);
            BiomeLibs.RegisterNewBiome("Stardust", 200, mod);
            BiomeLibs.RegisterNewBiome("Solar", 200, mod);
            BiomeLibs.AddBlockInBiome("Solar", new String[] { "SolarDirt", "SolarRock"});
        }

        public override TagCompound Save()
        {
            TagCompound tc = new TagCompound();
            tc.Add("UltraMode", UltraMode);
            tc.Add("EoADowned", Apocalypse);
            tc.Add("UltraEoCDowned", TerrariaUltraApocalypse.EoCDeath);
            //tc.Add("apocalypseMoon", apocalypseMoon);
            return tc;
        }

        public override void Load(TagCompound tag)
        {
            UltraMode = tag.GetBool("UltraMode");
            Apocalypse = tag.GetBool("EoADowned");
            TerrariaUltraApocalypse.EoCDeath = tag.GetInt("UltraEoCDowned");
            
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
                    } else if (targetType==2) {
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
            } else if (!tile.inActive() && (tile.type == (ushort)mod.TileType("MeteoridonGrass") || tile.type == (ushort)mod.TileType("MeteoridonStone") || tile.type == (ushort)mod.TileType("BrownIce") && WorldGen.genRand.Next(4) == 0))
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
                    } else if (targetType == 161)
                    {
                        Main.tile[toX, toY].type = (ushort)mod.TileType("BrownIce");
                        tileChanged = true;
                    }else if (targetType == 0 && Main.tile[toX, toY - 1] == null)
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

        public static void swapDimension() {
            Main.GetWorldPathFromName("test.wld", false);
            
        }

        public override void PreUpdate()
        {
            if (!apocalypseMoon) {
                Main.moonTexture = TerrariaUltraApocalypse.originalMoon;
            }
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
        
    }
}
