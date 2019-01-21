using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonSandstone : TUADefaultBlockItem
    {
        public override string name => "Strange sandstone";
        public override int value => 0;
        public override int blockToPlace => mod.TileType("MeteoridonSandstone");
    }
}
