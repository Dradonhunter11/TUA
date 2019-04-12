using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.Default
{
    public abstract class TUASolution : ModProjectile
    {

        public virtual int size => 4;
        public abstract String name { get; }
        public abstract int dust { get; }

        public abstract void Convert(int i, int j);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
        }

        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int dustType = mod.DustType("ExampleSolution");
            if (projectile.owner == Main.myPlayer)
            {
                Convert((int)(projectile.position.X + (float)(projectile.width / 2)) / 16, (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16);
            }
            if (projectile.timeLeft > 133)
            {
                projectile.timeLeft = 133;
            }
            if (projectile.ai[0] > 7f)
            {
                float dustScale = 1f;
                if (projectile.ai[0] == 8f)
                {
                    dustScale = 0.2f;
                }
                else if (projectile.ai[0] == 9f)
                {
                    dustScale = 0.4f;
                }
                else if (projectile.ai[0] == 10f)
                {
                    dustScale = 0.6f;
                }
                else if (projectile.ai[0] == 11f)
                {
                    dustScale = 0.8f;
                }
                projectile.ai[0] += 1f;
                for (int i = 0; i < 1; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                    dust.velocity.X = dust.velocity.X * 2f;
                    dust.velocity.Y = dust.velocity.Y * 2f;
                    dust.scale *= dustScale;
                }
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            projectile.rotation += 0.3f * (float)projectile.direction;
        }  
    }
}
