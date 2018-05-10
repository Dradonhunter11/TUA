using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Items
{
    class TerraMeter : TUAModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.consumable = false;
            item.useStyle = 4;
        }
    }
}