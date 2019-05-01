using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EotW
{
    public sealed class MeleeDebuff : TUABuff
    {
        public MeleeDebuff() : base("Melee Curse", "EotW Red: Can only inflict melee damage")
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
            player.meleeDamage *= 5f;
            player.magicDamage *= 0f;
            player.bulletDamage *= 0f;
            player.arrowDamage *= 0f;
            player.rocketDamage *= 0f;
            player.thrownDamage *= 0f;
            player.minionDamage *= 0f;
        }
    }
}
