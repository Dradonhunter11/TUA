using Microsoft.Xna.Framework;
using TUA.API.Default;

namespace TUA.Tiles.Furniture.Coins
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
