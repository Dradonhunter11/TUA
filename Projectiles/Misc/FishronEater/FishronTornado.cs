using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Projectiles.Misc.FishronEater
{
    public class FishronTornado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishron Tonado");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 34;
            projectile.aiStyle = -1;
            projectile.timeLeft = 180;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            int num746 = 10;
            int num747 = 15;
            float num748 = 1f;
            int num749 = 150;
            int num750 = 42;
            if (projectile.velocity.X != 0f)
            {
                projectile.direction = (projectile.spriteDirection = -Math.Sign(projectile.velocity.X));
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 2)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 6)
            {
                projectile.frame = 0;
            }
            if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
            {
                projectile.localAI[0] = 1f;
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.scale = ((float)(num746 + num747) - projectile.ai[1]) * num748 / (float)(num747 + num746);
                projectile.width = (int)((float)num749 * projectile.scale);
                projectile.height = (int)((float)num750 * projectile.scale);
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != -1f)
            {
                projectile.scale = ((float)(num746 + num747) - projectile.ai[1]) * num748 / (float)(num747 + num746);
                projectile.width = (int)((float)num749 * projectile.scale);
                projectile.height = (int)((float)num750 * projectile.scale);
            }
            if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha -= 30;
                if (projectile.alpha < 60)
                {
                    projectile.alpha = 60;
                }
                if (projectile.type == 386 && projectile.alpha < 100)
                {
                    projectile.alpha = 100;
                }
            }
            else
            {
                projectile.alpha += 30;
                if (projectile.alpha > 150)
                {
                    projectile.alpha = 150;
                }
            }
            if (projectile.ai[0] > 0f)
            {
                projectile.ai[0] -= 1f;
            }
            if (projectile.ai[0] == 1f && projectile.ai[1] > 0f && projectile.owner == Main.myPlayer)
            {
                projectile.netUpdate = true;
                Vector2 vector56 = projectile.Center;
                vector56.Y -= (float)num750 * projectile.scale / 2f;
                float num751 = ((float)(num746 + num747) - projectile.ai[1] + 1f) * num748 / (float)(num747 + num746);
                vector56.Y -= (float)num750 * num751 / 2f;
                vector56.Y += 2f;
                Projectile.NewProjectile(vector56.X, vector56.Y, projectile.velocity.X, projectile.velocity.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 10f, projectile.ai[1] - 1f);

            }
            if (projectile.timeLeft % 30 == 0)
            {
                Vector2 vector56 = projectile.Center;
                vector56.Y -= (float)num750 * projectile.scale / 2f;
                float num751 = ((float)(num746 + num747) - projectile.ai[1] + 1f) * num748 / (float)(num747 + num746);
                vector56.Y -= (float)num750 * num751 / 2f;
                vector56.Y += 2f;
                int num754 = Projectile.NewProjectile(vector56.X, vector56.Y, projectile.velocity.X + Main.rand.Next(-5, 5), projectile.velocity.Y, mod.ProjectileType("FishronBolt"), projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num754].netUpdate = true;
            }
            if (projectile.ai[0] <= 0f)
            {
                float num755 = 0.104719758f;
                float num756 = (float)projectile.width / 5f;
                if (projectile.type == 386)
                {
                    num756 *= 2f;
                }
                float num757 = (float)(Math.Cos((double)num755 * -(double)projectile.ai[0]) - 0.5) * num756;
                projectile.position.X = projectile.position.X - num757 * -(float)projectile.direction;
                projectile.ai[0] -= 1f;
                num757 = (float)(Math.Cos((double)num755 * -(double)projectile.ai[0]) - 0.5) * num756;
                projectile.position.X = projectile.position.X + num757 * -(float)projectile.direction;
            }
        }
    }
}
