using System;
using System.Linq;
using System.Reflection;
using BiomeLibrary.API;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.World.Generation;

namespace TUA.Biomes
{
    class Plagues : ModBiome
    {
        public override void SetDefault()
        {
            BiomeAlt = BiomeAlternative.evilAlt;
            biomeBlock.Add(mod.TileType("ApocalypseDirt"));
            MinimumTileRequirement = 300;
        }

        public override void BiomeAltWorldGeneration(GenerationProgress progress, PassLegacy pass)
        {
            FieldInfo info = typeof(PassLegacy).GetField("_method", BindingFlags.Instance | BindingFlags.NonPublic);
            WorldGenLegacyMethod method = (WorldGenLegacyMethod)info.GetValue(pass);
            var dungeonSideInfo = method.Method.DeclaringType?.GetFields
                (
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.Static
                )
                .Single(x => x.Name == "dungeonSide");
            int dungeonSide = (int)dungeonSideInfo.GetValue(method.Target);
            progress.Message = "Spreading the plagues...";
            if (WorldGen.crimson)
            {
                CrimPlagues(progress, dungeonSide);
            }
            else
            {
                CorruptPlagues(progress);
            }
        }

        private void CrimPlagues(GenerationProgress progress, int dungeonSide)
        {
            int num = 0;
            int i2;
            while ((double) num < (double) Main.maxTilesX * 0.00045)
            {
                float value = (float) ((double) num / ((double) Main.maxTilesX * 0.00045));
                progress.Set(value);
                bool flag2 = false;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                while (!flag2)
                {
                    int num5 = 0;
                    flag2 = true;
                    int num6 = Main.maxTilesX / 2;
                    int num7 = 200;
                    if (dungeonSide < 0)
                    {
                        num2 = WorldGen.genRand.Next(600, Main.maxTilesX - 320);
                    }
                    else
                    {
                        num2 = WorldGen.genRand.Next(320, Main.maxTilesX - 600);
                    }

                    num3 = num2 - WorldGen.genRand.Next(200) - 100;
                    num4 = num2 + WorldGen.genRand.Next(200) + 100;
                    if (num3 < 285)
                    {
                        num3 = 285;
                    }

                    if (num4 > Main.maxTilesX - 285)
                    {
                        num4 = Main.maxTilesX - 285;
                    }

                    if (dungeonSide < 0 && num3 < 400)
                    {
                        num3 = 400;
                    }
                    else if (dungeonSide > 0 && num3 > Main.maxTilesX - 400)
                    {
                        num3 = Main.maxTilesX - 400;
                    }

                    if (num2 > num6 - num7 && num2 < num6 + num7)
                    {
                        flag2 = false;
                    }

                    if (num3 > num6 - num7 && num3 < num6 + num7)
                    {
                        flag2 = false;
                    }

                    if (num4 > num6 - num7 && num4 < num6 + num7)
                    {
                        flag2 = false;
                    }

                    if (num2 > WorldGen.UndergroundDesertLocation.X &&
                        num2 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }

                    if (num3 > WorldGen.UndergroundDesertLocation.X &&
                        num3 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }

                    if (num4 > WorldGen.UndergroundDesertLocation.X &&
                        num4 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }

                    for (int k = num3; k < num4; k++)
                    {
                        for (int l = 0; l < (int) Main.worldSurface; l += 5)
                        {
                            if (Main.tile[k, l].active() && Main.tileDungeon[(int) Main.tile[k, l].type])
                            {
                                flag2 = false;
                                break;
                            }

                            if (!flag2)
                            {
                                break;
                            }
                        }
                    }

                    if (num5 < 200 &&
                        (int) typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic)
                            .GetValue(null) > num3 &&
                        (int) typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic)
                            .GetValue(null) < num4)
                    {
                        num5++;
                        flag2 = false;
                    }
                }

                CrimStart(num2, (int) WorldGen.worldSurfaceLow - 10);
                for (int m = num3; m < num4; m++)
                {
                    int num8 = (int) WorldGen.worldSurfaceLow;
                    while ((double) num8 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[m, num8].active())
                        {
                            int num9 = num8 + WorldGen.genRand.Next(10, 14);
                            for (int n = num8; n < num9; n++)
                            {
                                if ((Main.tile[m, n].type == 59 || Main.tile[m, n].type == 60) &&
                                    m >= num3 + WorldGen.genRand.Next(5) && m < num4 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[m, n].type = 0;
                                }
                            }

                            break;
                        }

                        num8++;
                    }
                }

