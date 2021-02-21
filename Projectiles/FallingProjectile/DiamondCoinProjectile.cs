using Terraria.ModLoader;
using TUA.Tiles.Furniture.Coins;

namespace TUA.Projectiles.FallingProjectile
{
    class DiamondCoinProjectile : TUAFallingProjectile
    {
        public override string name => "Diamond Coin";
        public override int Tile => ModContent.TileType<DiamondCoins>();
    }
}
