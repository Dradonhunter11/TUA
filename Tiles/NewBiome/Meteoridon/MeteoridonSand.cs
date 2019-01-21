using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TUA.API.Default;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonSand : TUAFallingBlock
    {
        public override int ItemDropID => mod.ItemType("MeteoridonSand");
        public override int ItemProjectileID => mod.ProjectileType("MeteoridonSandProjectile");
        public override bool sandTile => true;
        public override Color mapColor => Color.DarkOrange;
    }
}
