using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TUA.API.CustomInventory.UI
{
    class InputOutputSlot : UIElement
    {
        protected ExtraSlot boundSlot;
        private Texture2D slotTexture;
        public bool debug = false;

        protected Vector2 currentSlotVector;

        public int customOffsetX = 2;
        public int customOffsetY = 0;

        public InputOutputSlot(ExtraSlot boundSlot, Texture2D slotTexture)
        {
            CalculatedStyle s = GetInnerDimensions();
            currentSlotVector = new Vector2(s.X, s.Y);
            this.boundSlot = boundSlot;
            this.slotTexture = slotTexture;
        }

        public sealed override void OnInitialize()
        {
            Width.Set(slotTexture.Width, 0f);
            Height.Set(slotTexture.Height, 0f);
            
            OnMouseOver += MouseHover;
        }

        public void MouseHover(UIMouseEvent mouseEvent, UIElement listeningElement)
        {
            if (!boundSlot.IsEmpty)
            {
                Main.hoverItemName = boundSlot.GetItem().HoverName;
            }
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimension = GetInnerDimensions();
            DrawChildren(spriteBatch);
            DrawSelf(spriteBatch);
            spriteBatch.Draw(slotTexture, new Vector2(innerDimension.X, innerDimension.Y), Color.White);
        }
    }
}
