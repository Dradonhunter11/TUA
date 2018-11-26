using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Items
{
    class TerraMeter : EnergyItem
    {
        public override void SafeSetDefault(ref int maxEnergy)
        {
            maxEnergy = 100000;
            item.width = 30;
            item.height = 30;
            item.consumable = false;
            item.useStyle = 4;
        }
    }
}