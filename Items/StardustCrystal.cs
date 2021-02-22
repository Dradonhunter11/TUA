using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Items
{
    class StardustCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust crystal");
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
            if (!SubworldLibrary.Subworld.IsActive<StardustSubworld>())
            {
                TUA.BroadcastMessage("You are entering into the cold environment of stardust...", Color.CornflowerBlue);
                SubworldLibrary.Subworld.Enter<SolarSubworld>();
            }
            return true;
        }
    }
}
