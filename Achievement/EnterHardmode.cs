using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.Achievement
{
    class EnterHardmode : ModAchievement
    {
        public override void setDefault()
        {
            name = "You like it hard?";
            description = "You entered in hardmode";
            condition = () => Main.hardMode;
        }

        public override void reward(Player p)
        {
            int i = Item.NewItem(p.Center, ItemID.AvengerEmblem, 1, true, 0, true, false);
            
        }
    }
}