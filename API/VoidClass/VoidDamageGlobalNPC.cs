using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    class VoidDamageGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity { get { return true; } }

        private Color VoidDamageColor = new Color(186, 85, 211);

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.modProjectile is VoidDamageProjectile) {
                VoidDamageProjectile proj = projectile.modProjectile as VoidDamageProjectile;
                StrikeNPCVoid(npc, damage, knockback, hitDirection, crit);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.modItem is VoidDamageItem)
            {
                VoidDamageItem _item = item.modItem as VoidDamageItem;
                StrikeNPCVoid(npc, _item.VoidDamage, knockback, 0, crit);
            }
        }

        public double StrikeNPCVoid(NPC npc, int Damage, float knockBack, int hitDirection, bool crit = false, bool noEffect = false, bool fromNet = false)
        {
            bool flag = Main.netMode == 0;

            if (!npc.active || npc.life <= 0)
            {
                return 0.0;
            }
            double num = (double)Damage;
            int num2 = npc.defense;
            if (npc.ichor)
            {
                num2 -= 20;
            }
            if (npc.betsysCurse)
            {
                num2 -= 40;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (NPCLoader.StrikeNPC(npc, ref num, num2, ref knockBack, hitDirection, ref crit))
            {
                num = Damage;
                if (crit)
                {
                    num *= 2.0;
                }
                if (npc.takenDamageMultiplier > 1f)
                {
                    num *= (double)npc.takenDamageMultiplier;
                }
            }
            if ((npc.takenDamageMultiplier > 1f || Damage != 9999) && npc.lifeMax > 1)
            {
                if (npc.friendly)
                {
                    Color color = VoidDamageColor;
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y- 10, npc.width, npc.height), color, (int)num, crit, false);
                }
                else
                {
                    Color color2 = VoidDamageColor;
                    if (fromNet)
                    {
                        color2 = VoidDamageColor;
                    }
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)num, crit, false);
                }
            }
            if (num >= 1.0)
            {
                if (flag)
                {
                    npc.PlayerInteraction(Main.myPlayer);
                }
                npc.justHit = true;
                if (npc.townNPC)
                {
                    bool flag2 = npc.aiStyle == 7 && (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 16f || npc.ai[0] == 17f);
                    if (flag2)
                    {
                        NPC nPC = Main.npc[(int)npc.ai[2]];
                        if (nPC.active)
                        {
                            nPC.ai[0] = 1f;
                            nPC.ai[1] = (float)(300 + Main.rand.Next(300));
                            nPC.ai[2] = 0f;
                            nPC.localAI[3] = 0f;
                            nPC.direction = hitDirection;
                            nPC.netUpdate = true;
                        }
                    }
                    npc.ai[0] = 1f;
                    npc.ai[1] = (float)(300 + Main.rand.Next(300));
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.direction = hitDirection;
                    npc.netUpdate = true;
                }
                if (npc.aiStyle == 8 && Main.netMode != 1)
                {
                    if (npc.type == 172)
                    {
                        npc.ai[0] = 450f;
                    }
                    else if (npc.type == 283 || npc.type == 284)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            npc.ai[0] = 390f;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.type == 533)
                    {
                        if (Main.rand.Next(3) != 0)
                        {
                            npc.ai[0] = 181f;
                            npc.netUpdate = true;
                        }
                    }
                    else
                    {
                        npc.ai[0] = 400f;
                    }
                    npc.TargetClosest(true);
                }
                if (npc.aiStyle == 97 && Main.netMode != 1)
                {
                    npc.localAI[1] = 1f;
                    npc.TargetClosest(true);
                }
                if (npc.type == 371)
                {
                    num = 0.0;
                    npc.ai[0] = 1f;
                    npc.ai[1] = 4f;
                    npc.dontTakeDamage = true;
                }
                if (npc.type == 346 && (double)npc.life >= (double)npc.lifeMax * 0.5 && (double)npc.life - num < (double)npc.lifeMax * 0.5)
                {
                    Gore.NewGore(npc.position, npc.velocity, 517, 1f);
                }
                if (npc.type == 184)
                {
                    npc.localAI[0] = 60f;
                }
                if (npc.type == 535)
                {
                    npc.localAI[0] = 60f;
                }
                if (npc.type == 185)
                {
                    npc.localAI[0] = 1f;
                }
                if (!npc.immortal)
                {
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].life -= (int)num;
                        npc.life = Main.npc[npc.realLife].life;
                        npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                    }
                    else
                    {
                        npc.life -= (int)num;
                    }
                }
                if (knockBack > 0f && npc.knockBackResist > 0f)
                {
                    float num3 = knockBack * npc.knockBackResist;
                    if (num3 > 8f)
                    {
                        float num4 = num3 - 8f;
                        num4 *= 0.9f;
                        num3 = 8f + num4;
                    }
                    if (num3 > 10f)
                    {
                        float num5 = num3 - 10f;
                        num5 *= 0.8f;
                        num3 = 10f + num5;
                    }
                    if (num3 > 12f)
                    {
                        float num6 = num3 - 12f;
                        num6 *= 0.7f;
                        num3 = 12f + num6;
                    }
                    if (num3 > 14f)
                    {
                        float num7 = num3 - 14f;
                        num7 *= 0.6f;
                        num3 = 14f + num7;
                    }
                    if (num3 > 16f)
                    {
                        num3 = 16f;
                    }
                    if (crit)
                    {
                        num3 *= 1.4f;
                    }
                    int num8 = (int)num * 10;
                    if (Main.expertMode)
                    {
                        num8 = (int)num * 15;
                    }
                    if (num8 > npc.lifeMax)
                    {
                        if (hitDirection < 0 && npc.velocity.X > -num3)
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X = npc.velocity.X - num3;
                            }
                            npc.velocity.X = npc.velocity.X - num3;
                            if (npc.velocity.X < -num3)
                            {
                                npc.velocity.X = -num3;
                            }
                        }
                        else if (hitDirection > 0 && npc.velocity.X < num3)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X = npc.velocity.X + num3;
                            }
                            npc.velocity.X = npc.velocity.X + num3;
                            if (npc.velocity.X > num3)
                            {
                                npc.velocity.X = num3;
                            }
                        }
                        if (npc.type == 185)
                        {
                            num3 *= 1.5f;
                        }
                        if (!npc.noGravity)
                        {
                            num3 *= -0.75f;
                        }
                        else
                        {
                            num3 *= -0.5f;
                        }
                        if (npc.velocity.Y > num3)
                        {
                            npc.velocity.Y = npc.velocity.Y + num3;
                            if (npc.velocity.Y < num3)
                            {
                                npc.velocity.Y = num3;
                            }
                        }
                    }
                    else
                    {
                        if (!npc.noGravity)
                        {
                            npc.velocity.Y = -num3 * 0.75f * npc.knockBackResist;
                        }
                        else
                        {
                            npc.velocity.Y = -num3 * 0.5f * npc.knockBackResist;
                        }
                        npc.velocity.X = num3 * (float)hitDirection * npc.knockBackResist;
                    }
                }
                if ((npc.type == 113 || npc.type == 114) && npc.life <= 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && (Main.npc[i].type == 113 || Main.npc[i].type == 114))
                        {
                            Main.npc[i].HitEffect(hitDirection, num);
                        }
                    }
                }
                else
                {
                    npc.HitEffect(hitDirection, num);
                }
                if (npc.HitSound != null)
                {
                    Main.PlaySound(npc.HitSound, npc.position);
                }
                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].checkDead();
                }
                else
                {
                    npc.checkDead();
                }
                return num;
            }
            return 0.0;
        }
    }
}
