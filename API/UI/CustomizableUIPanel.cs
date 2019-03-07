using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace TUA.API.UI
{
    class CustomizableUIPanel : UIElement
    {
        private static int CORNER_SIZE = 10;
        private static int BAR_SIZE = 4;
        private static Texture2D _backgroundTexture;

        public bool isVisible;
        public CustomizableUIPanel(Texture2D texture)
        {
            if (_backgroundTexture == null)
            {
                _backgroundTexture = texture;
            }
            base.SetPadding((float)CORNER_SIZE);
        }

        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            
            CalculatedStyle dimensions = base.GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y); //opposite corner
            Point point2 = new Point(point.X + (int)dimensions.Width - CORNER_SIZE, point.Y + (int)dimensions.Height - CORNER_SIZE);
            int width = point2.X - point.X - CORNER_SIZE;
            int height = point2.Y - point.Y - CORNER_SIZE;

            //Top part drawing
            spriteBatch.Draw(_backgroundTexture, new Vector2(point.X, point.Y), new Rectangle(0, 0, 10, 10), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, 10), new Rectangle(CORNER_SIZE + 3, 0, 2, 10), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Vector2(point.X + CORNER_SIZE + width, point.Y), new Rectangle(18, 0, 10, 10), Color.White);
            
            //Middle part drawing
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X, point.Y + CORNER_SIZE, 10, height), new Rectangle(0, CORNER_SIZE + 3, 10, 2), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width + 1, height), new Rectangle(13, 13, 2, 2), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE + width + 1, point.Y + CORNER_SIZE, 10, height), new Rectangle(19, CORNER_SIZE + 3, 10, 2), Color.White);

            //Bottom part drawing
            spriteBatch.Draw(_backgroundTexture, new Vector2(point.X, point.Y + height + CORNER_SIZE), new Rectangle(0, 18, 10, 10), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point.X + CORNER_SIZE, point.Y + height + CORNER_SIZE, width, 10), new Rectangle(CORNER_SIZE + 3, 18, 2, 10), Color.White);
            spriteBatch.Draw(_backgroundTexture, new Vector2(point.X + CORNER_SIZE + width, point.Y + height + CORNER_SIZE), new Rectangle(18, 18, 10, 10), Color.White);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                this.DrawPanel(spriteBatch, _backgroundTexture, Color.Gray);
            }
        }
    }
}
