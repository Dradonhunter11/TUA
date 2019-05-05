using Terraria;

namespace TUA.Buff.Debuff.EoA
{
    public sealed class NoMountDebuff : TUABuff
    {
        public NoMountDebuff() : base("Blue Soul", "You can only jump and fly")
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

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