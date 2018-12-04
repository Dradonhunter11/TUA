using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUltraApocalypse.Tiles.NewBiome.Meteoridon;


namespace TerrariaUltraApocalypse.Projectiles.FallingProjectile
{
    class MeteoridonSandProjectile : TUAFallingProjectile
    {
        public override string name => "Meteoridon Sand";
        public override int Tile => mod.TileType<MeteoridonSand>();
    }
}
