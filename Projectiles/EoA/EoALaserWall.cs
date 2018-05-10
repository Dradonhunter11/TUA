using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Projectiles.EoA
{
    class EoALaserWall : ModProjectile
    {

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
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.position, new Vector2(projectile.position.X + 16, projectile.position.Y + 16), 720, DelegateMethods.CastLight);
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate == 0) {
                return true;
            }
            return false;
        }


    }
}
