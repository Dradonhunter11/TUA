using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Spells.BiomeSpell
{
    abstract class BaseBiomeSpellProjectile : ModProjectile
    {
        public override string Texture => "Spells/BiomeSpell";

        public abstract void Convert(int x, int y);

        public override void SetDefaults()
        {
            projectile.width = 33;
            projectile.height = 33;
            projectile.scale = 3f;
            projectile.damage = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.MaxUpdates = 1;
            projectile.timeLeft = 50;
            projectile.tileCollide = false;

        }

        public sealed override void AI()
        {
            int topPosition = (int) (projectile.position.Y / 16) - 1;
            int leftPosition = (int) (projectile.position.X / 16) - 1;
            int rightPosition = (int) (projectile.position.X + (float)projectile.width / 16) + 2;
            int bottomPosition = (int) (projectile.position.Y + (float) projectile.height / 16) + 2;

            if (leftPosition < 0)
            {
                leftPosition = 0;
            }

            if (topPosition < 0)
            {
                topPosition = 0;
            }

            if (rightPosition > Main.maxTilesX)
            {
                rightPosition = Main.maxTilesX;
            }

            if (bottomPosition > Main.maxTilesY)
            {
                bottomPosition = Main.maxTilesY;
            }

            for (int x = leftPosition; x < rightPosition; x++) {
                for (int y = topPosition; y < bottomPosition; y++)
                {
                    Convert(x, y);
                }
            }
        }
    }
}
