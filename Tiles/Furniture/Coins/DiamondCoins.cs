using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Tiles.Furniture.Coins
{
    class DiamondCoins : TUACoins
    {
        public override int coinDropID => mod.ItemType("ApocalypseDirt");
        public override int coinProjectileID => mod.ProjectileType("DiamondCoinProjectile");
    }
}
