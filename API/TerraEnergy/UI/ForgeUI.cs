using Terraria.UI;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.API.TerraEnergy.UI
{
    class ForgeUI : UIState
    {

        public static bool visible = false;
        private ForgeItemSlot[] item;
        private TerraForgeEntity currentForge;


        public override void OnInitialize()
        {
            item = new ForgeItemSlot[2];

            item[0] = new ForgeItemSlot();
            
            base.OnInitialize();
        }

        public void receiveFurnaceEntity(TerraForgeEntity entity)
        {
            currentForge = entity;
        }
    }

    class ForgeItemSlot : UIItemSlot
    {
        public override void sendItemToTileEntity()
        {
        }

        public override void sync()
        {
        }
    }
}