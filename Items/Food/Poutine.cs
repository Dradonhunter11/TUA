using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using TUA.API;

namespace TUA.Items.Food
{
    class Poutine : TUAModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poutine");
            Tooltip.AddLine("A purely awesome meal from the Canadians");
            Tooltip.AddLine("Heal 500 HP");
            Tooltip.AddLine("Give potion sickness for 30 second");
            
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 20;
            item.rare = 99;
            item.lavaWet = true;
            item.consumable = true;
            item.maxStack = 30;
        }

        public override bool UseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness))
            {
                return false;
            }

            player.HealEffect(500, true);
            player.AddBuff(BuffID.PotionSickness, 1800, true);

            return true;
        }
    }
}