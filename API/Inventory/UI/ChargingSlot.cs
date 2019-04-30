using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TUA.API.TerraEnergy.EnergyAPI;

namespace TUA.API.Inventory.UI
{
    class ChargingSlot : InputOutputSlot
    {
        private StorageEntity storageEntity;
        private readonly int maxTransferRate;

        public ChargingSlot(ExtraSlot boundSlot, Texture2D slotTexture, StorageEntity storageEntity, int maxTransferRate) : base(boundSlot, slotTexture)
        {
            this.storageEntity = storageEntity;
            this.maxTransferRate = maxTransferRate;
        }

        public override void Update(GameTime gameTime)
        {
            ModItem item = boundSlot.GetItem().modItem;
            if (item is EnergyItem energyItem)
            {
                if (!energyItem.isFull())
                {
                    energyItem.AddEnergy(storageEntity.energy.ConsumeEnergy(maxTransferRate));
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDim = GetInnerDimensions();
            Vector2 position = new Vector2(innerDim.X, innerDim.Y - 15);
            ModItem item = boundSlot.GetItem()?.modItem;
            if (item != null)
            {
                if (item is EnergyItem)
                {
                    EnergyItem energyItem = item as EnergyItem;
                    if (energyItem.isFull())
                    {
                        spriteBatch.DrawString(Main.fontMouseText, "Full!", position, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(Main.fontMouseText, "Charging", position, Color.White);
                    }
                }
                else
                {
                    spriteBatch.DrawString(Main.fontMouseText, "Can't charge", position, Color.White);
                }
            }
        }
    }
}
