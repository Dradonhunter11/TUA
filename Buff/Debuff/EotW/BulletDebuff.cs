using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EotW
{
    class BulletDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bullet Curse");
            Description.SetDefault("EotW Grey: Only bullet can do damage");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= -0f;
            player.magicDamage *= -0f;
            player.bulletDamage *= 5f;
            player.arrowDamage *= -0f;
            player.rocketDamage *= -0f;
            player.thrownDamage *= -0f;
            player.minionDamage *= -0f;

        }
    }
}