using Terraria.ModLoader;
using TUA.Tiles.Meteoridon;


namespace TUA.Projectiles.FallingProjectile
{
    class MeteoridonSandProjectile : TUAFallingProjectile
    {
        public override string name => "Meteoridon Sand";
        public override int Tile => ModContent.TileType<MeteoridonSand>();
    }
}
