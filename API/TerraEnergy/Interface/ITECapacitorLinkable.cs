using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API.TerraEnergy.TileEntities;

namespace TUA.API.TerraEnergy.Interface
{
    interface ITECapacitorLinkable
    {
        void LinkToCapacitor(CapacitorEntity capacitor);
    }
}
