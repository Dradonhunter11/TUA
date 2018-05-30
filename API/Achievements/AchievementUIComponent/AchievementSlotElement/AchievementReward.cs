using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent.AchievementSlotElement
{
    class AchievementReward : UIElement
    {
        
        private string reward = "";

        private UIText rewardText;
        public override void OnInitialize()
        {
            Width.Set(480, 0f);
            Height.Set(20f, 0f);
            Append(rewardText);
        }

        public void setText(String text)
        {
            reward = text;

            rewardText = new UIText("Reward : " + text);
            rewardText.Width.Set(480, 0f);
            rewardText.Height.Set(20f, 0f);
        }
    }
}
