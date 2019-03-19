using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;

namespace TUA.Structure.DungeonLike
{
    class SolarDungeon : MicroBiome
    {

        public const ushort TESTBLOCK = TileID.CopperBrick; 

        public struct DungeonSkyAccessRoom
        {
            public Point16 StartingPosition;
            public Point16 EndPosition;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            return true;
        }

        public override void Reset()
        {
            base.Reset();
        }

    }
}
