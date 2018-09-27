using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.World.Generation;

namespace TerrariaUltraApocalypse.Structure.hellalt
{
    class TheHeartArena : MicroBiome
    {
        private readonly int WIDTH = 100;
        private readonly int HEIGHT = 100;

        public override bool Place(Point origin, StructureMap structures)
        {
            Rectangle structureBound = new Rectangle(origin.X - WIDTH / 2, origin.Y - HEIGHT / 2, WIDTH, HEIGHT);
            structures.AddStructure(structureBound);
            generateArena(origin);
            return true;
        }

        public void generateArena(Point origin)
        {
            WorldUtils.Gen(origin, new Shapes.Circle(77, 77), Actions.Chain(new GenAction[]
            {
                new Actions.ClearTile(true),
                new Actions.PlaceTile(TileID.Obsidian) 
            }));

            WorldUtils.Gen(origin, new Shapes.Circle(75, 75), Actions.Chain(new GenAction[]
            {
                new Actions.ClearTile(true),
                new Actions.PlaceWall((byte) TerrariaUltraApocalypse.instance.WallType("WastestoneBrickWall"), true)
            }));

            WorldUtils.Gen(origin, new Shapes.Rectangle(144, 10), Actions.Chain(new GenAction[]
            {
                new Actions.ClearTile(true),
            }));
        }
    }
}
