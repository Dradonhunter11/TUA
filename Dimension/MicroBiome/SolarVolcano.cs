using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TUA.API;

namespace TUA.Dimension.MicroBiome
{
    internal class SolarVolcano : Terraria.World.Generation.MicroBiome
    {
        internal class VolcanoBranch
        {
            internal Point currentPoint;
            internal Point originPoint;
            internal int maxStep;
            internal int currentStep;
            internal int length;
            internal bool left;
            internal bool horizontal;
            internal int loop;

            public VolcanoBranch(Point point, int maxStep, int length, bool left, bool horizontal)
            {
                currentStep = 0;
                currentPoint = point;
                originPoint = point;
                this.maxStep = maxStep;
                this.left = left;
                this.horizontal = horizontal;
                this.length = length;
                this.loop = 0;
            }

            public VolcanoBranch(VolcanoBranch previousBranch, int length, bool horizontal)
            {
                this.length = length;
                this.horizontal = horizontal;
                this.currentPoint = previousBranch.currentPoint;
                this.originPoint = currentPoint;
                this.maxStep = previousBranch.maxStep;
                this.currentStep = previousBranch.currentStep++;
                this.currentStep += 1;
                this.left = previousBranch.left;
                this.loop = previousBranch.loop++;
                this.loop += 1;
            }
        }

        public static readonly int NUMBER_OF_VOLCANO_SMALL = 2;
        public static readonly int NUMBER_OF_VOLCANO_MEDIUM = 3;
        public static readonly int NUMBER_OF_VOLCANO_LARGE = 4;

        private const int WIDTH = 200;
        private const int HEIGHT = 150;

        internal Mod mod = TerrariaUltraApocalypse.instance;

        public override bool Place(Point origin, StructureMap structures)
        {
            Rectangle structureBound = new Rectangle(origin.X - WIDTH / 2, origin.Y - HEIGHT / 2, WIDTH, HEIGHT);

            /*if (!structures.CanPlace(structureBound))
            {
                return false;
            }*/


            Dictionary<ushort, int> environmentTile = new Dictionary<ushort, int>();
            /*WorldUtils.Gen(structureBound.Location, new Shapes.Rectangle(WIDTH, HEIGHT), new Actions.TileScanner(
                new ushort[]
                {
                    (ushort)ModLoader.GetMod("TUA").TileType("SolarDirt"),
                    (ushort)ModLoader.GetMod("TUA").TileType("SolarRock")
                }
            ).Output(environmentTile));

            int total = 0;
            foreach (KeyValuePair<ushort, int> pair in environmentTile)
            {
                total += pair.Value;
            }

            if (total < 2000 && origin.Y < Main.worldSurface)
            {
                return false;
            }*/

            GenerateVolcano(origin);
            return true;
        }

        public void GenerateVolcano(Point origin)
        {
            GenerateMountainShape(origin, out Point highestPoint, mod);
            GenerateBranch(highestPoint, out int depth);
            GenerateTheDeepTunnel(highestPoint, depth);
            ShapeTheTop(highestPoint);
        }

        internal void GenerateMountainShape(Point origin, out Point highestPoint, Mod mod)
        {
            highestPoint = new Point(0, 0);

            int amplitude = 50;
            int x = 0 - amplitude;
            int y = 0;
            int startingX = x;


            const float xModifer = 0.03f;
            float whileLoopStop = 2 / xModifer;

            ILog logger = LogManager.GetLogger("Volcano Generator");

            do
            {
                x++;
                y = (int)(amplitude + Math.Sin(x * xModifer) * amplitude);
                logger.Info("[X : " + x + ", Y : " + y + " ]");
                if (y > highestPoint.Y)
                {
                    highestPoint = new Point(x, y);
                }

                for (int tileY = origin.Y; tileY > origin.Y - y; tileY--)
                {
                    if (WorldGen.InWorld(origin.X + x, tileY))
                    {
                        if (Main.tile[origin.X + x, tileY] == null)
                        {
                            Main.tile[origin.X + x, tileY] = new Tile();
                        }

                        WorldGen.PlaceTile(origin.X + x, tileY, mod.TileID("SolarRock"), false, true);
                    }
                }
            } while (x <= startingX + whileLoopStop * Math.PI);

            highestPoint.Y = origin.Y - highestPoint.Y;
            highestPoint.X = origin.X + highestPoint.X;
            highestPoint.X += 7;
        }

