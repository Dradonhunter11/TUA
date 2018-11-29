using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    abstract class VoidItemPrefix : ModPrefix
    {
        public abstract void ApplyVoid(VoidDamageItem item);

        public sealed override void Apply(Item item)
        {
            if (item.modItem is VoidDamageItem)
            {
                
                VoidDamageItem i = item.modItem as VoidDamageItem;
                ApplyVoid(i);
            }
        }

        public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }

        public override bool CanRoll(Item item)
        {
            if(item.modItem is VoidDamageItem)
            {
                return true;
            }
            return false;
        }

        
    }
}
