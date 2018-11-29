using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    class VoidDamagePlayer : ModPlayer
    {
        public float VoidDamage = 1f;
        public float VoidKnockback = 0f;
        public int VoidCrit = 100;

        public int voidCatalyst = 0; 

        public static VoidDamagePlayer GetVoidPlayer(Player p)
        {
            return p.GetModPlayer<VoidDamagePlayer>();
        }

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            VoidDamage = 1f;
            VoidKnockback = 0f;
            VoidCrit = 100;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(item.modItem is VoidDamageItem)
            {
                VoidDamageItem voidItem = item.modItem as VoidDamageItem;
                

            }
        }
    }
}
