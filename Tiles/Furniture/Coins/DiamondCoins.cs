using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.Default;

namespace TerrariaUltraApocalypse.Tiles.Furniture.Coins
{
    class DiamondCoins : TUAFallingBlock
    {
        public override int ItemDropID => mod.ItemType("ApocalypseDirt");
        public override int ItemProjectileID => mod.ProjectileType("DiamondCoinProjectile");
        public override bool sandTile => false;
        public override Color mapColor => Color.White;
        public override string mapLegend => "Diamond Coin";
    }
}
