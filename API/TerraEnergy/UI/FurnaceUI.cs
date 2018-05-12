using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock;

namespace TerrariaUltraApocalypse.API.TerraEnergy.UI
{
    class FurnaceUI : UIState
    {
        public UIPanel furnaceUI;
        public static bool visible = false;

        private FurnaceItemSlot input = new FurnaceItemSlot("input", false);

        private FurnaceItemSlot output = new FurnaceItemSlot("output", true);

        UIText energyCounter = new UIText("");

        private TerraFurnaceEntity currentFurnace;

        public override void OnInitialize()
        {
            furnaceUI = new UIPanel();
            furnaceUI.SetPadding(0);
            furnaceUI.Width.Set(400, 0f);
            furnaceUI.Height.Set(200, 0f);
            furnaceUI.Top.Set(Main.screenHeight / 2, 0f);
            furnaceUI.Left.Set(Main.screenWidth / 2, 0f);
            
            furnaceUI.BackgroundColor = new Color(73, 94, 171);

            input.Width.Set(32, 0f);
            input.Height.Set(32, 0f);
            input.Top.Set(30, 0f);
            input.Left.Set(300, 0f);
            input.OnClick += inputSlotItem;
            furnaceUI.Append(input);

            output.Width.Set(32, 0f);
            output.Height.Set(32, 0f);
            output.Top.Set(120, 0f);
            output.Left.Set(300, 0f);
            output.OnClick += withdrawItem;
            furnaceUI.Append(output);

            Texture2D buttonDeleteTexture = ModLoader.GetTexture("Terraria/UI/ButtonDelete");
            UIImageButton closeButton = new UIImageButton(buttonDeleteTexture);
            closeButton.Left.Set(400 - 35, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            furnaceUI.Append(closeButton);

            energyCounter = new UIText("");
            energyCounter.Left.Set(10, 0f);
            energyCounter.Top.Set(5, 0f);
            energyCounter.Width.Set(100, 0f);
            furnaceUI.Append(energyCounter);

            Append(furnaceUI);
        }

        public void receiveFurnaceEntity(TerraFurnaceEntity entity) {
            currentFurnace = entity;
            input.receiveFurnaceEntity(entity);
            output.receiveFurnaceEntity(entity);
        }

        private void inputSlotItem(UIMouseEvent evt, UIElement listeningElement) {
            Item i = Main.mouseItem;

            if (i.Name != "") {
                Main.mouseItem = null;
                input.receiveEntityItem(i);
                return;
            }

            if (i.Name == "" && input.currentItem() != null) {
                Main.mouseItem = input.currentItem();
                input.receiveEntityItem(null);
            }
        }

        private void withdrawItem(UIMouseEvent evt, UIElement listeningElement)
        {
            if (currentFurnace.inventory[1] != null) {
                Main.mouseItem = currentFurnace.inventory[1];
                currentFurnace.inventory[1] = null;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            energyCounter.SetText(currentFurnace.getEnergy().getCurrentEnergyLevel() + " / " + currentFurnace.getMaxEnergyStored() + " TE" + "\nTerra Furnace");
            input.receiveEntityItem(currentFurnace.inventory[0]);
            output.receiveEntityItem(currentFurnace.inventory[1]);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            clearContent();
            visible = false;
        }

        public void clearContent() {
            currentFurnace = null;
            input.UIClosing();
            output.UIClosing();
        }

        class FurnaceItemSlot : UIItemSlot
        {
            private readonly String type;
            private TerraFurnaceEntity currentFurnace;

            public FurnaceItemSlot(String type, bool locked) : base(locked) {
                this.type = type;
            }

            public void receiveFurnaceEntity(TerraFurnaceEntity terraFurnaceEntity) {
                currentFurnace = terraFurnaceEntity;
            }


            public override void sendItemToTileEntity()
            {
                if (currentItemInSlot != null)
                {
                    currentFurnace.inventory[0] = currentItemInSlot;
                }
                Main.NewText(type);
            }

            public override void sync()
            {
                if (type == "input") {
                    currentItemInSlot = currentFurnace.inventory[0];
                } else
                {
                    currentItemInSlot = currentFurnace.inventory[1];
                }
            }
        }
    }

    abstract class UIItemSlot : UIElement
    {
        protected Item currentItemInSlot = new Item();
        private bool update = false;
        private bool locked;

        public abstract void sendItemToTileEntity();
        public abstract void sync();

        public UIItemSlot(bool locked) {
            this.locked = locked;
        }

        public Item currentItem() {
            return currentItemInSlot;
        }

        public UIItemSlot() {
            Width.Set(64, 0f);
            Height.Set(64, 0f);
        }

        public override void Click(UIMouseEvent evt)
        {
            Main.NewText(Main.mouseItem.Name);
            if (!locked && currentItemInSlot.Name == "" && Main.mouseItem.Name != "") {
                currentItemInSlot = Main.mouseItem;
                Main.mouseItem = new Item();
            } else

            if (currentItemInSlot.Name != "" && Main.mouseItem.Name == "") {
                Main.mouseItem = currentItemInSlot;
                currentItemInSlot = new Item();
            }
            update = true;
            sendItemToTileEntity();
        }

        public override void Update(GameTime gameTime)
        {
            if (update) {
                sync();
                update = false;
            }
        }

        public void receiveEntityItem(Item i) {
            currentItemInSlot = i;
            update = true;
        }

        

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 5f);
            spriteBatch.Draw(Main.inventoryBackTexture, drawPos, null, new Color(73, 94, 171), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
            if (currentItemInSlot.Name != "")
            {
                spriteBatch.Draw(Main.itemTexture[currentItemInSlot.type], drawPos, new Rectangle(0,0,32,32), new Color(73, 94, 171), 0f, new Vector2(-5, -5), 1f, SpriteEffects.None, 0f);
            }
            
        }

        public void UIClosing() {
            if (currentItemInSlot.Name != "") {
                sendItemToTileEntity();
            } 
            currentItemInSlot = new Item();
        }
    }
}
