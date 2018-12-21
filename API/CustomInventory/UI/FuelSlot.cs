using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using TerrariaUltraApocalypse.API.FurnaceRework;

namespace TerrariaUltraApocalypse.API.CustomInventory.UI
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
            if (!boundSlot.isEmpty())
            {
                boundSlot.manipulateCurrentStack(-1);
                core.addEnergy(1);
            }
        }
    }
}
