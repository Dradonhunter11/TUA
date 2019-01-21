using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TUA.API.CustomInventory
{

    class ExtraSlot
    {
        protected Item currentItem;

        public ExtraSlot()
        {
            currentItem = new Item();
            currentItem.TurnToAir();
        }

        public Item getItem(bool fullStack)
        {
            if (currentItem.IsAir)
            {
                return currentItem;
            }
            return currentItem.Clone();
        }
        

        public Item getItemAndRemove(bool fullStack)
        {
            Item tempItem;
            if (fullStack)
            {
                tempItem = currentItem;
                currentItem.TurnToAir();
                return tempItem;
            }

            tempItem = currentItem.Clone();
            tempItem.stack = 1;
            currentItem.stack -= 1;
            return tempItem;
        }

        public bool setItem(ref Item newItem)
        {
            if (!currentItem.IsAir)
            {
                return false;
            }
            swap(ref newItem);
            return true;
        }

        public bool manipulateCurrentItem(Item newItem, int i = 1)
        {
            if (currentItem.type == newItem.type)
            {
                int calculateAfterStack = currentItem.stack + newItem.stack;
                if (calculateAfterStack > currentItem.maxStack)
                {
                    int calculateItemToSubstract = calculateAfterStack - currentItem.stack;
                    currentItem.stack += calculateItemToSubstract;
                    newItem.stack -= calculateItemToSubstract;
                }
            }
            return false;
        }

        public void manipulateCurrentStack(int number)
        {
            if (currentItem.stack <= 0)
            {
                currentItem.TurnToAir();
                return;
            }

            currentItem.stack += number;
        }

        public bool isEmpty()
        {
            return currentItem.IsAir;
        }

        public Texture2D getItemTexture()
        {
            return Main.itemTexture[currentItem.type];
        }

        public void swap(ref Item mouseItem)
        {
            Utils.Swap<Item>(ref currentItem, ref mouseItem);
        }

    }
}
