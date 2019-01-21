using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TUA.API;

namespace TUA.NPCs.Memefest
{
    class InsanlyHardEoCForNothing : TUAModNPC
    {
        private int _currentFrame = 1;
        private int _animationTimer = 50;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Insanly Hard Eye of Cthulhu for nothing");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 110;
            npc.damage = 1;
            npc.defense = 0;
            npc.lifeMax = 28000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.timeLeft = NPC.activeTime * 30;
            npc.boss = true;
            npc.value = 30000f;
            npc.npcSlots = 5f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65 * bossLifeScale);
        }

        public override void ultraScaleDifficylty(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 5 * (TUAWorld.EoCDeath + 1));
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.EyeMask, 1, false, -1);
                }

                if (Main.rand.Next(40) == 0 || (Main.expertMode && Main.rand.Next(20) == 0))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Binoculars);
                }

                if (Main.expertMode)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.EoCShield, 1, false, -1);
                }

                var num44 = 1;
                if (Main.expertMode)
                {
                    num44 = 2;
                }

                for (var m = 0; m < num44; m++)
                {
                    if (WorldGen.crimson)
                    {
                        var stack = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrimtaneOre, stack);
                        stack = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrimtaneOre, stack);
                        stack = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrimtaneOre, stack);
                        stack = Main.rand.Next(3) + 1;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrimsonSeeds, stack);
                    }
                    else
                    {
                        var stack2 = Main.rand.Next(30) + 20;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.UnholyArrow, stack2);
                        stack2 = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DemoniteOre, stack2);
                        stack2 = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DemoniteOre, stack2);
                        stack2 = Main.rand.Next(20) + 10;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DemoniteOre, stack2);
                        stack2 = Main.rand.Next(3) + 1;
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CorruptSeeds, stack2);
                    }
                }
            }

            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.EyeofCthulhuTrophy);
            }

            // TODO: Change this
            //NPC.downedBoss1 = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                var num447 = 0;
                while (num447 < damage / npc.lifeMax * 100.0)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f);
                    num447++;
                }
                return;
            }

            for (var num448 = 0; num448 < 150; num448++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 5, 2 * hitDirection, -2f);
            }

            for (var num449 = 0; num449 < 2; num449++)
            {
                Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 2);
                Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7);
                Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 9);
                Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 10);
                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (_animationTimer == 0)
            {
                npc.frame.Y = frameHeight * _currentFrame;
                _currentFrame++;
                if (_currentFrame == 3)
                {
                    _currentFrame = 1;
                }
                if (npc.ai[0] > 1f) // Phase 2
                {
                    npc.frame.Y += frameHeight * 3;
                }
                _animationTimer = 25;
            }
            _animationTimer--;
        }

        public enum AIPhase
        {
            Start,
            Bezerk,
            Cooldown
        }

        public override void AI()
        {
            if (!AIInit())
                return;

            switch ((AIPhase)npc.ai[1])
            {
                case AIPhase.Start:
                    AIStart();
                    break;
                case AIPhase.Bezerk:
                    AIBezerk();
                    break;
                case AIPhase.Cooldown:
                    AICooldown();
                    break;
            }
        }

        public bool AIInit()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
            }

            var dead = Main.player[npc.target].dead;

            var dx = npc.position.X + npc.width / 2f - Main.player[npc.target].position.X - Main.player[npc.target].width / 2f;
            var dy = npc.position.Y + npc.height - 59f - Main.player[npc.target].position.Y - Main.player[npc.target].height / 2f;
            var directionToPlayer = (float)Math.Atan2(dy, dx) + 1.57f;

            if (directionToPlayer < 0f)
            {
                directionToPlayer += 6.283f;
            }
            else if (directionToPlayer > 6.283)
            {
                directionToPlayer -= 6.283f;
            }

            if (npc.rotation > directionToPlayer && npc.rotation < directionToPlayer)
            {
                npc.rotation = directionToPlayer;
            }

            if (npc.rotation < 0f)
            {
                npc.rotation += 6.283f;
            }
            else if (npc.rotation > 6.283)
            {
                npc.rotation -= 6.283f;
            }

            if (npc.rotation > directionToPlayer && npc.rotation < directionToPlayer)
            {
                npc.rotation = directionToPlayer;
            }

            if (Main.rand.Next(5) == 0)
            {
                var dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
                var dust1 = Main.dust[dust];
                dust1.velocity.X = dust1.velocity.X * 0.5f;
                var dust2 = Main.dust[dust];
                dust2.velocity.Y = dust2.velocity.Y * 0.1f;
            }

            if (dead) // Only night-time with living player
            {
                npc.velocity.Y = npc.velocity.Y - 0.04f;
                if (npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                    return false;
                }
            }
            return true;
        }

        public void AIStart()
        {
            var yOffset = 600f;
            var divisor = 9f;
            var offset = 0.3f;
            var vector9 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            var distanceX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2f - vector9.X;
            var distanceY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2f + yOffset - vector9.Y;
            var distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            distance = divisor / distance;
            distanceX *= distance;
            distanceY *= distance;

            // Slight movement
            if (npc.velocity.X < distanceX)
            {
                npc.velocity.X = npc.velocity.X + offset;
                if (npc.velocity.X < 0f && distanceX > 0f)
                {
                    npc.velocity.X = npc.velocity.X + offset;
                }
            }
            else if (npc.velocity.X > distanceX)
            {
                npc.velocity.X = npc.velocity.X - offset;
                if (npc.velocity.X > 0f && distanceX < 0f)
                {
                    npc.velocity.X = npc.velocity.X - offset;
                }
            }
            if (npc.velocity.Y < distanceY)
            {
                npc.velocity.Y = npc.velocity.Y + offset;
                if (npc.velocity.Y < 0f && distanceY > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + offset;
                }
            }
            else if (npc.velocity.Y > distanceY)
            {
                npc.velocity.Y = npc.velocity.Y - offset;
                if (npc.velocity.Y > 0f && distanceY < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - offset;
                }
            }

            npc.ai[2] += 1f;
            if (npc.ai[2] >= 70f)
            {
                npc.TargetClosest();
                AIGotoPhase(AIPhase.Bezerk);
                npc.ai[2] = -1f;
                npc.netUpdate = true;
            }

            AIGotoPhase(AIPhase.Bezerk);
        }

        public void AIBezerk()
        {
            if (Main.netMode != 1)
            {
                npc.TargetClosest();
                var multiplier = 40f;
                var npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);

                var xSpeed = Main.player[npc.target].position.X + Main.player[npc.target].width / 2f - npcCenter.X;
                var ySpeed = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2f - npcCenter.Y;
                var speed = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
                 speed += 10000f - speed;

                if (speed < 5f)
                {
                    speed = 5f;
                }

                if (speed > 15f)
                {
                    speed = 15f;
                }
                speed *= 2f;

                // Bezerk Dash Speed is calculated by taking the distance to the player minus the player's velocity multiplied by the distance
                //xSpeed -= Main.player[npc.target].velocity.X * speed;
                //ySpeed -= Main.player[npc.target].velocity.Y * speed / 4f;
                xSpeed *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                ySpeed *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                xSpeed *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                ySpeed *= 1f + Main.rand.Next(-10, 11) * 0.01f;

                var velocityDirection = (float)Math.Sqrt(xSpeed * xSpeed + ySpeed * ySpeed);
                velocityDirection = multiplier / velocityDirection;

                // Bezerk Dash
                npc.velocity.X = xSpeed * velocityDirection;
                npc.velocity.Y = ySpeed * velocityDirection;
                npc.velocity.X = npc.velocity.X + Main.rand.Next(-20, 21) * 0.1f;
                npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-20, 21) * 0.1f;

                // More Bezerk Dash
                npc.velocity.X = npc.velocity.X + Main.rand.Next(-50, 51) * 0.1f;
                npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-50, 51) * 0.1f;
                var velX = Math.Abs(npc.velocity.X);
                var velY = Math.Abs(npc.velocity.Y);
                if (npc.Center.X > Main.player[npc.target].Center.X)
                {
                    velY *= -1f;
                }

                if (npc.Center.Y > Main.player[npc.target].Center.Y)
                {
                    velX *= -1f;
                }
                npc.velocity.X = velY + npc.velocity.X;
                npc.velocity.Y = velX + npc.velocity.Y;
                npc.velocity.Normalize();
                npc.velocity *= multiplier;
                npc.velocity.X = npc.velocity.X + Main.rand.Next(-20, 21) * 0.1f;
                npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-20, 21) * 0.1f;

                // Play Bezerk sound
                Main.PlaySound(36, (int)npc.position.X, (int)npc.position.Y, -1);

                AIGotoPhase(AIPhase.Cooldown);

                npc.netUpdate = true;

                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
            }
        }

        public void AICooldown()
        {
            var bezerkDistanceLimit = 20f;
            float bezerkDelay = 0;//13f;

            npc.ai[2] += 1f;
            if ((int)npc.ai[2] == (int)bezerkDistanceLimit && Vector2.Distance(npc.position, Main.player[npc.target].position) < 200f)
            {
                npc.ai[2] -= 1f;
            }

            // Slowdown and Stop
            if (npc.ai[2] >= bezerkDistanceLimit)
            {
                npc.velocity *= 0.95f;

                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }

                if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                {
                    npc.velocity.Y = 0f;
                }
            }
            else
            {
                // Face player (predictive based on player velocity)
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
            }

            // Timer for bezerk dash
            var timerMax = bezerkDistanceLimit + bezerkDelay;
            if (npc.ai[2] >= timerMax)
            {
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
                npc.ai[2] = 0f;
                {
                    AIGotoPhase(AIPhase.Bezerk);
                }
            }
        }

        public void AIGotoPhase(AIPhase phase)
        {
            npc.ai[1] = (int)phase;
        }
    }
}
