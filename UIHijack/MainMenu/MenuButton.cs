using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace TUA.UIHijack.MainMenu
{
    class MenuButton : UIElement
    {
        private string buttonName;
        private UITextPanel<LocalizedText> textPanel;
        protected int xPosition, yPosition, width, height;
        private Vector2 rowColumn;
        private bool list = false;
        private int listIndex = 0;

        public MenuButton(string text, int xPosition, int yPosition)
        {
            buttonName = text;

            Object[] array = { "UI.Play", text};
            Type t = typeof(LocalizedText);
            LocalizedText localizedText = (LocalizedText)typeof(LocalizedText).Assembly.CreateInstance(t.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic, null, array, null, null); ;
            textPanel = new UITextPanel<LocalizedText>(localizedText, 2.25f, false);
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            
        }

        public MenuButton setPosition(Vector2 rowColumn)
        {
            this.rowColumn = rowColumn;
            return this;
        }

        public MenuButton isList(int listIndex)
        {
            this.listIndex = listIndex;
            list = true;
            return this;
        }

        public override void OnInitialize()
        {
            Vector2 textSize = Main.fontMouseText.MeasureString(buttonName) * 1.5f;
            if (list)
            {
                Top.Set(250 + (45 * listIndex), 0);
                yPosition = (int)(250 + (45 * listIndex));
                Left.Set(Main.screenWidth / 2 - (textSize.X / 2), 0);
                xPosition = (int)(Main.screenWidth / 2 - (textSize.X / 2));
                Width.Set(textSize.X, 0);
                Height.Set(textSize.Y, 0);
                return;
            }
            Top.Set(yPosition, 0);
            Left.Set(xPosition, 0);
            Width.Set(textSize.X, 0);
            Height.Set(textSize.Y, 0);
            OnClick += ExecuteButton;
        }

        public virtual void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, buttonName, new Vector2(xPosition, yPosition), Color.Yellow, 0, Vector2.One, new Vector2(1.5f, 1.5f), -1, 1f);
                Main.PlaySound(SoundID.MenuTick);
                return;
            }
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, buttonName, new Vector2(xPosition, yPosition), Color.White, 0, Vector2.One, new Vector2(1.5f, 1.5f), -1, 1f);


            /*Texture2D bound2 = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            bound2.SetData<Color>(new Color[] { Color.White });

            Rectangle rec2 = GetInnerDimensions().ToRectangle();
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, rec2.Width, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X + rec2.Width - 1, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y + rec2.Height - 1, rec2.Width, 1), Color.Green);*/
        }

        public override void Recalculate()
        {
            if (!list)
            {
                Vector2 textSize = Main.fontMouseText.MeasureString(buttonName) * 1.5f;
                Top.Set(250 + (20 + 35 * rowColumn.Y), 0);
                yPosition = (int)(250 + (20 + 35 * rowColumn.Y));
                if (rowColumn.X == 1)
                {
                    Left.Set(Main.screenWidth / 2 - 250, 0);
                    xPosition = Main.screenWidth / 2 - 250;
                }
                else
                {
                    Left.Set(Main.screenWidth / 2 + 100, 0);
                    xPosition = Main.screenWidth / 2 + 100;
                }
                Width.Set(textSize.X, 0);
                Height.Set(textSize.Y, 0);
            }
            else
            {
                Vector2 textSize = Main.fontMouseText.MeasureString(buttonName) * 1.5f;
                Top.Set(250 + (45 * listIndex), 0);
                yPosition = (int)(250 + (45 * listIndex));
                Left.Set(Main.screenWidth / 2 - (textSize.X / 2), 0);
                xPosition = (int) (Main.screenWidth / 2 - (textSize.X/2));
                Width.Set(textSize.X, 0);
                Height.Set(textSize.Y, 0);
            }
            base.Recalculate();
        }

        protected void PostRecalculate()
        {
            base.Recalculate();
        }
    }
}
