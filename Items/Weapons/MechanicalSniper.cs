using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Items.Weapons
{
    class MechanicalSniper : TUAModItem
    {
        private bool bonusHeld = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghoster");
            Tooltip.SetDefault("A sniper being able to shoot trough wall with special bullet\n100% crit chance when held after ultra mode at the cost of 50hp each shot");
        }

        public override void SetDefaults()
        {
            this.ultra = true;
            item.width = 74;
            item.height = 24;
            item.ranged = true;
            item.damage = 300;
            item.shoot = mod.ProjectileType("GhosterBullet");
            item.useStyle = 5;
            item.noMelee = true;
            item.rare = 12;
            item.useAnimation = 10;
            item.useTime = 80;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage,
            ref float knockBack)
        {
            
            if (player.statLife > 25)
            {
                player.statLife -= 50;
            }
            Projectile.NewProjectile(player.position.X, player.position.Y, speedX, speedY, mod.ProjectileType("GhosterBullet"), 500, knockBack, player.whoAmI);
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            Vector2 screenPosition = Main.screenPosition;
            Main.screenPosition.X += Main.zoomX * 10 + Main.mouseX;
            Main.screenPosition.Y += Main.zoomY * 10 + Main.mouseY + player.gravDir;

            
            
            
            if ((double)(Vector2.Distance(screenPosition, Main.screenPosition) - Main.player[Main.myPlayer].velocity.Length()) >= 0.25)
            {
                Main.screenPosition = Vector2.Lerp(screenPosition, Main.screenPosition, 0.5f);
            }

            return true;
        }


        public override void UpdateInventory(Player player)
        {
            if (player.HeldItem == item && !bonusHeld)
            {
                item.crit = 100;
                player.moveSpeed -= 3f;
                player.stepSpeed -= 3f;
                bonusHeld = true;
            } else if (player.HeldItem != item && bonusHeld)
            {
                item.crit = 50;
                player.moveSpeed += 3f;
                player.stepSpeed += 3f;
                bonusHeld = false;
            }
            if (player.HeldItem == item)
            {
                player.setSolar = true;
            }
        }
    }
}
