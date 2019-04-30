using TUA.Tiles.NewBiome.Meteoridon;


namespace TUA.Projectiles.FallingProjectile
{
    class MeteoridonSandProjectile : TUAFallingProjectile
    {
        public override string name => "Meteoridon Sand";
        public override int Tile => mod.TileType<MeteoridonSand>();
    }
}
