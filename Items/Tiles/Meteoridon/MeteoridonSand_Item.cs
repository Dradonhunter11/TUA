using Terraria.ModLoader;
using TUA.API.Default;
using TUA.Tiles.Meteoridon;

namespace TUA.Items.Tiles.Meteoridon
{
    class MeteoridonSand_Item : TUADefaultBlockItem
    {
        public override string TUAName => "Meteoridon sand";
        public override int TUAValue => 0;
        public override int BlockToPlace => ModContent.TileType<MeteoridonSand>();
    }
}
