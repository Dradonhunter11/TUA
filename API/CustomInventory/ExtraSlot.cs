using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
<<<<<<< Updated upstream
=======
using Terraria.GameInput;
>>>>>>> Stashed changes
using Terraria.UI;

namespace TUA.API.CustomInventory
{

    internal class ExtraSlot : UIElement
    {
        public Item Item;
<<<<<<< Updated upstream


        public int ItemStack => Item.stack;
        public int ItemType => Item.type;
=======
        private readonly int _context;
        private readonly float _scale;
        public Func<Item, bool> ValidItemFunc;
>>>>>>> Stashed changes

        public ExtraSlot(int context = ItemSlot.Context.ChestItem, float scale = .85f)
        {
<<<<<<< Updated upstream
            Item = new Item();
            Item.TurnToAir();
=======
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults();

            Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
            Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);

            ValidItemFunc = i => true;
>>>>>>> Stashed changes
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
<<<<<<< Updated upstream
            if (Item.IsAir)
            {
                return Item;
            }
            return Item.Clone();
=======
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            FixMouse();
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            FixMouse(false);

            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
>>>>>>> Stashed changes
        }

        public Item GetItem()
        {
            if (Item.IsAir)
            {
<<<<<<< Updated upstream
                tempItem = Item;
                Item.TurnToAir();
                return tempItem;
            }

            tempItem = Item.Clone();
            tempItem.stack = 1;
            Item.stack -= 1;
            return tempItem;
=======
                return Item;
            }
            return Item.Clone();
>>>>>>> Stashed changes
        }

        public bool SetItem(ref Item newItem)
        {
            if (!Item.IsAir)
            {
                return false;
            }
            Swap(ref newItem);
            return true;
        }

<<<<<<< Updated upstream
        public bool manipulateItem(Item newItem, int i = 1)
=======
        public bool ManipulateCurrentItem(Item newItem, int i = 1)
>>>>>>> Stashed changes
        {
            if (Item.type == newItem.type)
            {
                int calculateAfterStack = Item.stack + newItem.stack;
                if (calculateAfterStack > Item.maxStack)
                {
                    int calculateItemToSubstract = calculateAfterStack - Item.stack;
                    Item.stack += calculateItemToSubstract;
                    newItem.stack -= calculateItemToSubstract;
                }
            }
            return false;
        }

<<<<<<< Updated upstream
        public void manipulateCurrentStack(ref int number)
        {
            int preCalculate = Item.stack + number;
            if (preCalculate >= Item.maxStack)
            {
                int overflow = preCalculate - Item.maxStack;
                number = overflow;
                Item.stack = Item.maxStack;
                return;   
            }
            Item.stack = preCalculate;
        }

        public void manipulateCurrentStack(int number)
        {
            int preCalculate = Item.stack + number;
            if (preCalculate >= Item.maxStack)
            {
                int overflow = preCalculate - Item.maxStack;
                number = overflow;
                Item.stack = Item.maxStack;
=======
        public void ManipulateCurrentStack(int number)
        {
            if (Item.stack <= 0)
            {
                Item.TurnToAir();
>>>>>>> Stashed changes
                return;
            }
            Item.stack = preCalculate;
        }

<<<<<<< Updated upstream
        public void ManipulateSingleItem(ref int targetItem)
        {
            targetItem++;
            Item.stack--;
            if (Item.stack == 0)
            {
                Item.TurnToAir();
            }
        }

        public bool isEmpty()
        {
            return Item.IsAir;
        }

        public Texture2D getItemTexture()
        {
            return Main.itemTexture[Item.type];
        }
=======
            Item.stack += number;
        }

        public bool IsEmpty => Item.IsAir;

        public Texture2D GetItemTexture => Main.itemTexture[Item.type];

        public void Swap(ref Item mouseItem) => Utils.Swap(ref Item, ref mouseItem);
>>>>>>> Stashed changes

        int lastMouseXBak;
        int lastMouseYBak;
        int mouseXBak;
        int mouseYBak;
        int lastScreenWidthBak;
        int lastScreenHeightBak;
        void FixMouse(bool fix = true)
        {
<<<<<<< Updated upstream
            ItemSlot.LeftClick(ref Item);
            Utils.Swap<Item>(ref Item, ref mouseItem);
=======
            if (fix)
            {
                lastMouseXBak = Main.lastMouseX;
                lastMouseYBak = Main.lastMouseY;
                mouseXBak = Main.mouseX;
                mouseYBak = Main.mouseY;
                lastScreenWidthBak = Main.screenWidth;
                lastScreenHeightBak = Main.screenHeight;

                PlayerInput.SetZoom_Unscaled();
                PlayerInput.SetZoom_MouseInWorld();
            }
            else
            {
                Main.lastMouseX = lastMouseXBak;
                Main.lastMouseY = lastMouseYBak;
                Main.mouseX = mouseXBak;
                Main.mouseY = mouseYBak;
                Main.screenWidth = lastScreenWidthBak;
                Main.screenHeight = lastScreenHeightBak;
            }
>>>>>>> Stashed changes
        }
    }
}
