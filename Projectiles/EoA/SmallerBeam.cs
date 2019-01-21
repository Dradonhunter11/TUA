using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TUA.API.VoidClass;
using TUA.NPCs.Gods.EoA;

namespace TUA.Projectiles.EoA
{
    class SmallerBeam : ModProjectile
    {

        public float LaserLength = 0f;
        public float rotation = 0f;

        private ModNPC master;

        public void setMaster(ModNPC master)
        {
            this.master = master;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            cooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(LaserLength);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            LaserLength = reader.ReadSingle();
        }

        public override void AI()
        {
            if (master is Eye_of_Apocalypse)
            {
                Eye_of_Apocalypse trueMaster = master as Eye_of_Apocalypse;
                LaserLength = trueMaster.getMagnitude() * 2;
                projectile.velocity = trueMaster.npc.velocity;
                rotation = (projectile.Center - trueMaster.getCenterPosition()).ToRotation();
            } else if (master is Eye_of_Apocalypse_clone)
            {
                Eye_of_Apocalypse_clone trueMaster = master as Eye_of_Apocalypse_clone;
                LaserLength = trueMaster.getMagnitude() * 2;
                projectile.velocity = trueMaster.npc.velocity;
                rotation = (projectile.Center - trueMaster.getCenterPoint()).ToRotation();
            }
        }

        

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num7 = 0f;
            Vector2 end = projectile.Center - LaserLength * rotation.ToRotationVector2();
            Dust.QuickDustLine(projectile.Center, end, 5f, Color.Red);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, end, projectile.width, ref num7);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.statLife--;
            target.noKnockback = true;
            if (target.statLife <= 0)
            {
                target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " couldn't handle the destruction beam"), 1, 0);
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Main.DiscoColor;
            Vector2 direction = -rotation.ToRotationVector2();
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            for (float k = projectile.width * 1.5f; k < LaserLength; k += projectile.width)
            {
                Vector2 drawPos = projectile.Center + k * direction - Main.screenPosition;
                spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
