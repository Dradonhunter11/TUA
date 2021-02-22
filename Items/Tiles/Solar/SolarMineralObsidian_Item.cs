using TUA.API;
using TUA.API.Default;

namespace TUA.Items.Tiles.Solar
{
    class SolarMineralObsidian_Item : TUADefaultBlockItem
    {
        public override string TUAName => "Solar mineral obsidian";
        public override int TUAValue => 10;
        public override int BlockToPlace => mod.TileID("SolarMineralObsidian");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("A rich mineral rock found in the volcano of the solar dimension");
        }
    }
}
