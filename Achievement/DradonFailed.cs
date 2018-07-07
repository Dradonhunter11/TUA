using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.Achievement
{
    class DradonFailed : ModAchievement
    {
        public override void setDefault()
        {
            name = "Dradon failed to help";
            description = "Dradon failed to help Heather with the door";
            rewardDesc = "[i:" + ItemID.TrashCan + "]";
            condition = () => false;
        }

        public override void reward(Player p)
        {
            int i = Item.NewItem(p.Center, ItemID.TrashCan, 1, true, 0, true, false);
        }

        public DradonFailed(Mod mod) : base(mod)
        {
        }
    }
}