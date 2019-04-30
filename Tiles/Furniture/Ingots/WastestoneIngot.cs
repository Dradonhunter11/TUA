using System;
using Microsoft.Xna.Framework;

namespace TUA.Tiles.Furniture.Ingots
{
    class WastestoneIngot : TUAIngot
    {
        public override Color MapEntryColor => new Color(68, 74, 100);
        public override String MapNameLegend => "Wastestone ingot";
        public override int IngotDropName => mod.ItemType("WastestoneIngot");
    }
}
