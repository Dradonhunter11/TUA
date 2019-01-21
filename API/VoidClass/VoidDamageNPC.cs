using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TUA.API.VoidClass
{
    class VoidDamageNPC : TUAModNPC
    {
        public override bool Autoload(ref string name)
        {
            if (name == "VoidDamageNPC")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public override void SetDefaults()
        {
            
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }
    }
}
