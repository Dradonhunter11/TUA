using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EoC
{
    public sealed class EoCNerf : TUABuff
    {
        public EoCNerf() : base("Ultra Nerf : EoC", "Nothing can cancel it")
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}
