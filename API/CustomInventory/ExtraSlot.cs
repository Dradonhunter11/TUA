using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TUA.API.CustomInventory
{

    class ExtraSlot
    {
        public Item Item;


        public int ItemStack => Item.stack;
        public int ItemType => Item.type;

        public ExtraSlot()
        {
            Item = new Item();
            Item.TurnToAir();
        }

        public Item getItem(bool fullStack)
        {
            if (Item.IsAir)
            {
                return Item;
            }
            return Item.Clone();
        }
        

        public Item getItemAndRemove(bool fullStack)
        {
            Item tempItem;
            if (fullStack)
            {
                tempItem = Item;
                Item.TurnToAir();
                return tempItem;
            }

            tempItem = Item.Clone();
            tempItem.stack = 1;
            Item.stack -= 1;
            return tempItem;
        }

        public bool setItem(ref Item newItem)
        {
            if (!Item.IsAir)
            {
                return false;
            }
            swap(ref newItem);
            return true;
        }

        public bool manipulateItem(Item newItem, int i = 1)
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
                return;
            }
            Item.stack = preCalculate;
        }

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

        public void swap(ref Item mouseItem)
        {
            ItemSlot.LeftClick(ref Item);
            Utils.Swap<Item>(ref Item, ref mouseItem);
        }
    }
}
