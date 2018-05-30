﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent.AchievementSlotElement
{
    class AchievementNameText : UIElement
    {
        private string text;
        public override void OnInitialize()
        {
            Width.Set(480, 0f);
            Height.Set(20f, 0f);
        }

        public void setText(String text)
        {
            this.text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDim = base.GetInnerDimensions();
            Vector2 textPos = new Vector2(innerDim.X, innerDim.Y);

            spriteBatch.DrawString(Main.fontItemStack, text, textPos, Color.White, 0f, innerDim.ToRectangle().Size(), 0.75f, SpriteEffects.None, 0);
        }
    }
}
