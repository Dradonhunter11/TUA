using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonSandstone : TUADefaultBlockItem
    {
        public override string name => "Strange sandstone";
        public override int value => 0;
        public override int blockToPlace => mod.TileType("MeteoridonSandstone");
    }
}
