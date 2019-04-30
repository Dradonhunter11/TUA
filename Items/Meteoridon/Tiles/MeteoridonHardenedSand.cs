using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonHardenedSand : TUADefaultBlockItem
    {
        public override string TUAName => "Strange hardened sand";
        public override int TUAValue => 0;
        public override int BlockToPlace => mod.TileType("MeteoridonHardenedSand");
    }
}
