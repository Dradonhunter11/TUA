using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Projectiles.FallingProjectile
{
    abstract class TUAFallingProjectile : ModProjectile
    {
        public abstract String name { get; }
        public abstract int Tile { get; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.damage = 0;
            projectile.ranged = true;
            projectile.penetrate = 5;
            projectile.tileCollide = true;
            projectile.aiStyle = 10;
            aiType = ProjectileID.GoldCoinsFalling;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            WorldGen.PlaceTile((int)(projectile.position.X / 16), (int)(projectile.position.Y / 16),
                Tile);
        }
    }
}
