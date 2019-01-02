using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.Projectiles.Solutions;

namespace TerrariaUltraApocalypse.Items.Meteoridon
{
    class BrownSolution : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brown Solution");
            Tooltip.SetDefault("Used by the Clentaminator"
                               + "\nSpreads the meteoridon");
        }

        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType<MeteoridonSolution>() - ProjectileID.PureSpray;
            item.ammo = AmmoID.Solution;
            item.width = 10;
            item.height = 12;
            item.value = Item.buyPrice(0, 0, 25, 0);
            item.rare = 3;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
