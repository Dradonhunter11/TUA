using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.API.FurnaceRework.TileEntity
{
    class HellForgeEntity : BaseFurnaceEntity
    {
        public override void SetValue(ref int maxEnergy, ref int cookTimer, ref string furnaceName)
        {
            cookTimer = 60;
            maxEnergy = 150;
            furnaceName = "Hell Forge";
        }
    }
}
