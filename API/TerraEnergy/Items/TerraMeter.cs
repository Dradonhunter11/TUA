using TUA.API.TerraEnergy.EnergyAPI;

namespace TUA.API.TerraEnergy.Items
{
    class TerraMeter : EnergyItem
    {
        public override void SafeSetDefault(ref int maxEnergy)
        {
            maxEnergy = 100000;
            item.width = 30;
            item.height = 30;
            item.consumable = false;
            item.useStyle = 4;
        }
    }
}