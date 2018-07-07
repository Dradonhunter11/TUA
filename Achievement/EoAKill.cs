using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.Achievement
{
    class EoAKill : ModAchievement
    {
        public EoAKill(Mod mod) : base(mod)
        {

        }

        public override void setDefault()
        {
            name = "Burn and rave at close of day";
            description = "Succesfully destroy the god of destruction, but it'S only the start...";

            condition = () => TUAWorld.EoADowned;
        }

        public override void reward(Player p)
        {
            
        }
    }
}
