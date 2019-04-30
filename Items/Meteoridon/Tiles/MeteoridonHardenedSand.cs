using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonHardenedSand : TUADefaultBlockItem
    {
        public override string name => "Strange hardened sand";
        public override int value => 0;
        public override int blockToPlace => mod.TileType("MeteoridonHardenedSand");
    }
}
