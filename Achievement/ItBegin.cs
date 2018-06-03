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
    class ItBegin : ModAchievement
    {
        public override void setDefault()
        {
            name = "It's only the beginning...";
            description = "Defeat the Moon Lord  and trigger Ultra Mode";
            rewardDesc = "[i:" + ModLoader.GetMod("TerrariaUltraApocalypse").ItemType("SolarCrystal") + "]";

            condition = () => NPC.downedMoonlord;
        }

        public override void reward(Player p)
        {
            int i = Item.NewItem(p.Center, ModLoader.GetMod("TerrariaUltraApocalypse").ItemType("SolarCrystal"), 1, true, 0, true, false);
        }
    }
}