                double num10 = Main.worldSurface + 40.0;
                for (int num11 = num3; num11 < num4; num11++)
                {
                    num10 += (double) WorldGen.genRand.Next(-2, 3);
                    if (num10 < Main.worldSurface + 30.0)
                    {
                        num10 = Main.worldSurface + 30.0;
                    }

                    if (num10 > Main.worldSurface + 50.0)
                    {
                        num10 = Main.worldSurface + 50.0;
                    }

                    i2 = num11;
                    bool flag3 = false;
                    int num12 = (int) WorldGen.worldSurfaceLow;
                    while ((double) num12 < num10)
                    {
                        if (Main.tile[i2, num12].active())
                        {
                            if (Main.tile[i2, num12].type == 53 && i2 >= num3 + WorldGen.genRand.Next(5) &&
                                i2 <= num4 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num12].type = (ushort) mod.TileType("ApocalypseDirt");
                            }

                            if (Main.tile[i2, num12].type == 0 && (double) num12 < Main.worldSurface - 1.0 && !flag3)
                            {
                                typeof(WorldGen).GetField("grassSpread", BindingFlags.Static | BindingFlags.NonPublic)
                                    .SetValue(null, 0);
                                WorldGen.SpreadGrass(i2, num12, 0, 199, true, 0);
                            }

                            flag3 = true;
                            if (Main.tile[i2, num12].wall == 216)
                            {
                                Main.tile[i2, num12].wall = 218;
                            }
                            else if (Main.tile[i2, num12].wall == 187)
                            {
                                Main.tile[i2, num12].wall = 221;
                            }

                            if (Main.tile[i2, num12].type == 1)
                            {
                                if (i2 >= num3 + WorldGen.genRand.Next(5) && i2 <= num4 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[i2, num12].type = 203;
                                }
                            }
                            else if (Main.tile[i2, num12].type == 2)
                            {
                                Main.tile[i2, num12].type = 203;
                            }
                            else if (Main.tile[i2, num12].type == 161)
                            {
                                Main.tile[i2, num12].type = 203;
                            }
                            else if (Main.tile[i2, num12].type == 396)
                            {
                                Main.tile[i2, num12].type = 203;
                            }
                            else if (Main.tile[i2, num12].type == 397)
                            {
                                Main.tile[i2, num12].type = 203;
                            }
                        }

                        num12++;
                    }
                }

                int num13 = WorldGen.genRand.Next(10, 15);
                for (int num14 = 0; num14 < num13; num14++)
                {
                    int num15 = 0;
                    bool flag4 = false;
                    int num16 = 0;
                    while (!flag4)
                    {
                        num15++;
                        int num17 = WorldGen.genRand.Next(num3 - num16, num4 + num16);
                        int num18 = WorldGen.genRand.Next((int) (Main.worldSurface - (double) (num16 / 2)),
                            (int) (Main.worldSurface + 100.0 + (double) num16));
                        if (num15 > 100)
                        {
                            num16++;
                            num15 = 0;
                        }

                        if (!Main.tile[num17, num18].active())
                        {
                            while (!Main.tile[num17, num18].active())
                            {
                                num18++;
                            }

                            num18--;
                        }
                        else
                        {
                            while (Main.tile[num17, num18].active() && (double) num18 > Main.worldSurface)
                            {
                                num18--;
                            }
                        }

                        if (num16 > 10 || (Main.tile[num17, num18 + 1].active() && Main.tile[num17, num18 + 1].type == 203))
                        {
                            WorldGen.Place3x2(num17, num18, 26, 1);
                            if (Main.tile[num17, num18].type == 26)
                            {
                                flag4 = true;
                            }
                        }

                        if (num16 > 100)
                        {
                            flag4 = true;
                        }
                    }
                }

                num++;
            }
        }

        public void CrimStart(int i, int j)
        {
            int crimDir = 1;
            int heartCount = (int)typeof(WorldGen).GetField("heartCount", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);
            Vector2[] heartPos = (Vector2[])typeof(WorldGen).GetField("heartPos", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);
            WorldGen.crimson = true;
            int num = j;
            if ((double)num > Main.worldSurface)
            {
                num = (int)Main.worldSurface;
            }
            while (!WorldGen.SolidTile(i, num))
            {
                num++;
            }
            int num2 = num;
            Vector2 vector = new Vector2((float)i, (float)num);
            Vector2 value = new Vector2((float)WorldGen.genRand.Next(-20, 21) * 0.1f, (float)WorldGen.genRand.Next(20, 201) * 0.01f);
            if (value.X < 0f)
            {
                crimDir = -1;
            }
            float num3 = (float)WorldGen.genRand.Next(15, 26);
            bool flag = true;
            int num4 = 0;
            while (flag)
            {
                num3 += (float)WorldGen.genRand.Next(-50, 51) * 0.01f;
                if (num3 < 15f)
                {
                    num3 = 15f;
                }
                if (num3 > 25f)
                {
                    num3 = 25f;
                }
                int num5 = (int)(vector.X - num3 / 2f);
                while ((float)num5 < vector.X + num3 / 2f)
                {
                    int num6 = (int)(vector.Y - num3 / 2f);
                    while ((float)num6 < vector.Y + num3 / 2f)
                    {
                        if (num6 > num2)
                        {
                            if ((double)(Math.Abs((float)num5 - vector.X) + Math.Abs((float)num6 - vector.Y)) < (double)num3 * 0.3)
                            {
                                Main.tile[num5, num6].active(false);
                                Main.tile[num5, num6].wall = 83;
                            }
                            else if ((double)(Math.Abs((float)num5 - vector.X) + Math.Abs((float)num6 - vector.Y)) < (double)num3 * 0.8 && Main.tile[num5, num6].wall != 83)
                            {
                                Main.tile[num5, num6].active(true);
                                Main.tile[num5, num6].type = (WorldGen.genRand.NextBool()) ? (ushort)mod.TileType("ApocalypseDirt") : TileID.Crimstone;
                                if ((double)(Math.Abs((float)num5 - vector.X) + Math.Abs((float)num6 - vector.Y)) < (double)num3 * 0.6)
                                {
                                    Main.tile[num5, num6].wall = 83;
                                }
                            }
                        }
                        else if ((double)(Math.Abs((float)num5 - vector.X) + Math.Abs((float)num6 - vector.Y)) < (double)num3 * 0.3 && Main.tile[num5, num6].active())
                        {
                            Main.tile[num5, num6].active(false);
                            Main.tile[num5, num6].wall = 83;
                        }
                        num6++;
                    }
                    num5++;
                }
                if (vector.X > (float)(i + 50))
                {
                    num4 = -100;
                }
                if (vector.X < (float)(i - 50))
                {
                    num4 = 100;
                }
                if (num4 < 0)
                {
                    value.X -= (float)WorldGen.genRand.Next(20, 51) * 0.01f;
                }
                else if (num4 > 0)
                {
                    value.X += (float)WorldGen.genRand.Next(20, 51) * 0.01f;
                }
                else
                {
                    value.X += (float)WorldGen.genRand.Next(-50, 51) * 0.01f;
                }
                value.Y += (float)WorldGen.genRand.Next(-50, 51) * 0.01f;
                if ((double)value.Y < 0.25)
                {
                    value.Y = 0.25f;
                }
                if (value.Y > 2f)
                {
                    value.Y = 2f;
                }
                if (value.X < -2f)
                {
                    value.X = -2f;
                }
                if (value.X > 2f)
                {
                    value.X = 2f;
                }
                vector += value;
                if ((double)vector.Y > Main.worldSurface + 100.0)
                {
                    flag = false;
                }
            }
            num3 = (float)WorldGen.genRand.Next(40, 55);
            for (int k = 0; k < 50; k++)
            {
                int num7 = (int)vector.X + WorldGen.genRand.Next(-20, 21);
                int num8 = (int)vector.Y + WorldGen.genRand.Next(-20, 21);
                int num9 = (int)((float)num7 - num3 / 2f);
                while ((float)num9 < (float)num7 + num3 / 2f)
                {
                    int num10 = (int)((float)num8 - num3 / 2f);
                    while ((float)num10 < (float)num8 + num3 / 2f)
                    {
                        float num11 = (float)Math.Abs(num9 - num7);
                        float num12 = (float)Math.Abs(num10 - num8);
                        float num13 = 1f + (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
                        float num14 = 1f + (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
                        num11 *= num13;
                        num12 *= num14;
                        double num15 = Math.Sqrt((double)(num11 * num11 + num12 * num12));
                        if (num15 < (double)num3 * 0.25)
                        {
                            Main.tile[num9, num10].active(false);
                            Main.tile[num9, num10].wall = 83;
                        }
                        else if (num15 < (double)num3 * 0.4 && Main.tile[num9, num10].wall != 83)
                        {
                            Main.tile[num9, num10].active(true);
                            Main.tile[num9, num10].type = 203;
                            if (num15 < (double)num3 * 0.35)
                            {
                                Main.tile[num9, num10].wall = 83;
                            }
                        }
                        num10++;
                    }
                    num9++;
                }
            }
            int num16 = WorldGen.genRand.Next(5, 9);
            Vector2[] array = new Vector2[num16];
            for (int l = 0; l < num16; l++)
            {
                int num17 = (int)vector.X;
                int num18 = (int)vector.Y;
                int num19 = 0;
                bool flag2 = true;
                Vector2 vector2 = new Vector2((float)WorldGen.genRand.Next(-20, 21) * 0.15f, (float)WorldGen.genRand.Next(0, 21) * 0.15f);
                while (flag2)
                {
                    vector2 = new Vector2((float)WorldGen.genRand.Next(-20, 21) * 0.15f, (float)WorldGen.genRand.Next(0, 21) * 0.15f);
                    while ((double)(Math.Abs(vector2.X) + Math.Abs(vector2.Y)) < 1.5)
                    {
                        vector2 = new Vector2((float)WorldGen.genRand.Next(-20, 21) * 0.15f, (float)WorldGen.genRand.Next(0, 21) * 0.15f);
                    }
                    flag2 = false;
                    for (int m = 0; m < l; m++)
                    {
                        if ((double)value.X > (double)array[m].X - 0.75 && (double)value.X < (double)array[m].X + 0.75 && (double)value.Y > (double)array[m].Y - 0.75 && (double)value.Y < (double)array[m].Y + 0.75)
                        {
                            flag2 = true;
                            num19++;
                            break;
                        }
                    }
                    if (num19 > 10000)
                    {
                        break;
                    }
                }
                array[l] = vector2;
                CrimVein(new Vector2((float)num17, (float)num18), vector2, heartPos, ref heartCount);
            }
            for (int n = 0; n < heartCount; n++)
            {
                num3 = (float)WorldGen.genRand.Next(16, 21);
                int num20 = (int)heartPos[n].X;
                int num21 = (int)heartPos[n].Y;
                int num22 = (int)((float)num20 - num3 / 2f);
                while ((float)num22 < (float)num20 + num3 / 2f)
                {
                    int num23 = (int)((float)num21 - num3 / 2f);
                    while ((float)num23 < (float)num21 + num3 / 2f)
                    {
                        float num24 = (float)Math.Abs(num22 - num20);
                        float num25 = (float)Math.Abs(num23 - num21);
                        double num26 = Math.Sqrt((double)(num24 * num24 + num25 * num25));
                        if (num26 < (double)num3 * 0.4)
                        {
                            Main.tile[num22, num23].active(true);
                            Main.tile[num22, num23].type = (WorldGen.genRand.NextBool()) ? (ushort)mod.TileType("ApocalypseDirt") : TileID.Crimstone;
                            Main.tile[num22, num23].wall = 83;
                        }
                        num23++;
                    }
                    num22++;
                }
            }
            for (int num27 = 0; num27 < heartCount; num27++)
            {
                num3 = (float)WorldGen.genRand.Next(10, 14);
                int num28 = (int)heartPos[num27].X;
                int num29 = (int)heartPos[num27].Y;
                int num30 = (int)((float)num28 - num3 / 2f);
                while ((float)num30 < (float)num28 + num3 / 2f)
                {
                    int num31 = (int)((float)num29 - num3 / 2f);
                    while ((float)num31 < (float)num29 + num3 / 2f)
                    {
                        float num32 = (float)Math.Abs(num30 - num28);
                        float num33 = (float)Math.Abs(num31 - num29);
                        double num34 = Math.Sqrt((double)(num32 * num32 + num33 * num33));
                        if (num34 < (double)num3 * 0.3)
                        {
                            Main.tile[num30, num31].active(false);
                            Main.tile[num30, num31].wall = 83;
                        }
                        num31++;
                    }
                    num30++;
                }
            }
            for (int num35 = 0; num35 < heartCount; num35++)
            {
                WorldGen.AddShadowOrb((int)heartPos[num35].X, (int)heartPos[num35].Y);
            }
            int num36 = Main.maxTilesX;
            int num37 = 0;
            vector.X = (float)i;
            vector.Y = (float)num2;
            num3 = (float)WorldGen.genRand.Next(25, 35);
            float num38 = (float)WorldGen.genRand.Next(0, 6);
            for (int num39 = 0; num39 < 50; num39++)
            {
                if (num38 > 0f)
                {
                    float num40 = (float)WorldGen.genRand.Next(10, 30) * 0.01f;
                    num38 -= num40;
                    vector.Y -= num40;
                }
                int num41 = (int)vector.X + WorldGen.genRand.Next(-2, 3);
                int num42 = (int)vector.Y + WorldGen.genRand.Next(-2, 3);
                int num43 = (int)((float)num41 - num3 / 2f);
                while ((float)num43 < (float)num41 + num3 / 2f)
                {
                    int num44 = (int)((float)num42 - num3 / 2f);
                    while ((float)num44 < (float)num42 + num3 / 2f)
                    {
                        float num45 = (float)Math.Abs(num43 - num41);
                        float num46 = (float)Math.Abs(num44 - num42);
                        float num47 = 1f + (float)WorldGen.genRand.Next(-20, 21) * 0.005f;
                        float num48 = 1f + (float)WorldGen.genRand.Next(-20, 21) * 0.005f;
                        num45 *= num47;
                        num46 *= num48;
                        double num49 = Math.Sqrt((double)(num45 * num45 + num46 * num46));
                        if (num49 < (double)num3 * 0.2 * ((double)WorldGen.genRand.Next(90, 111) * 0.01))
                        {
                            Main.tile[num43, num44].active(false);
                            Main.tile[num43, num44].wall = 83;
                        }
                        else if (num49 < (double)num3 * 0.45)
                        {
                            if (num43 < num36)
                            {
                                num36 = num43;
                            }
                            if (num43 > num37)
                            {
                                num37 = num43;
                            }
                            if (Main.tile[num43, num44].wall != 83)
                            {
                                Main.tile[num43, num44].active(true);
                                Main.tile[num43, num44].type = 203;
                                if (num49 < (double)num3 * 0.35)
                                {
                                    Main.tile[num43, num44].wall = 83;
                                }
                            }
                        }
                        num44++;
                    }
                    num43++;
                }
            }
            for (int num50 = num36; num50 <= num37; num50++)
            {
                int num51 = num2;
                while ((Main.tile[num50, num51].type == 203 && Main.tile[num50, num51].active()) || Main.tile[num50, num51].wall == 83)
                {
                    num51++;
                }
                int num52 = WorldGen.genRand.Next(15, 20);
                while (!Main.tile[num50, num51].active() && num52 > 0 && Main.tile[num50, num51].wall != 83)
                {
                    num52--;
                    Main.tile[num50, num51].type = 203;
                    Main.tile[num50, num51].active(true);
                    num51++;
                }
            }
            CrimEnt(vector, crimDir);

            typeof(WorldGen).GetField("heartCount", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, heartCount);
            typeof(WorldGen).GetField("heartPos", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, heartPos);
        }

        public static void CrimEnt(Vector2 position, int crimDir)
        {
            float num = 0f;
            float num2 = (float)WorldGen.genRand.Next(6, 11);
            bool flag = true;
            Vector2 value = new Vector2(2f, (float)WorldGen.genRand.Next(-20, 0) * 0.01f);
            value.X *= (float)(-(float)crimDir);
            while (flag)
            {
                num += 1f;
                if (num >= 20f)
                {
                    flag = false;
                }
                num2 += (float)WorldGen.genRand.Next(-10, 11) * 0.02f;
                if (num2 < 6f)
                {
                    num2 = 6f;
                }
                if (num2 > 10f)
                {
                    num2 = 10f;
                }
                int num3 = (int)(position.X - num2 / 2f);
                while ((float)num3 < position.X + num2 / 2f)
                {
                    int num4 = (int)(position.Y - num2 / 2f);
                    while ((float)num4 < position.Y + num2 / 2f)
                    {
                        float num5 = Math.Abs((float)num3 - position.X);
                        float num6 = Math.Abs((float)num4 - position.Y);
                        double num7 = Math.Sqrt((double)(num5 * num5 + num6 * num6));
                        if (num7 < (double)num2 * 0.5 && Main.tile[num3, num4].active() && Main.tile[num3, num4].type == 203)
                        {
                            Main.tile[num3, num4].active(false);
                            flag = true;
                            num = 0f;
                        }
                        num4++;
                    }
                    num3++;
                }
                position += value;
            }
        }

        public void CrimVein(Vector2 position, Vector2 velocity, Vector2[] heartPos, ref int heartCount)
        {
            float num = (float)WorldGen.genRand.Next(15, 26);
            bool flag = true;
            Vector2 vector = velocity;
            Vector2 vector2 = position;
            int num2 = WorldGen.genRand.Next(100, 150);
            if (velocity.Y < 0f)
            {
                num2 -= 25;
            }
            while (flag)
            {
                num += (float)WorldGen.genRand.Next(-50, 51) * 0.02f;
                if (num < 15f)
                {
                    num = 15f;
                }
                if (num > 25f)
                {
                    num = 25f;
                }
                int num3 = (int)(position.X - num / 2f);
                while ((float)num3 < position.X + num / 2f)
                {
                    int num4 = (int)(position.Y - num / 2f);
                    while ((float)num4 < position.Y + num / 2f)
                    {
                        float num5 = Math.Abs((float)num3 - position.X);
                        float num6 = Math.Abs((float)num4 - position.Y);
                        double num7 = Math.Sqrt((double)(num5 * num5 + num6 * num6));
                        if (num7 < (double)num * 0.2)
                        {
                            Main.tile[num3, num4].active(false);
                            Main.tile[num3, num4].wall = 83;
                        }
                        else if (num7 < (double)num * 0.5 && Main.tile[num3, num4].wall != 83)
                        {
                            Main.tile[num3, num4].active(true);
                            Main.tile[num3, num4].type = (ushort)mod.TileType("ApocalypseDirt"); ;
                            if (num7 < (double)num * 0.4)
                            {
                                Main.tile[num3, num4].wall = 83;
                            }
                        }
                        num4++;
                    }
                    num3++;
                }
                velocity.X += (float)WorldGen.genRand.Next(-50, 51) * 0.05f;
                velocity.Y += (float)WorldGen.genRand.Next(-50, 51) * 0.05f;
                if ((double)velocity.Y < (double)vector.Y - 0.75)
                {
                    velocity.Y = vector.Y - 0.75f;
                }
                if ((double)velocity.Y > (double)vector.Y + 0.75)
                {
                    velocity.Y = vector.Y + 0.75f;
                }
                if ((double)velocity.X < (double)vector.X - 0.75)
                {
                    velocity.X = vector.X - 0.75f;
                }
                if ((double)velocity.X > (double)vector.X + 0.75)
                {
                    velocity.X = vector.X + 0.75f;
                }
                position += velocity;
                if (Math.Abs(position.X - vector2.X) + Math.Abs(position.Y - vector2.Y) > (float)num2)
                {
                    flag = false;
                }
            }
            heartPos[heartCount] = position;
            heartCount++;
        }

        public void CorruptPlagues(GenerationProgress progress)
        {
            int i2;
            int num19 = 0;
            while ((double)num19 < (double)Main.maxTilesX * 0.00045)
            {
                float value2 = (float)((double)num19 / ((double)Main.maxTilesX * 0.00045));
                progress.Set(value2);
                bool flag5 = false;
                int num20 = 0;
                int num21 = 0;
                int num22 = 0;
                while (!flag5)
                {
                    int num23 = 0;
                    flag5 = true;
                    int num24 = Main.maxTilesX / 2;
                    int num25 = 200;
                    num20 = WorldGen.genRand.Next(320, Main.maxTilesX - 320);
                    num21 = num20 - WorldGen.genRand.Next(200) - 100;
                    num22 = num20 + WorldGen.genRand.Next(200) + 100;
                    if (num21 < 285)
                    {
                        num21 = 285;
                    }
                    if (num22 > Main.maxTilesX - 285)
                    {
                        num22 = Main.maxTilesX - 285;
                    }
                    if (num20 > num24 - num25 && num20 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num21 > num24 - num25 && num21 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num22 > num24 - num25 && num22 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num20 > WorldGen.UndergroundDesertLocation.X && num20 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    if (num21 > WorldGen.UndergroundDesertLocation.X && num21 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    if (num22 > WorldGen.UndergroundDesertLocation.X && num22 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    for (int num26 = num21; num26 < num22; num26++)
                    {
                        for (int num27 = 0; num27 < (int)Main.worldSurface; num27 += 5)
                        {
                            if (Main.tile[num26, num27].active() && Main.tileDungeon[(int)Main.tile[num26, num27].type])
                            {
                                flag5 = false;
                                break;
                            }
                            if (!flag5)
                            {
                                break;
                            }
                        }
                    }
                    if (num23 < 200 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) > num21 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) < num22)
                    {
                        num23++;
                        flag5 = false;
                    }
                }
                int num28 = 0;
                for (int num29 = num21; num29 < num22; num29++)
                {
                    if (num28 > 0)
                    {
                        num28--;
                    }
                    if (num29 == num20 || num28 == 0)
                    {
                        int num30 = (int)WorldGen.worldSurfaceLow;
                        while ((double)num30 < Main.worldSurface - 1.0)
                        {
                            if (Main.tile[num29, num30].active() || Main.tile[num29, num30].wall > 0)
                            {
                                if (num29 == num20)
                                {
                                    num28 = 20;
                                    ChasmRunner(num29, num30, WorldGen.genRand.Next(150) + 150, true);
                                    break;
                                }
                                if (WorldGen.genRand.Next(35) == 0 && num28 == 0)
                                {
                                    num28 = 30;
                                    bool makeOrb = true;
                                    ChasmRunner(num29, num30, WorldGen.genRand.Next(50) + 50, makeOrb);
                                    break;
                                }
                                break;
                            }
                            else
                            {
                                num30++;
                            }
                        }
                    }
                    int num31 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num31 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[num29, num31].active())
                        {
                            int num32 = num31 + WorldGen.genRand.Next(10, 14);
                            for (int num33 = num31; num33 < num32; num33++)
                            {
                                if ((Main.tile[num29, num33].type == 59 || Main.tile[num29, num33].type == 60) && num29 >= num21 + WorldGen.genRand.Next(5) && num29 < num22 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[num29, num33].type = 0;
                                }
                            }
                            break;
                        }
                        num31++;
                    }
                }
                double num34 = Main.worldSurface + 40.0;
                for (int num35 = num21; num35 < num22; num35++)
                {
                    num34 += (double)WorldGen.genRand.Next(-2, 3);
                    if (num34 < Main.worldSurface + 30.0)
                    {
                        num34 = Main.worldSurface + 30.0;
                    }
                    if (num34 > Main.worldSurface + 50.0)
                    {
                        num34 = Main.worldSurface + 50.0;
                    }
                    i2 = num35;
                    bool flag6 = false;
                    int num36 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num36 < num34)
                    {
                        if (Main.tile[i2, num36].active())
                        {
                            if (Main.tile[i2, num36].type == 53 && i2 >= num21 + WorldGen.genRand.Next(5) && i2 <= num22 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num36].type = 112;
                            }
                            if (Main.tile[i2, num36].type == 0 && (double)num36 < Main.worldSurface - 1.0 && !flag6)
                            {
                                typeof(WorldGen).GetField("grassSpread", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 0);
                                WorldGen.SpreadGrass(i2, num36, 0, 23, true, 0);
                            }
                            flag6 = true;
                            if (Main.tile[i2, num36].type == 1 && i2 >= num21 + WorldGen.genRand.Next(5) && i2 <= num22 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num36].type = 25;
                            }
                            if (Main.tile[i2, num36].wall == 216)
                            {
                                Main.tile[i2, num36].wall = 217;
                            }
                            else if (Main.tile[i2, num36].wall == 187)
                            {
                                Main.tile[i2, num36].wall = 220;
                            }
                            if (Main.tile[i2, num36].type == 2)
                            {
                                Main.tile[i2, num36].type = 23;
                            }
                            if (Main.tile[i2, num36].type == 161)
                            {
                                Main.tile[i2, num36].type = 163;
                            }
                            else if (Main.tile[i2, num36].type == 396)
                            {
                                Main.tile[i2, num36].type = 400;
                            }
                            else if (Main.tile[i2, num36].type == 397)
                            {
                                Main.tile[i2, num36].type = 398;
                            }
                        }
                        num36++;
                    }
                }
                for (int num37 = num21; num37 < num22; num37++)
                {
                    for (int num38 = 0; num38 < Main.maxTilesY - 50; num38++)
                    {
                        if (Main.tile[num37, num38].active() && Main.tile[num37, num38].type == 31)
                        {
                            int num39 = num37 - 13;
                            int num40 = num37 + 13;
                            int num41 = num38 - 13;
                            int num42 = num38 + 13;
                            for (int num43 = num39; num43 < num40; num43++)
                            {
                                if (num43 > 10 && num43 < Main.maxTilesX - 10)
                                {
                                    for (int num44 = num41; num44 < num42; num44++)
                                    {
                                        if (Math.Abs(num43 - num37) + Math.Abs(num44 - num38) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Main.tile[num43, num44].type != 31)
                                        {
                                            Main.tile[num43, num44].active(true);
                                            Main.tile[num43, num44].type = 25;
                                            if (Math.Abs(num43 - num37) <= 1 && Math.Abs(num44 - num38) <= 1)
                                            {
                                                Main.tile[num43, num44].active(false);
                                            }
                                        }
                                        if (Main.tile[num43, num44].type != 31 && Math.Abs(num43 - num37) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num44 - num38) <= 2 + WorldGen.genRand.Next(3))
                                        {
                                            Main.tile[num43, num44].active(false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                num19++;
            }
        }

        public void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if (!makeOrb)
            {
                flag2 = true;
            }
            float num = (float)steps;
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            Vector2 value2;
            value2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            value2.Y = (float)WorldGen.genRand.Next(11) * 0.2f + 0.5f;
            int num2 = 5;
            double num3 = (double)(WorldGen.genRand.Next(5) + 7);
            while (num3 > 0.0)
            {
                if (num > 0f)
                {
                    num3 += (double)WorldGen.genRand.Next(3);
                    num3 -= (double)WorldGen.genRand.Next(3);
                    if (num3 < 7.0)
                    {
                        num3 = 7.0;
                    }
                    if (num3 > 20.0)
                    {
                        num3 = 20.0;
                    }
                    if (num == 1f && num3 < 10.0)
                    {
                        num3 = 10.0;
                    }
                }
                else if ((double)value.Y > Main.worldSurface + 45.0)
                {
                    num3 -= (double)WorldGen.genRand.Next(4);
                }
                if ((double)value.Y > Main.rockLayer && num > 0f)
                {
                    num = 0f;
                }
                num -= 1f;
                if (!flag && (double)value.Y > Main.worldSurface + 20.0)
                {
                    flag = true;
                    ChasmRunnerSideways((int)value.X, (int)value.Y, -1, WorldGen.genRand.Next(20, 40));
                    ChasmRunnerSideways((int)value.X, (int)value.Y, 1, WorldGen.genRand.Next(20, 40));
                }
                int num4;
                int num5;
                int num6;
                int num7;
                if (num > (float)num2)
                {
                    num4 = (int)((double)value.X - num3 * 0.5);
                    num5 = (int)((double)value.X + num3 * 0.5);
                    num6 = (int)((double)value.Y - num3 * 0.5);
                    num7 = (int)((double)value.Y + num3 * 0.5);
                    if (num4 < 0)
                    {
                        num4 = 0;
                    }
                    if (num5 > Main.maxTilesX - 1)
                    {
                        num5 = Main.maxTilesX - 1;
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
                            if ((double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < num3 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != 31 && Main.tile[k, l].type != 22)
                            {
                                Main.tile[k, l].active(false);
                            }
                        }
                    }
                }
                if (num <= 2f && (double)value.Y < Main.worldSurface + 45.0)
                {
                    num = 2f;
                }
                if (num <= 0f)
                {
                    if (!flag2)
                    {
                        flag2 = true;
                        WorldGen.AddShadowOrb((int)value.X, (int)value.Y);
                    }
                    else if (!flag3)
                    {
                        flag3 = false;
                        bool flag4 = false;
                        int num8 = 0;
                        while (!flag4)
                        {
                            int num9 = WorldGen.genRand.Next((int)value.X - 25, (int)value.X + 25);
                            int num10 = WorldGen.genRand.Next((int)value.Y - 50, (int)value.Y);
                            if (num9 < 5)
                            {
                                num9 = 5;
                            }
                            if (num9 > Main.maxTilesX - 5)
                            {
                                num9 = Main.maxTilesX - 5;
                            }
                            if (num10 < 5)
                            {
                                num10 = 5;
                            }
                            if (num10 > Main.maxTilesY - 5)
                            {
                                num10 = Main.maxTilesY - 5;
                            }
                            if ((double)num10 > Main.worldSurface)
                            {
                                WorldGen.Place3x2(num9, num10, 26, 0);
                                if (Main.tile[num9, num10].type == 26)
                                {
                                    flag4 = true;
                                }
                                else
                                {
                                    num8++;
                                    if (num8 >= 10000)
                                    {
                                        flag4 = true;
                                    }
                                }
                            }
                            else
                            {
                                flag4 = true;
                            }
                        }
                    }
                }
                value += value2;
                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
                if ((double)value2.X > 0.3)
                {
                    value2.X = 0.3f;
                }
                if ((double)value2.X < -0.3)
                {
                    value2.X = -0.3f;
                }
                num4 = (int)((double)value.X - num3 * 1.1);
                num5 = (int)((double)value.X + num3 * 1.1);
                num6 = (int)((double)value.Y - num3 * 1.1);
                num7 = (int)((double)value.Y + num3 * 1.1);
                if (num4 < 1)
                {
                    num4 = 1;
                }
                if (num5 > Main.maxTilesX - 1)
                {
                    num5 = Main.maxTilesX - 1;
                }
                if (num6 < 0)
                {
                    num6 = 0;
                }
                if (num7 > Main.maxTilesY)
                {
                    num7 = Main.maxTilesY;
                }
                for (int m = num4; m < num5; m++)
                {
                    for (int n = num6; n < num7; n++)
                    {
                        if ((double)(Math.Abs((float)m - value.X) + Math.Abs((float)n - value.Y)) < num3 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            if (Main.tile[m, n].type != 25 && n > j + WorldGen.genRand.Next(3, 20))
                            {
                                Main.tile[m, n].active(true);
                            }
                            if (steps <= num2)
                            {
                                Main.tile[m, n].active(true);
                            }
                            if (Main.tile[m, n].type != 31)
                            {
                                Main.tile[m, n].type = (ushort)((WorldGen._genRand.NextFloat() < 0.9f) ? mod.TileType("ApocalypseDirt") : 25);
                            }
                        }
                    }
                }
                for (int num11 = num4; num11 < num5; num11++)
                {
                    for (int num12 = num6; num12 < num7; num12++)
                    {
                        if ((double)(Math.Abs((float)num11 - value.X) + Math.Abs((float)num12 - value.Y)) < num3 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            if (Main.tile[num11, num12].type != 31)
                            {
                                Main.tile[num11, num12].type = (ushort)((WorldGen._genRand.NextFloat() < 0.9f) ? mod.TileType("ApocalypseDirt") : 25);
                            }
                            if (steps <= num2)
                            {
                                Main.tile[num11, num12].active(true);
                            }
                            if (num12 > j + WorldGen.genRand.Next(3, 20))
                            {
                                Main.tile[num11, num12].wall = 3;
                            }
                        }
                    }
                }
            }
        }

        public void ChasmRunnerSideways(int i, int j, int direction, int steps)
        {
            float num = (float)steps;
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            Vector2 value2;
            value2.X = (float)WorldGen.genRand.Next(10, 21) * 0.1f * (float)direction;
            value2.Y = (float)WorldGen.genRand.Next(-10, 10) * 0.01f;
            double num2 = (double)(WorldGen.genRand.Next(5) + 7);
            while (num2 > 0.0)
            {
                if (num > 0f)
                {
                    num2 += (double)WorldGen.genRand.Next(3);
                    num2 -= (double)WorldGen.genRand.Next(3);
                    if (num2 < 7.0)
                    {
                        num2 = 7.0;
                    }
                    if (num2 > 20.0)
                    {
                        num2 = 20.0;
                    }
                    if (num == 1f && num2 < 10.0)
                    {
                        num2 = 10.0;
                    }
                }
                else
                {
                    num2 -= (double)WorldGen.genRand.Next(4);
                }
                if ((double)value.Y > Main.rockLayer && num > 0f)
                {
                    num = 0f;
                }
                num -= 1f;
                int num3 = (int)((double)value.X - num2 * 0.5);
                int num4 = (int)((double)value.X + num2 * 0.5);
                int num5 = (int)((double)value.Y - num2 * 0.5);
                int num6 = (int)((double)value.Y + num2 * 0.5);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                if (num4 > Main.maxTilesX - 1)
                {
                    num4 = Main.maxTilesX - 1;
                }
                if (num5 < 0)
                {
                    num5 = 0;
                }
                if (num6 > Main.maxTilesY)
                {
                    num6 = Main.maxTilesY;
                }
                for (int k = num3; k < num4; k++)
                {
                    for (int l = num5; l < num6; l++)
                    {
                        if ((double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < num2 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != 31 && Main.tile[k, l].type != 22)
                        {
                            Main.tile[k, l].active(false);
                        }
                    }
                }
                value += value2;
                value2.Y += (float)WorldGen.genRand.Next(-10, 10) * 0.1f;
                if (value.Y < (float)(j - 20))
                {
                    value2.Y += (float)WorldGen.genRand.Next(20) * 0.01f;
                }
                if (value.Y > (float)(j + 20))
                {
                    value2.Y -= (float)WorldGen.genRand.Next(20) * 0.01f;
                }
                if ((double)value2.Y < -0.5)
                {
                    value2.Y = -0.5f;
                }
                if ((double)value2.Y > 0.5)
                {
                    value2.Y = 0.5f;
                }
                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
                if (direction == -1)
                {
                    if ((double)value2.X > -0.5)
                    {
                        value2.X = -0.5f;
                    }
                    if (value2.X < -2f)
                    {
                        value2.X = -2f;
                    }
                }
                else if (direction == 1)
                {
                    if ((double)value2.X < 0.5)
                    {
                        value2.X = 0.5f;
                    }
                    if (value2.X > 2f)
                    {
                        value2.X = 2f;
                    }
                }
                num3 = (int)((double)value.X - num2 * 1.1);
                num4 = (int)((double)value.X + num2 * 1.1);
                num5 = (int)((double)value.Y - num2 * 1.1);
                num6 = (int)((double)value.Y + num2 * 1.1);
                if (num3 < 1)
                {
                    num3 = 1;
                }
                if (num4 > Main.maxTilesX - 1)
                {
                    num4 = Main.maxTilesX - 1;
                }
                if (num5 < 0)
                {
                    num5 = 0;
                }
                if (num6 > Main.maxTilesY)
                {
                    num6 = Main.maxTilesY;
                }
                for (int m = num3; m < num4; m++)
                {
                    for (int n = num5; n < num6; n++)
                    {
                        if ((double)(Math.Abs((float)m - value.X) + Math.Abs((float)n - value.Y)) < num2 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[m, n].wall != 3)
                        {
                            if (Main.tile[m, n].type != 25 && n > j + WorldGen.genRand.Next(3, 20))
                            {
                                Main.tile[m, n].active(true);
                            }
                            Main.tile[m, n].active(true);
                            if (Main.tile[m, n].type != 31 && Main.tile[m, n].type != 22)
                            {
                                Main.tile[m, n].type = 25;
                            }
                            if (Main.tile[m, n].wall == 2)
                            {
                                Main.tile[m, n].wall = 0;
                            }
                        }
                    }
                }
                for (int num7 = num3; num7 < num4; num7++)
                {
                    for (int num8 = num5; num8 < num6; num8++)
                    {
                        if ((double)(Math.Abs((float)num7 - value.X) + Math.Abs((float)num8 - value.Y)) < num2 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[num7, num8].wall != 3)
                        {
                            if (Main.tile[num7, num8].type != 31 && Main.tile[num7, num8].type != 22)
                            {
                                Main.tile[num7, num8].type = (ushort)((WorldGen._genRand.NextFloat() < 0.9f) ? mod.TileType("ApocalypseDirt") : 25); 
                            }
                            Main.tile[num7, num8].active(true);
                            WorldGen.PlaceWall(num7, num8, 3, true);
                        }
                    }
                }
            }
            if (WorldGen.genRand.Next(3) == 0)
            {
                int num9 = (int)value.X;
                int num10 = (int)value.Y;
                while (!Main.tile[num9, num10].active())
                {
                    num10++;
                }
                WorldGen.TileRunner(num9, num10, (double)WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), 22, false, 0f, 0f, false, true);
            }
        }
    }

    public class Minecraft : ModBiome
    {
        public override void SetDefault()
        {
            BiomeAlt = BiomeAlternative.evilAlt;
            biomeBlock.Add(mod.TileType("ApocalypseDirt"));
            MinimumTileRequirement = 300;
            BiomeName = "Minecraft";
        }
    }

    public class CP: ModBiome
    {
        public override void SetDefault()
        {
            BiomeAlt = BiomeAlternative.evilAlt;
            biomeBlock.Add(mod.TileType("ApocalypseDirt"));
            MinimumTileRequirement = 300;
            BiomeName = "Club Penguin";
        }
    }
}
