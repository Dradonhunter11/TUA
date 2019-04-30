using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EotW
{
    class MagicDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("MAgic Curse");
            Description.SetDefault("EotW Blue: Only magic can do damage");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= -0f;
            player.magicDamage *= 5f;
            player.bulletDamage *= -0f;
            player.arrowDamage *= -0f;
            player.rocketDamage *= -0f;
            player.thrownDamage *= -0f;
            player.minionDamage *= -0f;

        }
    }
}
