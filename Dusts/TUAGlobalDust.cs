using Terraria;
using Terraria.ModLoader;

namespace TUA.Dusts
{
    class TUAGlobalDust : ModDust
    {
        
        protected int timer;

        public override bool Autoload(ref string name, ref string texture)
        {
            if (name == "TUAGlobalDust")
            {
                return false;
            }
            return true;
        }

        public override bool Update(Dust dust)
        {
            timer--;
            if (timer == 0)
            {
                dust.active = false;
            }
            return true;
        }
    }
}