using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using TUA.API.GenActionModifiers;
using TUA.Dimension.Block;
using TUA.Tiles.NewBiome.Meteoridon;

namespace TUA.Dimension.MicroBiome
{
    class StardustFrozenForest : Terraria.World.Generation.MicroBiome
    {
        private readonly int WIDTH = 300;
        private readonly int HEIGHT = 100;

        public static int roomNumber = 2;

        public override bool Place(Point origin, StructureMap structures)
        {
            Rectangle structureBound = new Rectangle(origin.X - WIDTH / 2, origin.Y - HEIGHT / 2, WIDTH, HEIGHT);

            if (!structures.CanPlace(structureBound))
            {
                return false;
            }


            Dictionary<ushort, int> environmentTile = new Dictionary<ushort, int>();
            WorldUtils.Gen(structureBound.Location, new Shapes.Rectangle(WIDTH, HEIGHT), new Actions.TileScanner(
                new ushort[]
                {
                    (ushort)ModLoader.GetMod("TUA").TileType("StardustRock"),
                    (ushort)ModLoader.GetMod("TUA").TileType("StardustIce")
                }
            ).Output(environmentTile));

            int total = 0;
            foreach (KeyValuePair<ushort, int> pair in environmentTile)
            {
                total += pair.Value;
            }

            if (total < 4000)
            {
                return false;
            }

            generateForest(origin);
            return true;
        }

        internal void generateForest(Point start)
        {

            int circleHorizontal = HEIGHT - WorldGen.genRand.Next(20);

            WorldUtils.Gen(start, new Shapes.Circle(circleHorizontal, 50), Actions.Chain(new GenAction[]
            {
                new Modifiers.Conditions(new IntegretyFilter(50)),
                new Actions.ClearTile(true)
            }));

            Point left = start;
            left.X += 50;
            Point right = start;
            right.X -= 50;


            for (int currentX = 0; currentX < 200; currentX++)
            {
                int randomYmodifier = WorldGen.genRand.Next(6);
                if (randomYmodifier == 1)
                {
                    left.Y += 1;
                }
                else if (randomYmodifier == 2)
                {
                    left.Y -= 1;
                }
                else if (randomYmodifier == 3)
                {
                    right.Y += 1;
                }
                else if (randomYmodifier == 4)
                {
                    right.Y -= 1;
                }

                left.X++;
                right.X--;

                WorldUtils.Gen(left, new Shapes.Circle(16, 16), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Conditions(new IntegretyFilter(50)),
                    new Actions.ClearTile(true)
                }));

                WorldUtils.Gen(right, new Shapes.Circle(16, 16), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Conditions(new IntegretyFilter(50)),
                    new Actions.ClearTile(true)
                }));
            }

            left = start;
            left.X += 100;
            right = start;
            right.X -= 100;
            left.Y -= 24;
            right.Y += 24;

            for (int currentX = 0; currentX < 10; currentX++)
            {

                left.X += 30;
                right.X -= 30;


                WorldUtils.Gen(left, new Shapes.Tail(16, new Vector2(WorldGen.genRand.Next(-2, 2), 24)), Actions.Chain(new GenAction[]
                {
                    new Actions.PlaceTile(TileID.CobaltBrick)
                }));

                WorldUtils.Gen(right, new Shapes.Tail(16, new Vector2(WorldGen.genRand.Next(-2, 2), -24)), Actions.Chain(new GenAction[]
                {
                    new Actions.PlaceTile(TileID.CobaltBrick)
                }));
            }

            generateRuin(start, 50, 35, 50, true);
        }

        internal void generateRuin(Point origin, int width, int height, int integretyPercent, bool chest = false)
        {
            ShapeData data = new ShapeData();

            origin.X -= width / 2;
            origin.Y -= height / 2;

            WorldUtils.Gen(origin, new Shapes.Rectangle(width + 2, height + 2), Actions.Chain(new GenAction[]
            {
                new Actions.ClearTile(true),
                new Actions.PlaceTile(TileID.CobaltBrick)
            }));

            origin.X++;
            origin.Y++;

            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(new GenAction[]
            {
                new Actions.ClearTile(true),
                new Modifiers.Conditions(new IntegretyFilter(integretyPercent)),
                new Actions.PlaceWall(WallID.IceBrick)
            }));

            int x = origin.X - 1 + WorldGen.genRand.Next(width - 2);
            int y = origin.Y + height - 1;

            WorldGen.PlaceTile(x, y, TerrariaUltraApocalypse.instance.TileType<MobSpawner>());
            TerrariaUltraApocalypse.instance.GetTileEntity<MobSpawnerEntity>().Place(x - 1, y - 1);
        }
    }
}
