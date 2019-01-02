using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUltraApocalypse.API.Default;

namespace TerrariaUltraApocalypse.Items.Meteoridon.Tiles
{
    class MeteoridonSandstone : TUADefaultBlockItem
    {
        public override string name => "Strange sandstone";
        public override int value => 0;
        public override int blockToPlace => mod.TileType("MeteoridonSandstone");
    }
}
