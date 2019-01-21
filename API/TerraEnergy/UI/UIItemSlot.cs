using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TUA.API.TerraEnergy.UI
{
    abstract class UIItemSlot : UIElement
    {
        internal float scale = .95f;

        protected Item currentItemInSlot = new Item();
        protected bool update = false;
        private bool locked;

        public abstract void sendItemToTileEntity();
        public abstract void sync();
        public UIText counter;

        public UIItemSlot(bool locked)
        {
            this.locked = locked;
        }

        public Item currentItem()
        {
            return currentItemInSlot;
        }

        public UIItemSlot()
        {
            Width.Set(64, 0f);
            Height.Set(64, 0f);

        }


        public void initComponent()
        {
            if (counter == null)
            {
                counter = new UIText("");
                counter.Width.Set(0f, 0f);
                counter.Height.Set(0f, 0f);
                counter.Top.Set(64 + 5, 0f);
                counter.Left.Set(64 + 5, 0f);
                base.Append(counter);
            }

        }


        public override void Update(GameTime gameTime)
        {
            if (update)
            {
                sync();
                update = false;
            }

        }

        public void receiveEntityItem(Item i)
        {
            currentItemInSlot = i;
            update = true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 5f);
            spriteBatch.Draw(Main.inventoryBackTexture, drawPos, null, new Color(73, 94, 171), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
            counter.SetText("", 1f, true);
            if (currentItemInSlot != null)
            {
                Texture2D itemTexture = Main.itemTexture[currentItemInSlot.type];
                Rectangle r = itemTexture.Bounds;
                Rectangle r2 = innerDimensions.ToRectangle();

                int width = r.Width;
                int height = r.Height;

                float drawScale = 1f;
                float availableWidth = (float)Main.inventoryBackTexture.Width * scale;

                if (width > availableWidth || height > availableWidth)
                {
                    if (width > height)
                    {
                        drawScale = availableWidth / width;
                    }
                    else
                    {
                        drawScale = availableWidth / height;
                    }
                }

                drawPos = new Vector2(innerDimensions.X + 10f, innerDimensions.Y + 10f);
                Vector2 vector = Main.inventoryBackTexture.Size() * scale;
                Vector2 pos2 = innerDimensions.Position() + vector / 2 - r2.Size() * drawScale / 2f;

                drawScale *= scale;

                spriteBatch.Draw(Main.itemTexture[currentItemInSlot.type], drawPos, null, Color.White, 0f, r2.Size() * (1f / 2f - 0.5f), drawScale, SpriteEffects.None, 0f);
                counter.SetText(currentItemInSlot.stack.ToString(), 0.29f, true);
            }

        }

        public void UIClosing()
        {
            sendItemToTileEntity();
        }
    }
}
