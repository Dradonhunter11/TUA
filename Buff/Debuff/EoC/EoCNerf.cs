using Terraria;
using Terraria.ModLoader;

namespace TUA.Buff.Debuff.EoC
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
