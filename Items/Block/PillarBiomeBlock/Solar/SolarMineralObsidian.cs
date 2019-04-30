using TUA.API;
using TUA.API.Default;

namespace TUA.Items.Block.PillarBiomeBlock.Solar
{
    class SolarMineralObsidian : TUADefaultBlockItem
    {
        public override string name => "Solar mineral obsidian";
        public override int value => 10;
        public override int blockToPlace => mod.TileID("SolarMineralObsidian");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("A rich mineral rock found in the volcano of the solar dimension");
        }
    }
}
