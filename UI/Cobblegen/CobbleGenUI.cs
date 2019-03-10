using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TUA.UI.Cobblegen
{
    class CobbleGenUI : UIState
    {
        private UIPanel panel;
        private UIImageButton exitButton;

        private static Texture2D background = TUA.instance.GetTexture("Texture/UI/Panel");

        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.BackgroundColor.A = 0;
            panel.BorderColor.A = 0;


            exitButton = new UIImageButton(TUA.instance.GetTexture("Texture/X_ui"));
            exitButton.Width.Set(20f, 0);
            exitButton.Height.Set(22f, 0);
            exitButton.HAlign = 1f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawPanel(spriteBatch, background, Color.White);
        }

        protected void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            CalculatedStyle dimensions = base.GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - CORNER_SIZE, point.Y + (int)dimensions.Height - CORNER_SIZE);
            int width = point2.X - point.X - CORNER_SIZE;
            int height = point2.Y - point.Y - CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, 0, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point2.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE + BAR_SIZE, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(0, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width, height), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE, BAR_SIZE, BAR_SIZE)), color);
        }
    }
}
