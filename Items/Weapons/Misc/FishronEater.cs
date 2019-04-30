using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUA.API;
using TUA.Projectiles.Misc.FishronEater;

namespace TUA.Items.Weapons.Misc
{
    class FishronEater : TUAModLegacyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishron Eater");
            Tooltip.AddLine("The duke is coming!");
        }

        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 74;
            item.scale = 0.75f;
            item.useStyle = 5;
            item.damage = 150;
            item.useAnimation = 45;
            item.shoot = mod.ProjectileType("TheDuke");
            item.useTime = 45;
            item.noMelee = true;
            item.ranged = true;
            item.shootSpeed = 12;
            Ultra = true;
        }
    }
}
