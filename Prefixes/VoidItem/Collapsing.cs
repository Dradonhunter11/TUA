using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API.VoidClass;

namespace TUA.Prefixes.VoidItem
{
    class Collapsing : VoidItemPrefix
    {


        public override void ApplyVoid(VoidDamageItem item)
        {
            item.VoidDamageMultplier = 0.50f;
            return;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("Collapsing", new Collapsing());
            }
            return false;
        }
    }
}
