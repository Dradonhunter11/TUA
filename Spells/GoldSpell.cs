﻿using Terraria.ID;
using Terraria;
using Terraria.Audio;

namespace TUA.Spells
{
    class GoldSpell : Spell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Shower");
            Tooltip.SetDefault("Make it rain gold!");
        }

        public override bool Cast(Player player)
        {
            for (int i = 0; i < Main.rand.Next(15); i++)
            {
                Item.NewItem((int)player.position.X,
                    (int)player.position.Y - Main.rand.Next(5),
                    player.width, player.height, Main.rand.Next(ItemID.SilverCoin,
                                                                ItemID.PlatinumCoin + 1)
                                                                );
            }
            return true;
        }

        public override void SafeSetDefaults()
        {
            item.useTime = 20;
            item.useAnimation = 60;
            item.UseSound = new LegacySoundStyle(SoundID.Coins, 0);
        }
    }
}
