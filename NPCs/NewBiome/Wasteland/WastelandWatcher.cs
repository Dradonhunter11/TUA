using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.NPCs.NewBiome.Wasteland
{
    class WastelandWatcher : ModNPC
    {
        private const byte AI_TileI = 0;
        private const byte AI_TileJ = 1;
        private const byte AI_State = 2;

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
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void AI()
        {
            // TODO
        }

        public override void SetDefaults()
        {
            npc.height = 38;
            npc.width = 38;
            npc.value = 100000;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath44;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.aiStyle = -1;
            npc.lifeMax = 120;
            npc.defense = 18;
            npc.knockBackResist = 0;
            npc.noGravity = true;
        }

        float rotation = -1;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (rotation == -1)
            {
                rotation = (npc.Center - new Vector2(npc.ai[AI_TileI], npc.ai[AI_TileJ])).ToRotation();
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
