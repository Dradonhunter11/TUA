using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    abstract class VoidDamageItem : TUAModItem
    {
        private int TrueVoidDamage = 0;
        protected virtual int VoidDamage { get; set; }

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
            ultra = true;
        }

        public sealed override void GetWeaponDamage(Player player, ref int damage)
        {
            VoidDamage = TrueVoidDamage;
            GetVoidWeaponDamage(player, VoidDamage, ref damage);
        }

        public void GetVoidWeaponDamage(Player player, int VoidDamage, ref int NonVoidDamage)
        {
            VoidDamagePlayer p = player.GetModPlayer<VoidDamagePlayer>();
            VoidDamage *= (int)p.VoidDamage;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            int index = tooltips.IndexOf(tt);

            if (tt != null)
            {
                tooltips.Insert(index + 1, new TooltipLine(mod, "VoidDamage", "[c/4B0082:" + VoidDamage + " void damage]"));
            }
        }
    }
}