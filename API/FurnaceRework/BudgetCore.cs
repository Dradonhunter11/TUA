using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUltraApocalypse.API.TerraEnergy;

namespace TerrariaUltraApocalypse.API.FurnaceRework
{
    class BudgetCore : Core
    {
        public BudgetCore(int maxEnergy) : base(maxEnergy)
        {
        }

        public override int consumeEnergy(int energyToRemove)
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
