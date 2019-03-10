using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Dimension.Solar
{
    class SolarNPC : GlobalNPC
    {
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            SolarWorld solar = TUA.instance.GetModWorld<SolarWorld>();
            if (solar.PillarCrashEvent && solar.PillarDetection())
            {
                drawColor *= 0.1f;
            }
            return base.PreDraw(npc, spriteBatch, drawColor);
        }
    }
}
