using Terraria;
using Terraria.ModLoader.IO;
using TUA.API.CustomInventory;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.UI;
using TUA.Utilities;

namespace TUA.API.TerraEnergy.TileEntities
{
    class CapacitorEntity : StorageEntity
    {
        public int maxTransferRate;
        public CapacitorUI CapacitorUi;
        private ExtraSlot[] slot;
        

        public void Activate()
        {
            InitializeItemSlot();
            CapacitorUi = new CapacitorUI(slot, this);
            Main.playerInventory = true;
            UIManager.OpenMachineUI(CapacitorUi);
        }

        public override void SaveEntity(TagCompound tag)
        {
            int itemSlotId = 0;
            for (int i = 0; i < slot.Length; i++)
            {
                ExtraSlot extraSlot = slot[i];
                tag.Add("slot" + 0, extraSlot.GetItem());
                itemSlotId++;
            }
        }

        public override void LoadEntity(TagCompound tag)
        {
            InitializeItemSlot();
            int itemSlotId = 0;
            for (int i = 0; i < slot.Length; i++)
            {
                ExtraSlot extraSlot = slot[i];
                Item item = tag.Get<Item>("slot" + itemSlotId);
                SetAir(ref item);
                extraSlot.SetItem(ref item);
                itemSlotId++;
            }
        }

        public void SetAir(ref Item item)
        {
            if (item.Name == "Unloaded Item")
            {
                item.TurnToAir();
            }
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && (tile.type == mod.TileType<BasicTECapacitor>());
        }

        public override void Update()
        {
            if (energy == null)
            {
                energy = new Core(maxEnergy);
            }
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            InitializeItemSlot();
            energy = new Core(maxEnergy);
            return Place(i - 1, j - 1);
        }

        private void InitializeItemSlot()
        {
            slot = new ExtraSlot[4];
            for(int i = 0; i < slot.Length; i++)
            {
                slot[i] = new ExtraSlot();
            }
        }
    }
}
