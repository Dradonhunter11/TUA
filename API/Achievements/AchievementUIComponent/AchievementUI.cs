using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent
{
    class AchievementUI : UIState
    {
        private UIPanel panel;
        private UIPanel title;
        private AchivementUIList list;
        public static bool visible = false;
        private int alphaBg = 125;
        private int alphaBorder = 200;


        public override void OnInitialize()
        {
            List<AchievementSlot> achievementSlots = new List<AchievementSlot>();
            panel = new UIPanel();
            panel.Width.Set(600, 0f);
            panel.Height.Set(400, 0f);
            panel.Top.Set(Main.screenHeight/2 - 200, 0f);
            panel.Left.Set(Main.screenWidth/2 - 300, 0f);
            panel.SetPadding(0);
            
            
            list = new AchivementUIList();

            list.Left.Set(5f, 0f);
            list.Top.Set(30, 0f);

            panel.Append(list);

            title = new UIPanel();
            title.Width.Set(200, 0f);
            title.Height.Set(50, 0f);
            title.Left.Set(Main.screenWidth / 2 - 100, 0f);
            title.Top.Set(Main.screenHeight / 2 - 225, 0f);
            title.BackgroundColor = Color.DarkBlue;

            UIText text = new UIText("Modded Achievement");
            text.Height.Set(25, 0);

            title.Append(text);

            Append(panel);
            Append(title);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
