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
            description = "You made a door (that is not functionnal...)";
            rewardDesc = "[i:" + ItemID.WoodenDoor + "]";
            condition = () => false;
        }

        public override void reward(Player p)
        {
            
        }
    }
}
