using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.VoidClass
{
    class VoidUtils
    {
        private static Color VoidDamageColor = new Color(186, 85, 211);

        public static double StrikeNPCVoid(NPC npc, int Damage, float knockBack, int hitDirection, bool crit = false, bool noEffect = false, bool fromNet = false)
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
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - 10, npc.width, npc.height), color, (int)num, crit, false);
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

            if (Main.rand.Next(0, 100) > 70)
            {
                CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), VoidDamageColor, "Void armor!", crit, false);
            }

            return 0.0;
        }
    }
}
