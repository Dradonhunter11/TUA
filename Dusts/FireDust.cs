using Terraria;

namespace TUA.Dusts
{
    class FireDust : TUAGlobalDust
    {
        public override void SetDefaults()
        {
            Main.dust[Type].noGravity = true;
            Main.dust[Type].noLight = false;
            timer = 10;
        }
    }
}