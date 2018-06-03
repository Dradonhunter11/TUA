using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.Achievement
{
    class UltraEoC : ModAchievement
    {
        public override void setDefault()
        {
            name = "Ultra Eye of cthulhu";
            description = "Kill the Ultra Eye of Cthulhu 10 time";
            rewardDesc = "[i:" + ModLoader.GetMod("TerrariaUltraApocalypse").ItemType("Spawner") + "]";
            condition = () => (TUAWorld.EoCDeath >= 10);
        }

        public override void reward(Player p)
        {
            int i = Item.NewItem(p.Center, ModLoader.GetMod("TerrariaUltraApocalypse").ItemType("Spawner"), 1, true, 0, true, false);
        }
    }
}
