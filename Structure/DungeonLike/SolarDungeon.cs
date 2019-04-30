using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;

namespace TUA.Structure.DungeonLike
{
    class SolarDungeon : MicroBiome
    {

        public const ushort TESTBLOCK = TileID.CopperBrick;


        public struct DungeonLink
        {
            public Rectangle boundary;
            public Point16 startingPosition;
            public Point16 endPosition;
        }

        public struct DungeonRoom
        {
            public Rectangle boundary;
            public int width;
            public int height;
            public int x;
            public int y;
            public bool decorated;
        }

        public override bool Place(Point origin, StructureMap structures)
        {

            return true;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public static void GenerateStructure()
        {
            List<DungeonRoom> DungeonRoom = new List<DungeonRoom>();
            Point16 initialStartingPoint = new Point16(Main.dungeonX, Main.dungeonY); 


        }

        public static void GenerateSpike(Vector2 initial)
        {
            int thiccness = Main.rand.Next(7, 10);
            int x = (int) initial.X;
            int y = (int) initial.Y;
            bool right = Main.rand.NextBool();

            if (right)
            {
                while (thiccness != 0)
                {
                    for (int trueX = x - thiccness; trueX < x + thiccness; trueX++)
                    {
                        WorldGen.PlaceTile(trueX, y, TESTBLOCK, true, true);
                    }

                    if (Main.rand.Next(thiccness / 2) == 0)
                    {
                        x++;
                    }

                    if (Main.rand.Next(3) == 0)
                    {
                        thiccness--;
                    }

                    y--;
                }
            }
            else
            {
                while (thiccness != 0)
                {
                    for (int trueX = x - thiccness; trueX < x + thiccness; trueX++)
                    {
                        WorldGen.PlaceTile(trueX, y, TESTBLOCK, true, true);
                    }

                    if (Main.rand.Next(thiccness / 2) == 0)
                    {
                        x--;
                    }

                    if (Main.rand.Next(3) == 0)
                    {
                        thiccness--;
                    }

                    y--;
                }
            }
        }

        public static void PlaceALine(Vector2 initial, Vector2 end)
        {
            float slope = (end.Y - initial.Y) / (end.X - initial.X);

            float magnitudeInitial = (float) Math.Sqrt(initial.X * initial.X + initial.Y * initial.Y);
            float magnitudeEnd = (float) Math.Sqrt(end.X * end.X + end.Y * end.Y);
            float dotProduct = initial.X * initial.Y + end.X * end.Y;
            float angle = (float) MathHelper.ToDegrees((float) Math.Atan2(end.Y - initial.Y, end.X - initial.X));

            Main.NewText("Angle: " + angle);

            float dist =
                (float) Math.Sqrt((end.X - initial.X) * (end.X - initial.X) +
                                  (end.Y - initial.Y) * (end.Y - initial.Y));
            float distanceTile = dist;
            float iterationX = (end.X - initial.X) / (distanceTile + 1);
            float iterationY = (end.Y - initial.Y) / (distanceTile + 1);

            if (angle > -45 && angle < 45)
            {
                GenerateAlongYAxis(initial, distanceTile, iterationX, iterationY);
                return;
            }

            GenerateAlongXAxis(initial, distanceTile, iterationX, iterationY);
        }

        private static void GenerateAlongXAxis(Vector2 initial, float distanceTile, float iterationX, float iterationY)
        {
            for (int k = 0; k < distanceTile + 1; k++)
            {
                int x = (int) (initial.X + iterationX * k);
                int y = (int) (initial.Y + iterationY * k);
                for (int trueX = x - 10; trueX < x + 10; trueX++)
                {
                    if (trueX > x - 5 && trueX < x + 5)
                    {
                        WorldGen.KillTile(trueX, y, false, false, true);
                        WorldGen.KillWall(trueX, y);
                        WorldGen.PlaceWall(trueX, y, WallID.AmberGemspark);
                    }
                    else
                    {
                        if (Main.tile[trueX, y].wall != WallID.AmberGemspark)
                            WorldGen.PlaceTile(trueX, y, TESTBLOCK, true, true);
                    }
                }
            }
        }

        private static void GenerateAlongYAxis(Vector2 initial, float distanceTile, float iterationX, float iterationY)
        {
            for (int k = 0; k < distanceTile + 1; k++)
            {
                int x = (int) (initial.X + iterationX * k);
                int y = (int) (initial.Y + iterationY * k);
                for (int trueY = y - 10; trueY < y + 10; trueY++)
                {
                    if (trueY > y - 5 && trueY < y + 5)
                    {
                        WorldGen.KillTile(x, trueY, false, false, true);
                        WorldGen.KillWall(x, trueY);
                        WorldGen.PlaceWall(x, trueY, WallID.AmberGemspark);
                    }
                    else
                    {
                        if (Main.tile[x, trueY].wall != WallID.AmberGemspark)
                            WorldGen.PlaceTile(x, trueY, TESTBLOCK, true, true);
                    }
                }
            }
        }

        public static void PlaceRoomBox(int x, int y, int width, int height, int wallTiccness) //extra ticc
        {
            wallTiccness = 0;
            /*for (int trueX = x - width / 2; trueX < x + width / 2; trueX++)
            {
                for (int trueY = y - wallTiccness; trueY < y + height; trueY++)
                {
                    if (trueX < x + wallTiccness || trueY < y + wallTiccness ||
                        trueX > x + width - wallTiccness || trueY > y + height - wallTiccness)
                    {
                        WorldGen.KillTile(trueX, trueY, false, false, true);
                        WorldGen.KillWall(trueX, trueY);
                        WorldGen.PlaceWall(trueX, trueY, WallID.AmberGemspark);
                    }
                    else
                    {
                        WorldGen.PlaceTile(trueX, trueY, TESTBLOCK, true, true);
                    }
                }
            }*/
            for (int trueX = x; trueX < x + width; trueX++)
            {
                for (int trueY = y; trueY < y + height; trueY++)
                {
                    if ((trueX == x || trueY == y) || (trueX == x + width - 1 || trueY == y + height - 1))
                    {
                        if (Main.tile[x, trueY].wall != WallID.AmberGemspark)
                            WorldGen.PlaceTile(trueX, trueY, TESTBLOCK, true, true);
                    }
                    else
                    {
                        WorldGen.KillTile(trueX, trueY, false, false, true);
                        WorldGen.KillWall(trueX, trueY);
                        WorldGen.PlaceWall(trueX, trueY, WallID.AmberGemspark);
                    }
                }
            }
        }
    }
}
