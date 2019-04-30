using Microsoft.Xna.Framework;
using TUA.API.Default;

namespace TUA.Tiles.Furniture.Coins
{
    class DiamondCoins : TUAFallingBlock
    {
        public override int ItemDropID => mod.ItemType("ApocalypseDirt");
        public override int ItemProjectileID => mod.ProjectileType("DiamondCoinProjectile");
        public override bool SandTile => false;
        public override Color MapColor => Color.White;
        public override string MapLegend => "Diamond Coin";
    }
}
