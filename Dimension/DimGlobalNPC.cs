using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TUA.Dimension
{
    [Obsolete("Delete later")]
    public class DimGlobalNPC : GlobalNPC
    {
        private static List<int[]> npcList;

        public DimGlobalNPC()
        {
            npcList = new List<int[]>
            {
                DimensionUtil.vortexEnemy,
                DimensionUtil.solarEnemy,
                DimensionUtil.stardustEnemy,
                DimensionUtil.nebulaEnemy
            };
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (DimensionUtil.InSolar)
            {
                pool.Clear();
                pool.Add(npcList[1][0], 10f);
                pool.Add(npcList[1][1], 1f);
                pool.Add(npcList[1][2], 5f);
                pool.Add(npcList[1][3], 9f);
            }
        }

        public void RemoveNPC(IDictionary<int, float> pool,  int npcID)
        {
            if (pool.ContainsKey(npcID))
            {
                pool.Remove(npcID);
            }
        }
    }
}