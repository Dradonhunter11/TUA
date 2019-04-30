using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonSand : TUADefaultBlockItem
    {
        public override string TUAName => "Meteoridon sand";
        public override int TUAValue => 0;
        public override int BlockToPlace => mod.TileType("MeteoridonSand");
    }
}
