using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace TerrariaUltraApocalypse.Dusts.EotW
{
    class BulletDust : ModDust
    {
        private int timer = 40;


        public override void OnSpawn(Terraria.Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= 1f;
            dust.velocity.X = Main.rand.Next(10, 20) * 0.8f;
        }

        public override bool MidUpdate(Terraria.Dust dust)
        {
            dust.position += dust.velocity;
            timer--;
            if (timer == 0)
            {
                dust.active = false;
                return false;
            }
            return true;
        }
    }
}
