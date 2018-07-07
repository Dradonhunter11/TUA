using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;
using TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent.AchievementSlotElement;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent
{
    class AchievementSlot : UIElement
    {
        private string name;
        private string description;
        private bool completed;
        private int index;

        private AchievementNameText achievementName;
        private AchievementDescriptionText achievementDescription;
        private AchievementReward achievementReward;
        
        private UIText achievementCompleted;

        private AchievementPic picture;

        private UIPanel panel;

        public override void OnInitialize()
        {
            
            panel.Width.Set(575, 0f);
            panel.Height.Set(87, 0f);

            picture.Left.Set(5, 0f);
            picture.Top.Set(9, 0f);

            Width.Set(575, 0f);
            Height.Set(90, 0f);

            achievementName = new AchievementNameText();
            achievementName.setText(name);
            achievementName.Top.Set(8f, 0f);
            achievementName.Left.Set(425f, 0f);

            achievementDescription = new AchievementDescriptionText();
            achievementDescription.setText(description);
            achievementDescription.Top.Set(48f, 0f);
            achievementDescription.Left.Set(548f, 0f);

            panel.Append(achievementName);
            panel.Append(achievementDescription);
            panel.Append(picture);

            Append(panel);
        }

        internal void setProperties(ModAchievement achievement, int i)
        {
            picture = new AchievementPic();
            panel = new UIPanel();
            name = achievement.name;
            description = achievement.description;
            if (achievement.texture != null)
            {
                picture.setTexture(achievement.texture);
            }

           

            //completed = p.checkAchievement(achievement.name);
            index = i;
            if (achievement.rewardDesc != "")
            {
                achievementReward = new AchievementReward();
                achievementReward.setText(achievement.rewardDesc);
                achievementReward.Top.Set(45f, 0f);
                achievementReward.Left.Set(-124f, 0f);
                panel.Append(achievementReward);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }






        /*protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDim = base.GetInnerDimensions();
            Vector2 picturePos = new Vector2(innerDim.X + 5f, innerDim.Y + 5);
            Rectangle r = picture.Bounds;
            picture = ModLoader.GetMod("TerrariaUltraApocalypse").GetTexture("Achievement/Default");

            spriteBatch.Draw(picture, new Vector2(innerDim.X + 66f, innerDim.Y + innerDim.Height * index), r, Color.White, 0f, r.Size(), 1f, SpriteEffects.None, 0f);
        }*/
    }
}
