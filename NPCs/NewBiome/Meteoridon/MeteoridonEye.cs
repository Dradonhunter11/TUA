using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.NPCs.NewBiome.Meteoridon
{
    internal class MeteoridonEye : TUAModNPC
    {
        private static Texture2D infernoRing;
        private bool isCharging = false;
        private int animationTimer = 25;
        private int currentFrame = 1;
        private bool aggro = false;
        private int wanderingTimer;
        private int chargeTimer = 50;
        private int chargePower = 0;
        private bool chargeReady = false;


        public Player GetPlayer(NPC npc)
        {
            Player player = Main.player[npc.target];
            return player;
        }

        public float GetDistance(NPC npc, Player p)
        {

            Vector2 centerNPC = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
            Vector2 centerPlayer = new Vector2(p.position.X + (p.width * 0.5f), p.position.Y + (p.height * 0.5f));
            Vector2 distance = centerNPC - centerPlayer;
            return (float)Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y);
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            wanderingTimer = Main.rand.Next(100, 400);
            return base.SpawnNPC(tileX, tileY);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteoridon Burner");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.height = 77 * 2;
            npc.width = 62 * 2;
            npc.lifeMax = 250;
            npc.lavaImmune = true;
            npc.value = 10f;
            npc.damage = 40;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.knockBackResist = 100;
            infernoRing = ModLoader.GetTexture("Terraria/FlameRing");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 1.5);
            npc.lifeMax = (int)(npc.lifeMax * 2.5);
            npc.defense = (int)(npc.defense * 1.2);
        }

        public override void ultraScaleDifficylty(NPC npc)
        {
            npc.damage = (npc.damage * 2);
            npc.lifeMax = (npc.lifeMax * 5);
            npc.defense = (npc.defense * 2);
        }

        public override void AI()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }

            Player p = GetPlayer(npc);
            float distance = GetDistance(npc, p) / 16;

            //Main.NewText(distance > 600f);
            if (distance > 25)
            {
                aggro = false;
            }
            else
            {
                aggro = true;
            }
            if (!aggro || chargePower == 1000)
            {
                Wander();
            }
            else if(aggro && chargePower != 1000)
            {
                Charge(p);
            }

            if (distance < 15)
            {
                p.AddBuff(BuffID.OnFire, chargePower, true);
            }
        }

        private void Charge(Player p)
        {

            isCharging = true;
            

            if (isCharging)
            {
                float subit = (float)Math.PI / 2f;
                npc.velocity = Vector2.Zero;
                spawnAbunchOfDust();
                chargePower++;
                if ((GetDistance(npc, p) < 300 && chargePower > 50) || chargePower == 500)
                {
                    Vector2 nPos = npc.Center;
                    Vector2 pPos = p.Center;
                    chargeReady = true;
                }
            }
            else if (chargeReady && chargePower > 0)
            {
                npc.damage = (40 * chargePower / 100);
            }
        }



        private void Wander()
        {
            npc.rotation = 0f;
            if (wanderingTimer <= 0)
            {
                npc.velocity.X *= -1;
                wanderingTimer = Main.rand.Next(100, 400);

            }
            if (npc.velocity == Vector2.Zero)
            {
                npc.velocity.X = 2;
            }

            if (npc.velocity.X > 2 || npc.velocity.X < 2)
            {
                npc.velocity.X = 2;
            }
            if (npc.velocity.Y != 0)
            {
                npc.velocity.Y = 0;
            }

            if (chargePower == 1000)

            {
                if (npc.spriteDirection == 1)
                    npc.velocity.X = 4;
                else
                    npc.velocity.X = -4;
            }

            isCharging = false;
            chargePower = 0;
            chargeReady = false;
            wanderingTimer--;
        }

        public override void FindFrame(int frameHeight)
        {
            animationTimer--;
            if (animationTimer == 0)
            {
                if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                currentFrame++;
                if (currentFrame >= 8)
                {
                    currentFrame = 0;
                }
                animationTimer = 10;
                npc.frame.Y = frameHeight * currentFrame;
            }
        }



        public override void NPCLoot()
        {
            if (!aggro)
            {
                return;
            }
            Item.NewItem(npc.Center, mod.ItemType("MeteorideScale"), Main.rand.Next(1, 2), true);
        }

        public void spawnAbunchOfDust()
        {
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(npc.Center, 5, 5, mod.DustType("FireDust"), (Main.rand.NextBool()) ? 1 : -1, (Main.rand.NextBool()) ? 1 : -1, 0, Color.White, 0.25f);
                Main.dust[d].scale = 0.25f * chargePower / 50;

            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float opacity = chargePower / 10000f;
            spriteBatch.Draw(infernoRing, new Vector2(npc.Center.X - Main.screenPosition.X - infernoRing.Width / 2, npc.Center.Y - Main.screenPosition.Y - infernoRing.Height / 2), Color.White * opacity);
        }
    }
}