using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.VoidClass
{
    class VoidDamageGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity { get { return true; } }

        

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.modProjectile is VoidDamageProjectile) {
                VoidDamageProjectile proj = projectile.modProjectile as VoidDamageProjectile;
                VoidUtils.StrikeNPCVoid(npc, damage, knockback, hitDirection, crit);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.modItem is VoidDamageItem)
            {
                VoidDamageItem _item = item.modItem as VoidDamageItem;
                VoidUtils.StrikeNPCVoid(npc, _item.VoidDamage, knockback, 0, crit);
            }
        }

        
    }
}
