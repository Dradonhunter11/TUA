using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TUA.NPCs.NewBiome.Wasteland
{
    class WastelandWatcher : ModNPC
    {
        public static void Spawn(int x, int y)
        {
            bool success = false;
            int radius = 1;
            Point spawnPos = new Point();
            float radian = MathHelper.ToRadians(1);
            do
            {
                for (float d = 0; d < MathHelper.TwoPi; d += radian)
                {
                    var i = radius * (int)Math.Sin(d) + x;
                    var j = radius * (int)Math.Cos(d) + y;
                    if (Main.tile[i, j].nactive() && Main.rand.NextBool(3))
                    {
                        spawnPos = new Point(i, j);
                        success = true;
                    }
                }
                radius++;
            }
            while (!success);
            int spawnX = spawnPos.X += spawnPos.X > x ? -1 : 1;
            int spawnY = spawnPos.Y += spawnPos.Y > y ? 1 : -1;
            NPC.NewNPC(spawnX, spawnY, TUA.instance.NPCType("WastelandWatcher"),
                ai0: spawnPos.X, ai1: spawnPos.Y);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Watcher");
        }

        public override void AI()
        {
            
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        float rotation = -1;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (rotation == -1)
            {
                rotation = (npc.Center - new Vector2(npc.ai[0], npc.ai[1])).ToRotation();
            }
            spriteBatch.Draw(Main.npcTexture[npc.type],
                npc.Center, 
                null,
                Color.White,
                rotation, 
                Main.npcTexture[npc.type].Size() / 2,
                0, SpriteEffects.None, 0);
            return false;
        }
    }
}
