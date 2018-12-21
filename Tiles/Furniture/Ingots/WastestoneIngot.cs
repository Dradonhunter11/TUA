using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TerrariaUltraApocalypse.Tiles.Furniture.Ingots
{
    class WastestoneIngot : TUAIngot
    {
        public override Color MapEntryColor => new Color(68, 74, 100);
        public override String MapNameLegend => "Wastestone ingot";
        public override int ingotDropName => mod.ItemType("WastestoneIngot");
    }
}
