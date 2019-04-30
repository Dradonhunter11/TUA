using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace TUA.API.Inventory
{
    public class ExtraSlot : UIElement
    {
        private readonly int _context;
        private readonly float _scale;
        public Func<Item, bool> validItemFunc;

        public ExtraSlot(int context = ItemSlot.Context.ChestItem, float scale = 0.85f)
        {
            item = new Item();
            item.TurnToAir();

            _context = context;
            _scale = scale;
            item = new Item();
            item.SetDefaults();

            Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
            Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);

            validItemFunc = i => true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            FixMouse();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (validItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref item, _context);
                }
            }

            FixMouse(false);

            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }

        public Item GetItem()
        {
            Item tempItem;

            if (item.IsAir)
            {
                tempItem = item;
                item.TurnToAir();
                return tempItem;
            }

            tempItem = item.Clone();
            tempItem.stack = 1;
            item.stack -= 1;
            return tempItem;
        }

        public bool SetItem(ref Item newItem)
        {
            if (!item.IsAir)
            {
                return false;
            }
            Swap(ref newItem);
            return true;
        }

        public bool ManipulateCurrentItem(Item newItem, int i = 1)
        {
            if (item.type == newItem.type)
            {
                int calculateAfterStack = item.stack + newItem.stack;
                if (calculateAfterStack > item.maxStack)
                {
                    int calculateItemToSubstract = calculateAfterStack - item.stack;
                    item.stack += calculateItemToSubstract;
                    newItem.stack -= calculateItemToSubstract;
                }
            }
            return false;
        }

        public void ManipulateCurrentStack(int number)
        {
            int preCalculate = item.stack + number;
            if (preCalculate >= item.maxStack)
            {
                int overflow = preCalculate - item.maxStack;
                number = overflow;
                item.stack = item.maxStack;
                return;
            }
            item.stack = preCalculate;
        }

        public void ManipulateSingleItem(ref int targetItem)
        {
            targetItem++;
            item.stack--;

            if (item.stack == 0)
                item.TurnToAir();
        }

        public void Swap(ref Item mouseItem) => Utils.Swap(ref item, ref mouseItem);

        void FixMouse(bool fix = true)
        {
            if (fix)
            {
                MouseX = Main.mouseX;
                MouseY = Main.mouseY;
                LastMouseX = Main.lastMouseX;
                LastMouseY = Main.lastMouseY;
                LastScreenWidth = Main.screenWidth;
                LastScreenHeight = Main.screenHeight;

                PlayerInput.SetZoom_Unscaled();
                PlayerInput.SetZoom_MouseInWorld();
            }
            else
            {
                Main.lastMouseX = LastMouseX;
                Main.lastMouseY = LastMouseY;
                Main.mouseX = MouseX;
                Main.mouseY = MouseY;
                Main.screenWidth = LastScreenWidth;
                Main.screenHeight = LastScreenHeight;
            }
        }

        public Item item;
        public int ItemStack => item.stack;
        public int ItemType => item.type;

        public bool IsEmpty => item.IsAir;

        public Texture2D ItemTexture => Main.itemTexture[item.type];

        public int MouseX { get; private set; }
        public int MouseY { get; private set; }

        public int LastMouseX { get; private set; }
        public int LastMouseY { get; private set; }

        public int LastScreenWidth { get; private set; }
        public int LastScreenHeight { get; private set; }
    }
}
