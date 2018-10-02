using Dimlibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Dimension
{
    class DimGlobalNPC : GlobalNPC
    {

        private static List<int[]> npcList = new List<int[]>();

        public DimGlobalNPC()
        {
            npcList.Add(new int[] { NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier });
            npcList.Add(new int[] { NPCID.SolarCorite, NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider });
            npcList.Add(new int[] { NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustSpiderBig, NPCID.StardustWormHead });
            npcList.Add(new int[] { NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier });
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (Dimlibs.Dimlibs.getPlayerDim() == "Solar")
            {
                pool.Clear();
                pool.Add(npcList[1][0], 10f);
                pool.Add(npcList[1][1], 1f);
                pool.Add(npcList[1][2], 5f);
                pool.Add(npcList[1][3], 9f);

            }
        }

    }
}