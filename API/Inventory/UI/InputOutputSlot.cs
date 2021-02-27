using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TUA.API.Inventory.UI
{
    class InputOutputSlot : ExtraSlot
    {
        private Texture2D slotTexture;
        public bool debug = false;

        protected Vector2 currentSlotVector;

        public int customOffsetX = 2;
        public int customOffsetY = 0;

        public InputOutputSlot(Ref<Item> reference, Texture2D slotTexture) : base(reference)
        {
            CalculatedStyle s = GetInnerDimensions();
            currentSlotVector = new Vector2(s.X, s.Y);
            this.slotTexture = slotTexture;
        }

        private void OnSlotClick(UIMouseEvent evt, UIElement listeningelement)
        {
            Utils.Swap(ref Main.mouseItem, ref item);
        }

        public sealed override void OnInitialize()
        {
            Width.Set(slotTexture.Width, 0f);
            Height.Set(slotTexture.Height, 0f);
            
            OnMouseOver += MouseHover;
            OnClick += OnSlotClick;
        }

        public void MouseHover(UIMouseEvent mouseEvent, UIElement listeningElement)
        {
            if (!IsEmpty)
            {
                Main.hoverItemName = item.HoverName;
            }
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimension = GetInnerDimensions();
            spriteBatch.Draw(slotTexture, new Vector2(innerDimension.X, innerDimension.Y), Color.White);
            DrawChildren(spriteBatch);
            DrawSelf(spriteBatch);
        }
    }
}
