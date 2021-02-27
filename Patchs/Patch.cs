using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using Terraria.IO;

namespace TUA.Patchs
{
    internal static partial class Patch
    {
        public static void Load()
        {
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItemOnctor;
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItemOnDrawSelf;
        }

        
    }
}
