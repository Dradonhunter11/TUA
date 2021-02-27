using Terraria.DataStructures;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.Interface;

namespace TUA.Items
{
    class RodOfLinking : ModItem
    {
        private int _storedEntityID;
        private Point16 currentStockedEntityLocation;

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.useStyle = 4;
            item.consumable = false;
        }

        public void SaveLinkableEntityLocation<T>(T entity) where T : TileEntity
        {
            _storedEntityID = entity.ID;
        }

        public TileEntity GetStoredEntityType()
        {
            return ModTileEntity.ByID[_storedEntityID];
        }

        public int GetEntity()
        {
            return _storedEntityID;
        }
    }
}