        internal void GenerateBranch(Point origin, out int depth)
        {
            Point volcanoBranchOrigin = origin;
            depth = WorldGen.genRand.Next(100, 200) * 2;
            int numberOFBranch = 0;
            int localY = 0;
            for (int y = volcanoBranchOrigin.Y; y < origin.Y + depth; y++)
            {

                if (WorldGen.genRand.Next(10) == 0)
                {
                    GenerateBranch(new VolcanoBranch(volcanoBranchOrigin, WorldGen.genRand.Next(localY / 20), WorldGen.genRand.Next(10, 15), WorldGen.genRand.NextBool(), WorldGen.genRand.NextBool()));
                    numberOFBranch++;
                }

                localY++;
                volcanoBranchOrigin.Y++;
            }
        }

        internal void GenerateBranch(VolcanoBranch branch)
        {
            ILog log = LogManager.GetLogger("Volcano Gen");
            int lastX = branch.originPoint.X + ((branch.left) ? -branch.length : branch.length);

            if (branch.left)
            {
                for (int x = branch.originPoint.X; x > lastX; x--)
                {
                    branch = GenerateLavaInBranch(branch, x);
                }
            }
            else
            {
                for (int x = branch.currentPoint.X; x < lastX; x++)
                {
                    branch = GenerateLavaInBranch(branch, x);
                }
            }

            if (branch.currentStep < branch.maxStep)
            {
                bool horizontal = WorldGen.genRand.NextBool();
                VolcanoBranch newerbranch =
                    new VolcanoBranch(branch, WorldGen.genRand.Next(10, 15), horizontal);
                GenerateBranch(newerbranch);
                if (WorldGen.genRand.NextBool(5))
                {
                    newerbranch =
                        new VolcanoBranch(branch, WorldGen.genRand.Next(10, 15), !horizontal);
                    GenerateBranch(newerbranch);
                }
            }
        }

        private VolcanoBranch GenerateLavaInBranch(VolcanoBranch branch, int x)
        {
            for (int y = branch.currentPoint.Y - 2; y < branch.currentPoint.Y + 3; y++)
            {
                Main.tile[x, y].type = mod.TileID("SolarDirt");
                if (y == branch.currentPoint.Y && Main.tile[x, y].active())
                {

                    Main.tile[x, y].ClearTile();
                    Main.tile[x, y].lava(true);
                    Main.tile[x, y].liquid = 255;
                    if (WorldGen.genRand.NextFloat() >= 0.6f)
                    {
                        int yModifier = (branch.horizontal) ? 1 : -1;
                        branch.originPoint.Y += (branch.horizontal) ? 1 : -1;
                        branch.currentPoint.Y += yModifier;
                        branch.currentPoint.X = x;
                        branch.originPoint.X = x;
                        Main.tile[x, y + yModifier].ClearTile();
                        Main.tile[x, y + yModifier].lava(true);
                        Main.tile[x, y + yModifier].liquid = 255;
                    }

                }
            }

            return branch;
        }

        private void ShapeTheTop(Point highestPoint)
        {
            WorldUtils.Gen(highestPoint, new Shapes.Circle(WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(13, 17)),
                Actions.Chain(new GenAction[]
                    {
                        new Actions.ClearTile(true),
                        new Actions.Clear()
                    }));
        }

        private void GenerateTheDeepTunnel(Point highestPoint, int depth)
        {
            for (int i = highestPoint.Y; i < highestPoint.Y + depth; i++)
            {
                
                int tunnelModifer = WorldGen.genRand.Next(6, 10);
                for (int j = highestPoint.X - tunnelModifer; j < highestPoint.X + tunnelModifer; j++)
                {
                    if (j < highestPoint.X - 2 || j > highestPoint.X + 2 && !Main.tile[i, j].lava())
                    {
                        WorldGen.PlaceTile(j, i, mod.TileID("SolarDirt"));
                        if (Main.tile[j, i].type != mod.TileID("SolarDirt"))
                        {
                            Main.tile[j, i].type = mod.TileID("SolarDirt");
                        }
                    }
                    else
                    {
                        Main.tile[j, i].ClearTile();
                        Main.tile[j, i].lava(true);
                        Main.tile[j, i].liquid = 255;
                    }
                }
            }
        }
    }
}
