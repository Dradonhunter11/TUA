using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent.AchievementSlotElement
{
    class AchievementDescriptionText : UIElement
    {
        private UIText title;
        private string text;
        public override void OnInitialize()
        {
            Width.Set(480, 0f);
            Height.Set(40, 0f);
        }

        public void setText(String text)
        {
            this.text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDim = base.GetInnerDimensions();
            Vector2 textPos = new Vector2(innerDim.X, innerDim.Y);
            
            spriteBatch.DrawString(Main.fontItemStack, text, textPos, Color.White, 0f, innerDim.ToRectangle().Size(), 1f, SpriteEffects.None, 0);
        }
    }
}
