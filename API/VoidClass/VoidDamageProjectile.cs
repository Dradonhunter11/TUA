using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    class VoidDamageProjectile : ModProjectile
    {

        private int _voidDamage = 0;
        private bool _void = false;

        public bool Void
        {
            get { return _void; }
            set { _void = value;}
        }

        public int VoidDamage
        {
            get { return _voidDamage; }
            set { _voidDamage = value;}
        }


        public override bool Autoload(ref string name)
        {
            if (name == "VoidDamageProjectile")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            VoidUtils.StrikeNPCVoid(target, _voidDamage, knockback, hitDirection, false, false, false);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }




    }
}
