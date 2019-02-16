using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.MoonEvent
{
    class ApocalypseMoon : API.EventManager.MoonEvent
    {
        public override List<int> scoreThresholdLimitPerWave => new List<int>()
        {
            50, 40, 150, 150, 200, 200, 200, 200, 20
        };

        public override string EventName => "Apocalypse Moon";
        public override int MaxWave => 8;
        public override void Initialize()
        {
            AddEnemy(NPCID.TheDestroyer, 0.3f, 5);
            AddEnemy(NPCID.SkeletronPrime, 0.2f, 10);
            AddEnemy(NPCID.Retinazer, 0.4f, 5);
            AddEnemy(NPCID.Spazmatism, 0.1f, 15);
            nextWave++;

            AddEnemy(NPCID.Plantera, 0.3f, 10);
            AddEnemy(NPCID.DukeFishron, 0.1f, 30);
            AddEnemy(NPCID.Golem, 0.6f, 5);
            nextWave++;

            AddEnemy(NPCID.Pumpking, 0.5f, 10);
            AddEnemy(NPCID.MourningWood, 0.2f, 5);
            AddEnemy(NPCID.HeadlessHorseman, 0.3f, 5);
            nextWave++;

            AddEnemy(NPCID.IceQueen, 0.5f, 10);
            AddEnemy(NPCID.SantaNK1, 0.2f, 5);
            AddEnemy(NPCID.Everscream, 0.3f, 5);
            nextWave++;
            
            AddEnemy(NPCID.LunarTowerSolar, 0.05f, 20); //5%
            AddEnemy(NPCID.SolarDrakomireRider, 0.03f, 5); //35%
            AddEnemy(NPCID.SolarCorite, 0.1f, 5); //45%
            AddEnemy(NPCID.SolarCrawltipedeHead, 0.05f, 10); //50%
            AddEnemy(NPCID.SolarDrakomire, 0.1f, 5); //60%
            AddEnemy(NPCID.SolarSolenian, 0.2f, 5);
            AddEnemy(NPCID.SolarSpearman, 0.15f, 5);
            AddEnemy(NPCID.SolarSroller, 0.05f, 10);
            nextWave++;

            AddEnemy(NPCID.LunarTowerStardust, 0.05f, 20); //5%
            AddEnemy(NPCID.StardustCellBig, 0.25f, 5); //30%
            AddEnemy(NPCID.StardustCellSmall, 0, 1); //30%
            AddEnemy(NPCID.StardustJellyfishBig, 0.05f, 5); //35%
            AddEnemy(NPCID.StardustJellyfishSmall, 0.05f, 5); //40%
            AddEnemy(NPCID.StardustSoldier, 0.2f, 5); //60%
            AddEnemy(NPCID.StardustSpiderBig, 0.1f, 5); // 70%
            AddEnemy(NPCID.StardustSpiderSmall, 0.1f, 5); // 80%
            AddEnemy(NPCID.StardustWormHead, 0.2f, 5); //100%
            nextWave++;

            AddEnemy(NPCID.LunarTowerNebula, 0.05f, 20);
            AddEnemy(NPCID.NebulaSoldier, 0, 5);
            AddEnemy(NPCID.NebulaHeadcrab, 0, 5);
            AddEnemy(NPCID.NebulaBrain, 0, 5);
            AddEnemy(NPCID.NebulaBeast, 0, 5);
            nextWave++;
            
            //Add the rest of the nebula enemy

            AddEnemy(NPCID.LunarTowerVortex, 0.05f, 20);
            AddEnemy(NPCID.VortexHornet, 0, 5);
            AddEnemy(NPCID.VortexLarva, 0, 5);
            AddEnemy(NPCID.VortexRifleman, 0, 5);
            AddEnemy(NPCID.VortexSoldier, 0, 5);
            //Add the rest of the vortex enemy
        }

        public override void Message(int wave)
        {
            switch (wave)
            {
                case 0:
                    BaseUtility.Chat("The mechanical madness arises from the ground", Color.DarkSlateGray);
                    break;
                case 1:
                    BaseUtility.Chat("The mythical beast from the jungle and the legendary fish of the ocean are enraged", Color.Green);
                    break;
                case 2:
                    BaseUtility.Chat("The scary king and his minion decided to get out...", Color.Orange);
                    break;
                case 3:
                    BaseUtility.Chat("The chilling queen and her minion are freezing the atmosphere...", Color.LightBlue);
                    break;
                case 4:
                    BaseUtility.Chat("A breach has been observed in the sky, seem like it's solar invading the world", Color.Red);
                    break;
                case 5:
                    BaseUtility.Chat("A portal has quickly opened and close, seem like stardust summon are coming...", Color.Blue);
                    break;
                case 6: 
                    BaseUtility.Chat("Your mind feel overtaken by creature from another world, maybe they are from Nebula", Color.Violet);
                    break;
                case 7:
                    BaseUtility.Chat("The vortexian queen decided to send her minion from nebula into her, seem like they are invading", Color.Cyan);
                    break;
                case 8:
                    NPC.SpawnOnPlayer(Main.LocalPlayer.whoAmI, NPCID.MoonLordCore);
                    NPC.SpawnOnPlayer(Main.LocalPlayer.whoAmI, NPCID.MoonLordCore);
                    BaseUtility.Chat("The god of destruction has summoned moon lord illusion", Color.Black);
                    break;
            }
        }

        public override void OnDefeat()
        {
            BaseUtility.Chat("You proved yourself worthy of the god that once destroyed the world, the apocalypsio seem to have changed", Color.White);
        }
    }
}
