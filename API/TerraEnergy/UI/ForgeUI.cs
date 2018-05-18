using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria.UI;

namespace TerrariaUltraApocalypse.API.TerraEnergy.UI
{
    class ForgeUI : UIState
    {

        public static bool visible = false;

    }

    class ForgeItemSlot : UIItemSlot
    {
        public override void sendItemToTileEntity()
        {
        }

        public override void sync()
        {
        }
    }
}