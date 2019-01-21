using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API;

namespace TUA.Items.Misc.Debug
{
    class DebugStick : TUAModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Stick");
            Tooltip.AddLine("Totally not a minecraft reference");
            Tooltip.AddLine("Only those that worthy can use it");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.rare = -13;
        }
    }
}
