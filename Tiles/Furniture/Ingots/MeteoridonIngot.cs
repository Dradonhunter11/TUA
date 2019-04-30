using Microsoft.Xna.Framework;

namespace TUA.Tiles.Furniture.Ingots
{
    class MeteoridonIngot : TUAIngot
    {
        public override Color MapEntryColor => Color.BlanchedAlmond;
        public override string MapNameLegend => "Meteoride Bar";
        public override int IngotDropName => mod.ItemType("MeteoridonBar");
    }
}
