using Terraria;
using Terraria.World.Generation;

namespace TUA.API.GenActionModifiers
{
    class IntegretyFilter : GenCondition
    {
        private int integretyPercent;

        public IntegretyFilter(int percent)
        {
            integretyPercent = percent;
        }


        protected override bool CheckValidity(int x, int y)
        {
            return WorldGen.genRand.Next(100) < integretyPercent;
        }
    }
}
