using System.Linq;
using Terraria;

namespace TUA.Raids.Script.Stage
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
