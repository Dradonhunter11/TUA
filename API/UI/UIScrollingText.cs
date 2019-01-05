using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace TerrariaUltraApocalypse.API.UI
{
    class UIScrollingText : UIElement
    {
        private int line = 0;
        private int lastLine = 0;
        private int wordLimit = 5;
        private Vector2 scaling;

        private String text;

        public void SetText(String text)
        {
            this.text = text;
        }

        public override void OnInitialize()
        {
            OnScrollWheel += onScroll;
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            String[] trimmedString = Utils.WordwrapString(text, Main.fontMouseText, 250, 999, out lastLine);
            StringBuilder builder = new StringBuilder();
            for (int i = line; i <  line + 9; i++)
            {
                if (i <= lastLine - 1)
                {
                    builder.AppendLine(trimmedString[i]);
                }
                else
                {
                    break;
                }
            }

            this.text = Regex.Replace(this.text, "^([^ ]+(?: [^ ]+){4}) ", "$1" + Environment.NewLine);

            CalculatedStyle style = GetInnerDimensions();
            /*Utils.DrawBorderStringBig(spriteBatch,  builder.ToString(), style.Position(),
                Color.White);*/
            ChatManager.DrawColorCodedString(spriteBatch, Main.fontMouseText, builder.ToString(), style.Position(), Color.White, 0f, Vector2.Zero, Vector2.One);
        }

        public void onScroll(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            if (evt.ScrollWheelValue == -120)
            {
                line += (line >= lastLine - 9) ? 0 : 1;
            }
            else
            {
                line -= (line == 0) ? 0 : 1;
            }
        }
    }
}
