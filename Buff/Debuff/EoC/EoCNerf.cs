using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Buff.Debuff.EoC
{
    class EoCNerf : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ultra Nerf : EoC");
            Description.SetDefault("Nothing can cancel it... unless you cancel the AI...");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}
