using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Inventory;
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
        private Item[] slot;

        public CapacitorEntity()
        {
            slot = new Item[4];
            for (int i = 0; i < slot.Length; i++)
            {
                slot[i] = new Item();
                slot[i].TurnToAir();
            }
        }
        
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
                Item extraSlot = slot[i];
                tag.Add("slot" + i, extraSlot);
                itemSlotId++;
            }
        }

        public override void LoadEntity(TagCompound tag)
        {
            InitializeItemSlot();
            for (int i = 0; i < slot.Length; i++)
            {
                Item item = tag.Get<Item>("slot" + i);
                SetAir(ref item);
                slot[i] = item;
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
            return tile.active() && (tile.type == ModContent.TileType<BasicTECapacitor>() || tile.type == ModContent.TileType<AdvancedTECapacitor>());
        }

        public override void Update()
        {
            if (energy == null)
            {
                energy = new EnergyCore(maxEnergy);
            }
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            InitializeItemSlot();
            energy = new EnergyCore(maxEnergy);
            return Place(i - 1, j - 1);
        }

        private void InitializeItemSlot()
        {
            slot = new Item[4];
            for(int i = 0; i < slot.Length; i++)
            {
                slot[i] = new Item();
                slot[i].TurnToAir();
            }
        }
    }
}
