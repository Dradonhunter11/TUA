using Terraria.DataStructures;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.EnergyAPI;

namespace TUA.Items
{
    class RodOfLinking : ModItem
    {
        private Point16 currentStockedEntityLocation;
        private StorageEntity linkedEntity;
        private int xLoc;
        private int yLoc;

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.useStyle = 4;
            item.consumable = false;
        }

        public void saveCollectorLocation(StorageEntity entity)
        {
            this.linkedEntity = entity;
        }

        public void saveCollectorLocation(Point16 location)
        {
            currentStockedEntityLocation = location;
        }

        public void saveCollectorLocation(int x, int y)
        {
            xLoc = x;
            yLoc = y;
        }

        public Point16 getCollectorLocation()
        {
            return currentStockedEntityLocation;
        }

        public StorageEntity getEntity()
        {
            return linkedEntity;
        }

        public int getXLoc()
        {
            return xLoc;
        }

        public int getYLoc()
        {
            return yLoc;
        }

    }
}
