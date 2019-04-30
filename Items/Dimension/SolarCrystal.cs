using Terraria;
using TUA.API;

namespace TUA.Items.Dimension
{
    class SolarCrystal : TUADimItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar crystal");
            Tooltip.SetDefault("Allow you to travel in a new universe\nUltra mode");
        }

        public override void SetDefaults()
        {
            Ultra = true;
            item.width = 32;
            item.height = 32;
            item.useStyle = 4;
            item.useTime = 5;
            item.useAnimation = 20;
            dimensionName = "solar";
            dimensionMessage = "Welcome to solar, hope you can chill off here.";
            base.SetDefaults();
        }

        public override void UpdateInventory(Player player)
        {
            itemUseCooldown--;
            if (itemUseCooldown < 0)
            {
                itemUseCooldown = 0;
            }
        }
    }
}
