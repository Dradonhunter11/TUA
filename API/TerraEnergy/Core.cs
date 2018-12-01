using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TerrariaUltraApocalypse.API.TerraEnergy
{
    public class Core
    {
        protected int maxEnergy;
        protected int currentEnergy = 0;

        public Core(int maxEnergy)
        {
            this.maxEnergy = maxEnergy;
        }

        public void addEnergy(int energyToAdd)
        {
            currentEnergy += energyToAdd;
            if (currentEnergy >= maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }

        public virtual int consumeEnergy(int energyToRemove)
        {
            if (currentEnergy - energyToRemove < 0)
            {
                int leftover = currentEnergy;
                currentEnergy = 0;
                return leftover;
            }
            currentEnergy -= energyToRemove;
            return energyToRemove;
        }

        public int getCurrentEnergyLevel()
        {
            return currentEnergy;
        }

        public int getMaxEnergyLevel()
        {
            return maxEnergy;
        }

        public bool isFull()
        {
            return currentEnergy == maxEnergy;
        }

        public void setMaxEnergyLevel(int i)
        {
            maxEnergy = i;
        }
    }
}