using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Items
{
    class SolarCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar crystal");
            Tooltip.SetDefault("Allow you to travel in a new universe\nUltra mode");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useStyle = 4;
            item.useTime = 5;
            item.useAnimation = 20;
            base.SetDefaults();
        }


        public override bool UseItem(Player player)
        {
            if (!SubworldLibrary.Subworld.IsActive<SolarSubworld>())
            {
                TUA.BroadcastMessage("You are suddendly entering the solar realm, it's quite fiery here...", Color.OrangeRed);
                SubworldLibrary.Subworld.Enter<SolarSubworld>();
            }
            return true;
        }
    }
}
