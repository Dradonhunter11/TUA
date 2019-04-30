using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using TUA.API.FurnaceRework;

namespace TUA.API.Inventory.UI
{
    class FuelSlot : InputOutputSlot
    {
        private BudgetCore core; 

        public FuelSlot(ExtraSlot boundSlot, Texture2D slotTexture, BudgetCore core) : base(boundSlot, slotTexture)
        {
            this.core = core;
            CalculatedStyle s = GetInnerDimensions();
            currentSlotVector = new Vector2(s.X, s.Y);
            this.boundSlot = boundSlot;
        }

        public override void Update(GameTime gameTime)
        {
            if (!boundSlot.IsEmpty)
            {
                boundSlot.ManipulateCurrentStack(-1);
                core.addEnergy(1);
            }
        }
    }
}
