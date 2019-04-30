namespace TUA.Projectiles.FallingProjectile
{
    class DiamondCoinProjectile : TUAFallingProjectile
    {
        public override string name => "Diamond Coin";
        public override int Tile => mod.TileType("DiamondCoins");
    }
}
