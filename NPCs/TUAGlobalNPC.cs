using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TerrariaUltraApocalypse;
using Microsoft.Xna.Framework;

namespace TerrariaUltraApocalypse.NPCs
{
    class TUAGlobalNPC : GlobalNPC
    {
        //Eye Of Cthulhu after moon lord
        private static int EoCcooldown = 250;

        public static bool EoCUltraActivated = false;
        private static bool EoCAttackReady = false;
        private static int immunityCooldown = 50;
        private static int damageCap = 1250;
        private static int healTimer = 200;
        private static int currentDamage = 0;

        private static int drainTimer = 25;

        //EoC clone section
        private static bool spawnClone1 = false;
        private static bool spawnClone2 = false;
        private static bool spawnClone3 = false;
        private static int cloneID1;
        private static int cloneID2;
        private static int cloneID3;


        //EotW section
        private static bool headDied = false;
        private static int mode = 0;
        private static int cooldown = 0;


        public override void SetDefaults(NPC npc)
        {
            EoCUltraActivated = false;
            if (NPC.downedMoonlord && !npc.boss)
            {
                npc.lifeMax *= 10;
            }
            if (npc.type == NPCID.EyeofCthulhu && npc.boss && NPC.downedMoonlord)
            {
                npc.damage = 50;
                npc.defense = 50;
                npc.lifeMax *= (TUAWorld.EoCDeath + 1) * 2;
                //Main.NewText("<Eye of cthulhu> - You really think we would let the lord dying from you? Well, welcome to the ultra mode muhahaha", Color.White);
            }
            else if (npc.type == NPCID.EaterofWorldsHead)
            {
                npc.lifeMax = 30000;

            }
            else if (npc.type == NPCID.EaterofWorldsBody)
            {
                npc.lifeMax = 1;
                npc.dontTakeDamage = true;
            }
            else if (npc.type == NPCID.EaterofWorldsTail)
            {
                npc.lifeMax = 1;
                npc.dontTakeDamage = true;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu && npc.boss && NPC.downedMoonlord)
            {
                if (npc.life <= npc.lifeMax - npc.lifeMax / 3)
                {
                    Player p = Main.player[npc.target];
                    npc.damage = 75;
                    npc.defense = 100;
                    npc.buffImmune[24] = true;
                    npc.Opacity = 0.05f;
                    if (!TerrariaUltraApocalypse.EoCUltraActivated)
                    {
                        Main.NewText(
                            "<Ultra Eye of Cthulhu> - Did you really think it will be easy? No you are wrong, right before you summoned me, my god gave me a brand new destructive source of power.",
                            Color.White);
                        Main.NewText("You feel that you're health is being drained...", Color.Red);
                        if (p.statDefense >= 80)
                        {
                            int pDust = Dust.NewDust(new Vector2(p.Center.X, p.Center.Y), 75, 50, DustID.Smoke);
                            Main.dust[pDust].velocity *= 0.2f;
                            Main.NewText("<Ultra Eye of Cthulhu> Your armor... So weak...", Color.Red);
                        }
                        int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), 75, 50, DustID.FlameBurst);
                        Main.dust[dust].velocity *= 0.2f;
                        TerrariaUltraApocalypse.EoCUltraActivated = true;
                    }
                    return true;
                }
            }
            return base.PreAI(npc);
        }




        public override void AI(NPC npc)
        {
            Player p = Main.player[npc.target];
            if (npc.type == NPCID.EyeofCthulhu && npc.boss && NPC.downedMoonlord)
            {
                {
                    p.AddBuff(mod.BuffType("EoCNerf"), 99999, false);
                    npc.daybreak = false;
                    p.buffImmune[145] = false;
                    p.AddBuff(145, 1000);
                    p.setSolar = false;
                    p.setNebula = false;
                    p.turtleArmor = false;
                    p.ghostHeal = false;
                    if (p.statDefense >= 80)
                    {
                        p.statDefense = 80;
                    }
                    Vector2 oldVector = npc.velocity;
                    if (npc.life <= npc.lifeMax - npc.lifeMax / 4 && !spawnClone1)
                    {
                        spawnClone1 = true;
                        cloneID1 = NPC.NewNPC((int)(p.Center.X + 100f), (int)(p.Center.Y - 100f), NPCID.EyeofCthulhu,
                            0, 0, 0, 0, Main.myPlayer);
                        Main.npc[cloneID1].boss = false;
                        Main.npc[cloneID1].AddBuff(24, 9999);
                        Main.npc[cloneID1].damage = 0;
                        Main.npc[cloneID1].lifeMax = 30;
                        Main.npc[cloneID1].life = 1;
                        Main.npc[cloneID1].dontTakeDamage = true;
                        Main.npc[cloneID1].velocity = oldVector;
                        Main.npc[cloneID1].GivenName = "Ultra Eye of Cthulhu clone";
                    }

                    if (npc.life <= npc.lifeMax - npc.lifeMax / 2 && !spawnClone2)
                    {
                        spawnClone2 = true;
                        cloneID2 = NPC.NewNPC((int)(p.Center.X + 100f), (int)(p.Center.Y - 100f), NPCID.EyeofCthulhu,
                            0, 0, 0, 0, Main.myPlayer);
                        Main.npc[cloneID2].boss = false;
                        Main.npc[cloneID2].AddBuff(24, 9999);
                        Main.npc[cloneID2].damage = 0;
                        Main.npc[cloneID2].lifeMax = 30;
                        Main.npc[cloneID2].life = 1;
                        Main.npc[cloneID2].dontTakeDamage = true;
                        Main.npc[cloneID2].velocity = oldVector;
                        Main.npc[cloneID2].GivenName = "Ultra Eye of Cthulhu clone";
                    }

                    if (npc.life <= npc.lifeMax - (npc.lifeMax / 4) * 3 && !spawnClone3)
                    {
                        spawnClone3 = true;
                        cloneID3 = NPC.NewNPC((int)(p.Center.X + 100f), (int)(p.Center.Y - 100f), NPCID.EyeofCthulhu,
                            0, 0, 0, 0, Main.myPlayer);
                        Main.npc[cloneID3].boss = false;
                        Main.npc[cloneID3].AddBuff(24, 9999);
                        Main.npc[cloneID3].damage = 0;
                        Main.npc[cloneID3].lifeMax = 30;
                        Main.npc[cloneID3].life = 1;
                        Main.npc[cloneID3].dontTakeDamage = true;
                        Main.npc[cloneID3].velocity = oldVector;
                        Main.npc[cloneID3].GivenName = "Ultra Eye of Cthulhu clone";
                    }

                    if (immunityCooldown != 0)
                    {
                        immunityCooldown--;
                    }

                    if (immunityCooldown == 0)
                    {
                        immunityCooldown = 75;
                        currentDamage = 0;
                    }

                    if (npc.life <= npc.lifeMax - npc.lifeMax / 3 && npc.boss)
                    {

                        //npc.modNPC.DisplayName.SetDefault("Ultra Eye of Cthulhu");
                        npc.GivenName = "Ultra Eye of Cthulhu";

                        if (p.dead)
                        {
                            Main.NewText(
                                "<Ultra Eye of Cthulhu> - I knew you were not powerful enough for the ultra mode...");
                            npc.active = false;
                            EoCUltraActivated = false;
                            Main.npc[cloneID1].active = false;
                            Main.npc[cloneID2].active = false;
                            Main.npc[cloneID3].active = false;
                            Main.npc[cloneID1].life = -1;
                            Main.npc[cloneID2].life = -1;
                            Main.npc[cloneID3].life = -1;
                            cloneID1 = 0;
                            cloneID2 = 0;
                            cloneID3 = 0;
                            spawnClone1 = false;
                            spawnClone2 = false;
                            spawnClone3 = false;
                            p.ClearBuff(mod.BuffType("EoCNerf"));
                        }

                        healTimer--;
                        if (healTimer == 0 && npc.life <= npc.lifeMax - npc.lifeMax / 3 - 2000)
                        {
                            npc.HealEffect(1000);
                            npc.life += 1000;
                            healTimer = 250;
                        }

                        if (Main.rand.Next(250) == 0 && npc.life >= 2000)
                        {
                            p.buffImmune[BuffID.Stoned] = false;
                            p.AddBuff(BuffID.Stoned, 30);
                        }


                        npc.damage = 75;
                        if (EoCcooldown != 0)
                        {
                            EoCcooldown -= 1;
                        }

                        if (drainTimer != 0)
                        {
                            drainTimer -= 1;
                        }
                        else if (drainTimer == 0)
                        {
                            p.statLife -= 10;
                            drainTimer = 100;
                        }


                        if (EoCcooldown == 75 && !p.dead)
                        {
                            Main.NewText("Ultra Eye of Cthulhu is preparing a powerful charge attack", Color.Purple);
                        }

                        int shouldTeleport = Main.rand.Next(500);

                        if (EoCcooldown == 0 && !p.dead)
                        {




                            float speed = 8f;
                            Vector2 move = new Vector2(p.Center.X + 100f - npc.Center.X,
                                p.Center.Y + 100f - npc.Center.Y);
                            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                            if (magnitude > speed)
                            {
                                move *= speed / magnitude;
                            }

                            npc.ai[0] = 0;
                            npc.position.Y = p.Center.Y - 100f;
                            npc.position.X = p.Center.X - 100f;
                            int dust = Dust.NewDust(new Vector2(npc.position.X / 2, npc.position.Y / 2), 100, 100,
                                DustID.Shadowflame);
                            Main.dust[dust].velocity *= 0.1f;
                            npc.damage = 75;
                            npc.rotation = 0f;
                            npc.velocity = move;

                            EoCcooldown = 500;
                        }
                        npc.ai[0] = 3;
                    }
                    else
                    {
                        int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 75, 45, DustID.Fire);
                        Main.dust[dust].velocity *= 0.2f;

                    }

                }
            }
            else if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
            {
                int dust = 0;

                if (npc.type == NPCID.EaterofWorldsHead /*&& npc.boss && NPC.downedMoonlord*/)
                {
                    npc.GivenName = "Ultra Eater of the World";
                }
                else
                {
                    npc.lifeMax = 1;
                }

                if (cooldown == 0)
                {
                    mode = Main.rand.Next(0, 4);
                    cooldown = 90000;
                }
                changeEotWDust(dust, npc);
                cooldown--;
            }

            base.AI(npc);
        }

        private void changeEotWDust(int dust, NPC npc)
        {
            switch (mode)
            {
                case 0:
                    dust = Dust.NewDust(new Vector2(npc.position.X - Main.rand.Next(0, 25), npc.position.Y + Main.rand.Next(0, 10)), 8, 8, mod.DustType("ArrowDust"));
                    Main.dust[dust].velocity *= 0.2f;
                    Main.player[Main.myPlayer].AddBuff(mod.BuffType("ArrowDebuff"), 99999, false);
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("BulletDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MeleeDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MagicDebuff"));
                    break;
                case 1:
                    dust = Dust.NewDust(new Vector2(npc.position.X - Main.rand.Next(0, 25), npc.position.Y + Main.rand.Next(0, 10)), 8, 8, mod.DustType("MeleeDust"));
                    Main.player[Main.myPlayer].AddBuff(mod.BuffType("MeleeDebuff"), 99999, false);
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("ArrowDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("BulletDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MagicDebuff"));
                    break;
                case 2:
                    dust = Dust.NewDust(new Vector2(npc.position.X - Main.rand.Next(0, 25), npc.position.Y + Main.rand.Next(0, 10)), 8, 8, mod.DustType("MagicDust"));
                    Main.player[Main.myPlayer].AddBuff(mod.BuffType("MagicDebuff"), 99999, false);
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("ArrowDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("BulletDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MeleeDebuff"));
                    break;
                case 3:
                    dust = Dust.NewDust(new Vector2(npc.position.X - Main.rand.Next(0, 25), npc.position.Y + Main.rand.Next(0, 10)), 8, 8, mod.DustType("BulletDust"));
                    Main.player[Main.myPlayer].AddBuff(mod.BuffType("BulletDebuff"), 99999, false);
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("ArrowDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MeleeDebuff"));
                    Main.player[Main.myPlayer].ClearBuff(mod.BuffType("MagicDebuff"));
                    break;
            }
        }

        public override bool CheckActive(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu && npc.boss && NPC.downedMoonlord)
            {
                if (npc.active && npc.life <= npc.lifeMax - npc.lifeMax / 3)
                {
                    TerrariaUltraApocalypse.EoCUltraActivated = true;
                }
            }
            if (npc.type == NPCID.EaterofWorldsBody)
            {
                npc.life = 1;
            }
            if (npc.type == NPCID.EaterofWorldsTail)
            {
                npc.life = 1;
                return false;
            }
            return base.CheckActive(npc);
        }

        public override bool CheckDead(NPC npc)
        {
            Player p = Main.player[Main.myPlayer];
            if (npc.type == NPCID.EyeofCthulhu && npc.boss && NPC.downedMoonlord)
            {
                if (npc.life <= 0)
                {
                    TerrariaUltraApocalypse.EoCUltraActivated = false;
                    Main.npc[cloneID1].active = false;
                    Main.npc[cloneID2].active = false;
                    Main.npc[cloneID3].active = false;
                    Main.npc[cloneID1].life = -1;
                    Main.npc[cloneID2].life = -1;
                    Main.npc[cloneID3].life = -1;
                    cloneID1 = 0;
                    cloneID2 = 0;
                    cloneID3 = 0;
                    spawnClone1 = false;
                    spawnClone2 = false;
                    spawnClone3 = false;
                    TUAWorld.EoCDeath++;
                    p.ClearBuff(mod.BuffType("EoCNerf"));
                    return true;
                }
            }

            if (npc.type == NPCID.EaterofWorldsHead)
            {
                headDied = true;
                npc.life = 1;
                npc.HealEffect(-1);
            }

            return base.CheckDead(npc);
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail && NPC.downedMoonlord)
            {

            }
            base.OnHitPlayer(npc, target, damage, crit);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (/*TerrariaUltraApocalypse.UltraMode &&*/ npc.type == NPCID.EyeofCthulhu && NPC.downedMoonlord)
            {
                currentDamage += damage;
                if (currentDamage >= damageCap)
                {
                    damage = 0;
                }
            }

            if (npc.type == NPCID.EaterofWorldsHead && NPC.downedMoonlord /*&& TerrariaUltraApocalypse.UltraMode*/)
            {
                if (projectile.arrow && mode == 0)
                {
                    damage *= 5;
                }
                else if (projectile.melee && mode == 1)
                {
                    damage *= 5;
                }
                else if (projectile.aiStyle == ProjectileID.Bullet && mode == 2)
                {
                    damage *= 5;
                }
                else if (projectile.magic && mode == 3)
                {
                    damage *= 5;
                }
            }
        }


        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (/*TerrariaUltraApocalypse.UltraMode &&*/ npc.type == NPCID.EyeofCthulhu && NPC.downedMoonlord)
            {
                currentDamage += (int)damage;
                damageCap = (int)(1250 * TUAWorld.EoCDeath * 1.5);
                if (immunityCooldown != 0 && currentDamage >= damageCap)
                {
                    damage = 0;
                    crit = false;
                }

            }
            return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
        }
    }
}
