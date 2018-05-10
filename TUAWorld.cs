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
        //private static TUADimension dimHandler = new TUADimension();

        public override void Initialize()
        {
            BiomeLibs.RegisterNewBiome("Plagues", 50, mod);
            BiomeLibs.AddBlockInBiome("Plagues", new String[] { "ApocalypseDirt" });
            BiomeLibs.RegisterNewBiome("Meteoridon", 50, mod);
            BiomeLibs.AddBlockInBiome("Meteoridon", new String[] { "MeteoridonStone", "MeteoridonGrass", "BrownIce" });
            BiomeLibs.addHallowAltBiome("Meteoridon");
            BiomeLibs.RegisterNewBiome("Nebula", 200, mod);
            BiomeLibs.RegisterNewBiome("Vortex", 200, mod);
            BiomeLibs.RegisterNewBiome("Stardust", 200, mod);
            BiomeLibs.RegisterNewBiome("Solar", 200, mod);
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
            //apocalypseMoon = tag.GetBool("apocalypseMoon");
            //dimHandler.CreateDimension(null);
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
        public void MERunner(int i, int j, float speedX = 0f, float speedY = 0f)
        {
            int num = WorldGen.genRand.Next(200, 250);
            float num2 = (float)(Main.maxTilesX / 4200);
            num = (int)((float)num * num2);
            double num3 = (double)num;
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
            bool flag = true;
            while (flag)
            {
                int num4 = (int)((double)value.X - num3 * 0.5);
                int num5 = (int)((double)value.X + num3 * 0.5);
                int num6 = (int)((double)value.Y - num3 * 0.5);
                int num7 = (int)((double)value.Y + num3 * 0.5);
                if (num4 < 0)
                {
                    num4 = 0;
                }
                if (num5 > Main.maxTilesX)
                {
                    num5 = Main.maxTilesX;
                }
                if (num6 < 0)
                {
                    num6 = 0;
                }
                if (num7 > Main.maxTilesY)
                {
                    num7 = Main.maxTilesY;
                }
                for (int k = num4; k < num5; k++)
                {
                    for (int l = num6; l < num7; l++)
                    {
                        if ((double)(System.Math.Abs((float)k - value.X) + System.Math.Abs((float)l - value.Y)) < (double)num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            /*if (Main.tile[k, l].wall == 63 || Main.tile[k, l].wall == 65 || Main.tile[k, l].wall == 66 || Main.tile[k, l].wall == 68 || Main.tile[k, l].wall == 69 || Main.tile[k, l].wall == 81)
                            {
                                Main.tile[k, l].wall = TileDef.wallByName["Notch:MeteoridonGrassWall"]; //Will need to make this in the future
                            }*/
                            if (Main.tile[k, l].wall == 3 || Main.tile[k, l].wall == 83)
                            {
                                Main.tile[k, l].wall = 28;
                            }
                            if (Main.tile[k, l].type == 2 || Main.tile[k, l].type == 23 || Main.tile[k, l].type == 199)
                            {
                                Main.tile[k, l].type = (ushort)mod.TileType("MeteoridonGrass");
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            else if (Main.tile[k, l].type == 1 || Main.tile[k, l].type == 25 || Main.tile[k, l].type == 203)
                            {
                                Main.tile[k, l].type = (ushort)mod.TileType("MeteoridonStone");
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            /*else if (Main.tile[k, l].type == 53 || Main.tile[k, l].type == 123)
                            {
                                Main.tile[k, l].type = TileDef.byName["Notch:MeteoridonSand"];
                                WorldGen.SquareTileFrame(k, l, true);
                            }*/
                            /*else if (Main.tile[k, l].type == 112 || Main.tile[k, l].type == 234)
                            {
                                Main.tile[k, l].type = TileDef.byName["Notch:MeteoridonSand"];
                                WorldGen.SquareTileFrame(k, l, true);
                            }*/
                            else if (Main.tile[k, l].type == 161 || Main.tile[k, l].type == 163 || Main.tile[k, l].type == 200)
                            {
                                Main.tile[k, l].type = (ushort)mod.TileType("BrownIce");
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                        }
                    }
                }
                value += value2;
                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                if (value2.X > speedX + 1f)
                {
                    value2.X = speedX + 1f;
                }
                if (value2.X < speedX - 1f)
                {
                    value2.X = speedX - 1f;
                }
                if (value.X < -(float)num || value.Y < -(float)num || value.X > (float)(Main.maxTilesX + num) || value.Y > (float)(Main.maxTilesX + num))
                {
                    flag = false;
                }
            }
            
        }
    }
}
