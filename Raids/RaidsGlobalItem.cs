using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Raids
{
    class RaidsGlobalItem : GlobalItem
    {
        public override bool Autoload(ref string name)
        {
            
            return false;
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.type == ItemID.GuideVoodooDoll)
            {
                item.SetDefaults(mod.ItemType("GuideVoodooDoll"));
            }
        }
    }
}
