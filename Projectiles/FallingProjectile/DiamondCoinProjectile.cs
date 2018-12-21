using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.Projectiles.FallingProjectile
{
    class DiamondCoinProjectile : TUAFallingProjectile
    {
        public override string name => "Diamond Coin";
        public override int Tile => mod.TileType("DiamondCoins");
    }
}
