using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Buff.Debuff.EotW
{
    class MeleeDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Melee Curse");
            Description.SetDefault("EotW Red: Only melee can do damage");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= 5f;
            player.magicDamage *= -0f;
            player.bulletDamage *= -0f;
            player.arrowDamage *= -0f;
            player.rocketDamage *= -0f;
            player.thrownDamage *= -0f;
            player.minionDamage *= -0f;

        }
    }
}
