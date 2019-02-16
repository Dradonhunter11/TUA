using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    public abstract class ItemPickupStage : BaseStage
    {
        public abstract int ItemID { get; }
        public virtual int quantity => 1;

        public sealed override bool CheckCondition()
        {
            return Main.LocalPlayer.inventory.Any(i => i.type == ItemID && i.stack >= quantity) && AdditionalCondition();
        }

        public virtual bool AdditionalCondition()
        {
            return true;
        }

    }
}
