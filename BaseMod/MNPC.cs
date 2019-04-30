using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TUA.BaseMod.Base;

namespace TUA.BaseMod
{
	public class MNPC : GlobalNPC
	{
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			BaseArmorData.lastShaderDrawObject = npc;			
			return base.PreDraw(npc, spriteBatch, drawColor);
		}
	}
}