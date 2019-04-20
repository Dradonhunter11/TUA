using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.VoidClass
{
    abstract class VoidDamageItem : TUAModItem
    {
        private int TrueVoidDamage = 0;
        public int VoidDamage = 0;
        public override bool CloneNewInstances { get { return true; } }
        public bool haveNormalDamage { get; set; }

        public float VoidDamageMultplier = 0.0f;
        public float voidArmorDebuffDuration = 0.0f;
        

        public void setVoidDamage(int VoidDamage) {
            TrueVoidDamage = VoidDamage;
            
        }

        public abstract void safeSetDefaults();

        public override bool Autoload(ref string name)
        {
            if (name == "VoidDamageItem")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public sealed override void SetDefaults()
        {
            safeSetDefaults();
            TrueVoidDamage = VoidDamage;
            Ultra = true;
            if (!haveNormalDamage)
            {
                item.damage = 1;
            }
        }

        public sealed override void GetWeaponDamage(Player player, ref int damage)
        {
            VoidDamage = TrueVoidDamage;
            GetVoidWeaponDamage(player, ref VoidDamage, ref damage);
            
        }

        public void GetVoidWeaponDamage(Player player, ref int VoidDamage, ref int NonVoidDamage)
        {
            TUAPlayer p = player.GetModPlayer<TUAPlayer>();
            VoidDamage += (int)(VoidDamage * VoidDamageMultplier);
            VoidDamage *= (int)p.voidDmg;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            int index = tooltips.IndexOf(tt);
            if (VoidDamageMultplier != 0.0)
            {
                tooltips.Add(new TooltipLine(mod, "VoidDamageMultiplier", "[c/A020F0: +" + VoidDamageMultplier*100 + "% void damage multiplier]"));
            }
            if (voidArmorDebuffDuration != 0.0)
            {
                tooltips.Add(new TooltipLine(mod, "VoidArmorMultiplier", "[c/A020F0: +" + VoidDamage + "% time on Void Armor debuff]"));

            }
            if (!haveNormalDamage)
            {
                tooltips[index] = new TooltipLine(mod, "VoidDamage", "[c/4B0082:" + VoidDamage + " void damage]");
                return;
            }
            if (tt != null)
            {
                tooltips.Insert(index + 1, new TooltipLine(mod, "VoidDamage", "[c/4B0082:" + VoidDamage + " void damage]"));
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (item.prefix == mod.PrefixType(""))
            {

            }
        }
    }
}