using System;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Projectiles.Misc.FishronEater
{
    class TheDuke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TheDuke");
        }

        public override void SetDefaults()
        {
            projectile.width = 49;
            projectile.height = 36;
            projectile.scale = 0.25f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 180;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            if (projectile.velocity.X > 0f)
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            }
        }

        public override void Kill(int timeLeft)
        {
            int Proj1 = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, mod.ProjectileType("FishronTornado"), projectile.damage, projectile.knockBack, projectile.owner);
            Main.projectile[Proj1].ai[0] = 6f;
            Main.projectile[Proj1].ai[1] = 6f;
            int rand = Main.rand.Next(4);
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
        }
    }
}
