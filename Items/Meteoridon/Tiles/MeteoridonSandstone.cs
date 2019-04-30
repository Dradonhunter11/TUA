using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonSandstone : TUADefaultBlockItem
    {
        public override string TUAName => "Strange sandstone";
        public override int TUAValue => 0;
        public override int BlockToPlace => mod.TileType("MeteoridonSandstone");
    }
}
