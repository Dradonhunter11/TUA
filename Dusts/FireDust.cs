using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace TerrariaUltraApocalypse.Dusts
{
    class FireDust : TUAGlobalDust
    {
        public override void SetDefaults()
        {
            Main.dust[Type].noGravity = true;
            Main.dust[Type].noLight = false;
            timer = 10;
        }
    }
}