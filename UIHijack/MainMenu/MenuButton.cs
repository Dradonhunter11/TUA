using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace TUA.UIHijack.MainMenu
{
    internal class MenuButton : UIElement
    {
        private readonly string buttonName;
        private readonly UITextPanel<LocalizedText> textPanel;
        protected int xPosition, yPosition, width, height;
        protected float min, max = 1.2f;
        protected float scale = 1f;
        private bool list = false;
        private int listIndex = 0;
        private bool tickSound = false;

        public string ButtomName => buttonName;

        public MenuButton(string text, int xPosition, int yPosition)
        {
            buttonName = text;
            SetPosition(new Vector2(xPosition, yPosition));
        }

        public void SetPosition(Vector2 position)
        {
            xPosition = (int)position.X;
            Left.Set(position.X, 0f);
            yPosition = (int)position.Y;
            Top.Set(position.Y, 0f);
        }

        public void SetChangingSize(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public override void OnInitialize()
        {
            Vector2 textSize = Main.fontDeathText.MeasureString(buttonName) * 1.5f;
            Top.Set(yPosition, 0);
            Left.Set(xPosition, 0);
            Width.Set(textSize.X, 0);
            Height.Set(textSize.Y, 0);
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 textSize = Main.fontDeathText.MeasureString(buttonName) * scale;
            Left.Set(400 - textSize.X / 2, 0f);
            Width.Set(textSize.X, 0f);
            Height.Set(textSize.Y, 0f);
            SetChangingSize(0.8f, 0.95f);
            if (IsMouseHovering && scale <= max)
            {
                scale += 0.01f;
                tickSound = true;
            }
            else if (!IsMouseHovering && scale >= min)
            {
                scale -= 0.01f;
                tickSound = false;
            }

            if (IsMouseHovering && !tickSound)
            {
                Main.PlaySound(SoundID.MenuTick);
            }
            Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            CalculatedStyle style = GetOuterDimensions();
            if (IsMouseHovering)
            {
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, buttonName, style.X,
                    style.Y, Color.Yellow, Color.Black, default(Vector2), scale);
            }
            else
            {
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, buttonName, style.X,
                    style.Y, Color.Gray, Color.Black, default(Vector2), scale);
            }

            /*
            Texture2D bound2 = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            bound2.SetData<Color>(new Color[] { Color.White });

            Rectangle rec2 = GetInnerDimensions().ToRectangle();
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, rec2.Width, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X + rec2.Width - 1, rec2.Y, 1, rec2.Height), Color.Green);
            spriteBatch.Draw(bound2, new Rectangle(rec2.X, rec2.Y + rec2.Height - 1, rec2.Width, 1), Color.Green);*/
        }
    }
}
