using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using TerrariaUltraApocalypse.API.VoidClass;


namespace TerrariaUltraApocalypse.Projectiles
{
    class GodExplosiveArrow : VoidDamageProjectile
    {

        private int timer = 0;
        private Player p;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God Explosive Arrow");
            DisplayName.AddTranslation(GameCulture.French, "Flèche divine explosive");
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 14;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.timeLeft = 76;
            Void = true;
            VoidDamage = 50;
            aiType = ProjectileID.HolyArrow;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return true;
        }

        public override void AI()
        {
            projectile.rotation /= projectile.velocity.X / projectile.velocity.Y;
            timer++;
            if (timer == 75)
            {
                projectile.damage = 50;
                explode();
            }
            
            if (projectile.owner == Main.myPlayer) {
                //Main.NewText("Success", Color.AliceBlue);
            }

            CreateDust();
        }

        private void explode()
        {
            Projectile.NewProjectile(projectile.Center, new Vector2(0f, 10f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(0f, -10f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(10f, 0f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(-10f, 0f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(5f, 5f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(-5f, -5f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(5f, 2f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(-5f, 2f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(2f, 5f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
            Projectile.NewProjectile(projectile.Center, new Vector2(-2f, 5f), ProjectileID.HellfireArrow, 150, 5f, projectile.owner);
        }

        public virtual void CreateDust() {
            Color? color = Color.DarkRed;
            if (color.HasValue) {
                int dust = Dust.NewDust(projectile.position, 16, 16, DustID.FlameBurst);
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].velocity += projectile.velocity;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            explode();
            target.StrikeNPCNoInteraction(150 - Main.rand.Next(-50, 50), 5, -target.direction);
        }
    }
}
