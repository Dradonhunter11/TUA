using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Projectiles
{
    class GhosterBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghoster Bullet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;  
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.width = 20;
            projectile.height = 10;
            projectile.damage = 150;
            projectile.aiStyle = 1;
            projectile.timeLeft = 200;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i]; //Go trough the entity list

                if (!target.friendly)
                {
                    float shootToX = target.position.X + (float) target.width * 0.5f - projectile.Center.X; //Basically X speed. There math here are X - It's width / 0.5 pixel - the projectile center
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)Math.Sqrt(shootToX * shootToX + shootToY * shootToY);
                    if (distance <= 480f && !target.friendly && target.active)
                    {
                        distance /= 3;

                        shootToY *= distance * 2;
                        shootToX *= distance * 2;

                        projectile.velocity.X = shootToX;
                        projectile.velocity.Y = shootToY;
                        createDust(projectile);
                    }
                }
            }
        }

        private void createDust(Projectile p)
        {
            int trail = Dust.NewDust(p.velocity, 10, 10, DustID.Shadowflame, 0f, 0f, 0,Microsoft.Xna.Framework.Color.Red);
            Main.dust[trail].position = p.position;
            Main.dust[trail].fadeIn = 2f;
        }
    }
}
