using System;
using BiomeLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Dimlibs;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse
{
    class TUAWorld : ModWorld
    {
        public static bool apocalypseMoon = false;
        public static int apocalypseMoonPoint = 0;
        public static bool Apocalypse;
        public static bool UltraMode;
        public static int EoCDeath;

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
            Func<bool> c = () => Main.LocalPlayer.GetModPlayer<DimPlayer>().getCurrentDimension() == "solar";
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
            BiomeLibs.RegisterNewBiome("Stardust", 200, mod);
        }

        public override TagCompound Save()
        {
            TagCompound tc = new TagCompound();
            tc.Add("UltraMode", UltraMode);
            tc.Add("EoADowned", Apocalypse);
            tc.Add("UltraEoCDowned", EoCDeath);
            //tc.Add("apocalypseMoon", apocalypseMoon);
            return tc;
        }

        public override void Load(TagCompound tag)
        {
            UltraMode = tag.GetBool("UltraMode");
            Apocalypse = tag.GetBool("EoADowned");
            EoCDeath = tag.GetInt("UltraEoCDowned");


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
            ModExtension.ForceSpawnNPC();
        }

        public static void solarWorldGen(Mod mod)
        {
            setDimInfo();
            solarBlockReplacer(mod);
            Volcano(mod, 100, Main.rand.Next(200, 250));
            Volcano(mod, Main.maxTilesX - 100, Main.rand.Next(250, 300));

            Volcano(mod, Main.maxTilesY - Main.rand.Next(-200, 200), Main.rand.Next(200, 250));
            Volcano(mod, Main.maxTilesX - Main.rand.Next(400, 600), Main.rand.Next(200, 250));

            Volcano(mod, Main.maxTilesX - Main.rand.Next(900, 1000), Main.rand.Next(150, 200));
        }

        private static void setDimInfo()
        {
            NPC.downedBoss1 = true;
            NPC.downedBoss2 = true;
            NPC.downedBoss3 = true;
            NPC.downedGoblins = true;
            NPC.downedFrost = true;
            Main.hardMode = true;
            NPC.downedChristmasIceQueen = true;
            NPC.downedChristmasSantank = true;
            NPC.downedChristmasTree = true;
            NPC.downedClown = true;
            NPC.downedGolemBoss = true;
        }

        private static void solarBlockReplacer(Mod mod)
        {

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile T = Main.tile[i, j];
                    if (T == null)
                        T = new Tile();
                    if (T.active())
                    {
                        if (T.type == 0 || T.type == TileID.Sand || T.type == TileID.SnowBlock || T.type == TileID.Mud || T.type == TileID.ClayBlock || T.type == TileID.Grass || T.type == TileID.JungleGrass || T.type == TileID.CorruptGrass || T.type == TileID.FleshGrass || T.type == TileID.MushroomGrass)
                        {
                            T.type = (ushort)mod.TileType("SolarDirt");
                        }
                        if (T.type == TileID.Stone || T.type == TileID.Crimstone || T.type == TileID.Ebonstone || T.type == TileID.IceBlock || T.type == TileID.CorruptIce || T.type == TileID.FleshIce || T.type == TileID.HallowedIce || T.type == TileID.Sandstone)
                        {
                            T.type = (ushort)mod.TileType("SolarRock");
                        }
                        if (Main.tile[i, j].liquidType() == 0)
                        {
                            Main.tile[i, j].liquidType(1);
                            Main.tile[i, j].liquid = 255;
                        }
                    }
                }
            }
        }

        private static void Volcano(Mod mod, int i, int j)
        {
            int x = i;
            int y = j;

            int volcanoBlock = mod.TileType("SolarRock");

            for (int yPos = j; yPos < j + Main.maxTilesY - 200; yPos += 4)
            {
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
