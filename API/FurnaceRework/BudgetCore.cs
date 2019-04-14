using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API.TerraEnergy;

namespace TUA.API.FurnaceRework
{
    class BudgetCore : Core
    {
        public BudgetCore(int maxEnergy) : base(maxEnergy)
        {
        }

        public override int ConsumeEnergy(int energyToRemove)
        {
            if (currentEnergy < 0)
            {
                return 0;
            }
            currentEnergy--;
            return 1;
        }
    }
}
