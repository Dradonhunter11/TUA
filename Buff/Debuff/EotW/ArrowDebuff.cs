﻿using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EotW
{
    public sealed class ArrowDebuff : TUABuff
    {
        public ArrowDebuff() : base("Arrow Curse", "EotW purple: Can only inflict damage with arrows")
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= 0f;
            player.magicDamage *= 0f;
            player.bulletDamage *= 0f;
            player.arrowDamage *= 5f;
            player.rocketDamage *= 0f;
            player.thrownDamage *= 0f;
            player.minionDamage *= 0f;
        }
    }
}