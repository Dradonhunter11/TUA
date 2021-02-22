using Terraria.ModLoader;
using TUA.API.Default;
using TUA.Tiles.Meteoridon;

namespace TUA.Items.Tiles.Meteoridon
{
    class MeteoridonHardenedSand_Item : TUADefaultBlockItem
    {
        public override string TUAName => "Strange hardened sand";
        public override int TUAValue => 0;
        public override int BlockToPlace => ModContent.TileType<MeteoridonHardenedSand>();
    }
}
