using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.Achievement
{
    class YouMadeADoor : ModAchievement
    {
        public override void setDefault()
        {
            name = "Knock Knock. Who's there? THE DOOR!";
            description = "Heather made a functional door";
            rewardDesc = "[i:" + ItemID.WoodenDoor + "]";
            condition = () => true;
        }

        public override void reward(Player p)
        {
            Item.NewItem(p.Center, ItemID.WoodenDoor, 99, true, 0, true, false);
        }
    }
}
