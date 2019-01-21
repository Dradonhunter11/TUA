﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TUA.API.Default;

namespace TUA.Items.Meteoridon.Tiles
{
    class MeteoridonSand : TUADefaultBlockItem
    {
        public override string name => "Meteoridon sand";
        public override int value => 0;
        public override int blockToPlace => mod.TileType("MeteoridonSand");
    }
}
