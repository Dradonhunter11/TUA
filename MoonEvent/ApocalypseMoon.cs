using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.MoonEvent
{
    class ApocalypseMoon : GlobalNPC 
    {
        public static int wave = 0;

        private static List<int[]> npcList = new List<int[]>();
        private static bool initialMessage = false;

        public ApocalypseMoon() {
            npcList.Add(new int[] {NPCID.TheDestroyer, NPCID.Retinazer, NPCID.Spazmatism, NPCID.SkeletronPrime });
            npcList.Add(new int[] { NPCID.DukeFishron, NPCID.Plantera, NPCID.Golem });
            npcList.Add(new int[] { NPCID.Pumpking, NPCID.MourningWood });
            npcList.Add(new int[] { NPCID.IceQueen, NPCID.SantaNK1, NPCID.Everscream });
            npcList.Add(new int[] { NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier});
            npcList.Add(new int[] { NPCID.SolarCorite, NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider});
            npcList.Add(new int[] { NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustSpiderBig, NPCID.StardustWormHead });
            npcList.Add(new int[] { NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier });
            npcList.Add(new int[] { NPCID.MoonLordCore });
            
        }

        public override bool CheckDead(NPC npc)
        {
            if (TUAWorld.apocalypseMoon)
            {
                if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism || npc.type == NPCID.Skeleton || npc.type == NPCID.DukeFishron || npc.type == NPCID.Plantera || npc.type == NPCID.Golem || npc.type == NPCID.SantaNK1 || npc.type == NPCID.IceQueen || npc.type == NPCID.Everscream || npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking || npc.type == NPCID.VortexHornet || npc.type == NPCID.VortexHornetQueen || npc.type == NPCID.VortexLarva || npc.type == NPCID.VortexRifleman || npc.type == NPCID.VortexSoldier || npc.type == NPCID.SolarCorite || npc.type == NPCID.SolarCrawltipedeHead || npc.type == NPCID.SolarDrakomire || npc.type == NPCID.SolarDrakomireRider || npc.type == NPCID.StardustCellSmall || npc.type == NPCID.StardustJellyfishBig || npc.type == NPCID.StardustSoldier || npc.type == NPCID.StardustSpiderBig || npc.type == NPCID.StardustWormHead || npc.type == NPCID.NebulaBeast || npc.type == NPCID.NebulaBrain || npc.type == NPCID.NebulaHeadcrab || npc.type == NPCID.NebulaSoldier || npc.type == NPCID.MoonLordCore)
                {
                    TUAWorld.apocalypseMoonPoint += 5;
                    return true;
                }
            }
            return true;
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (TUAWorld.apocalypseMoon) {
                pool.Clear();
                editPool(pool);
                checkWave();
            }
            base.EditSpawnPool(pool, spawnInfo);
        }

        private void editPool(IDictionary<int, float> pool) {
            for (int i = 0; i < npcList[wave].Length; i++) {
                pool.Add(npcList[wave][i], 10f);
            } 
        }

        private void checkWave()
        {
            if(!initialMessage)
            {
                Main.NewText("The mechanical are raising against you...", Microsoft.Xna.Framework.Color.Purple);
                initialMessage = true;
                return;
            }
            if (wave == 0 && TUAWorld.apocalypseMoonPoint >= 50)
            {
                Main.NewText("The hidden beast are coming out!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 1 && TUAWorld.apocalypseMoonPoint >= 20)
            {
                Main.NewText("The spooky boss are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 2 && TUAWorld.apocalypseMoonPoint >= 60)
            {
                Main.NewText("The festive boss are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 3 && TUAWorld.apocalypseMoonPoint >= 60)
            {
                Main.NewText("The Vortex creature are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 4 && TUAWorld.apocalypseMoonPoint >= 600)
            {
                Main.NewText("The Solar creature are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 5 && TUAWorld.apocalypseMoonPoint >= 600)
            {
                Main.NewText("The stradust creature are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 6 && TUAWorld.apocalypseMoonPoint >= 600)
            {
                Main.NewText("The nebula creature are invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 7 && TUAWorld.apocalypseMoonPoint >= 600)
            {
                Main.NewText("The moon lord is invading the world!", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
            else if (wave == 8 && TUAWorld.apocalypseMoonPoint >= 15)
            {
                Main.NewText("<Eye of the apocalypse> : WHO SUMMONED ME, I AM ONE OF THE GOD THAT CREATED THIS WORLD AND THAT WILL DESTROY IT", Microsoft.Xna.Framework.Color.Purple);
                wave++;
                TUAWorld.apocalypseMoonPoint = 0;
                return;
            }
        }
    }
}
