using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;

namespace TUA.Buff.Debuff.EoA
{
    class NoMountDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blue soul");
            Description.SetDefault("You can only jump and fly...");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.mount._active)
            {
                player.mount._active = false;
            }

            player.controlHook = false;
            player.releaseHook = false;
            player.noKnockback = true;
            player.teleporting = false;

            if (player.releaseJump)
            {
                player.wingTime = 0;
            }
        }
    }
}