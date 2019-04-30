using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TUA.BaseMod.Base;

namespace TUA.BaseMod
{
	public class MProjectile : GlobalProjectile
	{
		public override bool PreDrawExtras(Projectile projectile, SpriteBatch spriteBatch)
		{
			BaseArmorData.lastShaderDrawObject = projectile;
			return base.PreDrawExtras(projectile, spriteBatch);
		}		
	}
}