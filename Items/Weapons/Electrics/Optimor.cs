using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TerrariaUltraApocalypse.API;
using TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI;

namespace TerrariaUltraApocalypse.Items.Weapons.Electrics
{
    class Optimor : EnergyWeapon
    {
        public int chargeTime = 0;
        public const int maxChargeTime = 300;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Optimor");
            Tooltip.SetDefault("Just imagine a BFG!");
            Tooltip.AddLine("Also, not for kids");
        }

        public override void SafeSetDefault(ref int maxEnergy)
        {
            maxEnergy = 20000;
            EnergyConsumedPerShot = 2000;
            item.width = 94;
            item.height = 40;
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.damage = 1000;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage,
            ref float knockBack)
        {
            if (chargeTime == maxChargeTime)
            {
                chargeTime = 0;
                energy -= EnergyConsumedPerShot;
                return true;
            }
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Main.mouseRight)
            {
                chargeTime++;
                return;
            }

            chargeTime = 0;
        }
    }
}
