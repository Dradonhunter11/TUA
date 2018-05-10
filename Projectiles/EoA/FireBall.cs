using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Projectiles.EoA
{
    class FireBall : ModProjectile
    {

        private int changeVelocityTimer = 25;
        private int dustTimer = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            DisplayName.AddTranslation(GameCulture.French, "");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.damage = 30;
            projectile.tileCollide = false;
            projectile.timeLeft = 175;
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.penetrate = 2;
            projectile.hostile = true;
        }

        public override void AI()
        {
            if (changeVelocityTimer == 0 && projectile.ai[0] == 0) {
                changeVelocityTimer = 25;
                projectile.velocity.X = -projectile.velocity.X;
            } else if (changeVelocityTimer == 0 && projectile.ai[0] == 1) {
                changeVelocityTimer = 25;
                projectile.velocity.Y = -projectile.velocity.Y;
            }
            changeVelocityTimer--;

            if (dustTimer == 0) {
                int dust = Dust.NewDust(projectile.Center, 4, 4, DustID.Fire, 0, 2f);
                Main.dust[dust].color = Color.Green;
            }
        }
    }
}