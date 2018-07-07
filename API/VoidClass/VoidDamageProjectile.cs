using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.VoidClass
{
    class VoidDamageProjectile : ModProjectile
    {
        public override bool Autoload(ref string name)
        {
            if (name == "VoidDamageProjectile")
            {
                return false;
            }
            return base.Autoload(ref name);
        }
    }
}
