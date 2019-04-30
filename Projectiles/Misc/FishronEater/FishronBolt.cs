using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Projectiles.Misc.FishronEater
{
    public class FishronBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharknado");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 21;
            projectile.height = 27;
            projectile.scale = 0.5f;
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
            float num486 = projectile.position.X;
            float num487 = projectile.position.Y;
            float num488 = 100000f;
            bool flag17 = false;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 30f)
            {
                projectile.ai[0] = 30f;
                for (int num489 = 0; num489 < 200; num489++)
                {
                    if (Main.npc[num489].active && !Main.npc[num489].dontTakeDamage && !Main.npc[num489].friendly && Main.npc[num489].lifeMax > 5 && (!Main.npc[num489].wet || projectile.type == 307))
                    {
                        float num490 = Main.npc[num489].position.X + (float)(Main.npc[num489].width / 2);
                        float num491 = Main.npc[num489].position.Y + (float)(Main.npc[num489].height / 2);
                        float num492 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num490) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num491);
                        if (num492 < 800f && num492 < num488 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num489].position, Main.npc[num489].width, Main.npc[num489].height))
                        {
                            num488 = num492;
                            num486 = num490;
                            num487 = num491;
                            flag17 = true;
                        }
                    }
                }
            }
            if (!flag17)
            {
                num486 = projectile.position.X + (float)(projectile.width / 2) + projectile.velocity.X * 100f;
                num487 = projectile.position.Y + (float)(projectile.height / 2) + projectile.velocity.Y * 100f;
            }
            float num493 = 6f;
            float num494 = 0.1f;
            Vector2 vector36 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num495 = num486 - vector36.X;
            float num496 = num487 - vector36.Y;
            float num497 = (float)Math.Sqrt((double)(num495 * num495 + num496 * num496));
            num497 = num493 / num497;
            num495 *= num497;
            num496 *= num497;
            if (projectile.velocity.X < num495)
            {
                projectile.velocity.X = projectile.velocity.X + num494;
                if (projectile.velocity.X < 0f && num495 > 0f)
                {
                    projectile.velocity.X = projectile.velocity.X + num494 * 2f;
                }
            }
            else
            {
                if (projectile.velocity.X > num495)
                {
                    projectile.velocity.X = projectile.velocity.X - num494;
                    if (projectile.velocity.X > 0f && num495 < 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X - num494 * 2f;
                    }
                }
            }
            if (projectile.velocity.Y < num496)
            {
                projectile.velocity.Y = projectile.velocity.Y + num494;
                if (projectile.velocity.Y < 0f && num496 > 0f)
                {
                    projectile.velocity.Y = projectile.velocity.Y + num494 * 2f;
                }
            }
            else
            {
                if (projectile.velocity.Y > num496)
                {
                    projectile.velocity.Y = projectile.velocity.Y - num494;
                    if (projectile.velocity.Y > 0f && num496 < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - num494 * 2f;
                    }
                }
            }
            projectile.light = 0.9f;
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
            if (projectile.frameCounter < 5)
                projectile.frame = 0;
            else if (projectile.frameCounter >= 5 && projectile.frameCounter < 10)
                projectile.frame = 1;
            else
                projectile.frameCounter = 0;
            projectile.frameCounter++;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.localAI[0] < 30 && target.life > 1)
            {
                projectile.localAI[0] += 1;
                projectile.position.X = target.position.X;
                projectile.position.Y = target.position.Y;
                projectile.velocity.X = target.velocity.X;
                projectile.velocity.Y = target.velocity.Y;
            }
        }

        public override void Kill(int timeleft)
        {
            int rand = Main.rand.Next(3);
            if (rand == 0)
                Gore.NewGore(projectile.position, new Vector2(MathHelper.Lerp(-4f, 4f, (float)Main.rand.NextDouble()), -2), 577, 0.5f);
            else if (rand == 1)
                Gore.NewGore(projectile.position, new Vector2(MathHelper.Lerp(-4f, 4f, (float)Main.rand.NextDouble()), -2), 578, 0.5f);
            else
                Gore.NewGore(projectile.position, new Vector2(MathHelper.Lerp(-4f, 4f, (float)Main.rand.NextDouble()), -2), 579, 0.5f);
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
        }
    }
}
