using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent
{
    class AchivementUIList : UIList
    {
        public override void OnInitialize()
        {
            Width.Set(580, 0f);
            Height.Set(360, 0f);
            SetPadding(0);

            _scrollbar = new UIScrollbar();
            _scrollbar.Width.Set(10, 0f);
            _scrollbar.Height.Set(360, 0f);
            _scrollbar.Left.Set(580, 0f);
            _scrollbar.Top.Set(0, 0f);
            _scrollbar.SetPadding(0);
            Append(_scrollbar);

            ListPadding = 0;

            loadAchievement();
        }

        private void loadAchievement()
        {
            int index = 0;
            foreach (ModAchievement achivement in AchievementManager.GetInstance().getList())
            {
                AchievementSlot slot = new AchievementSlot();
                slot.setProperties(achivement, index);
                slot.Left.Set(0, 0f);

                Add(slot);
                index++;
            }
        }
    }
}