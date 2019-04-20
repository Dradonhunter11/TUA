using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using System.IO;
using TUA.API;

namespace TUA.Items.Weapons
{
    class MechanicalBow : TUAModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(GameCulture.French, "L'arc des dieux apocalyptic");
            DisplayName.SetDefault("The god feather");
            Tooltip.AddLine("Forged from the rest of an ancient corrupted god...");
            Tooltip.AddLine("Won't consume arrows");
            Tooltip.AddTranslation(GameCulture.French, "Forger a partir des restes d'un ancien dieu corrompu...\nNe consumme pas de flèche");
            Tooltip.SetDefault(AddUltraTooltip(Tooltip.GetDefault()));
            Tooltip.AddTranslation(GameCulture.French, AddUltraTooltip(Tooltip.GetTranslation(GameCulture.French)));
        }

        

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 50;
            item.noMelee = true;
            item.rare = -12;
            item.damage = 150;
            item.ranged = true;
            item.useStyle = 5;
            item.shoot = mod.ProjectileType("GodExplosiveArrow");
            item.autoReuse = true;
            item.shootSpeed = 12;
            item.knockBack = 10;
            item.useAnimation = 10;
            item.useTime = 20;
            item.useAmmo = AmmoID.Arrow;
            item.crit = 50;
        }

        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.AddIngredient(ItemID.DirtBlock, 1);
            r.SetResult(this);
            r.AddRecipe();
            base.AddRecipes();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("GodExplosiveArrow"), damage, knockBack, Main.myPlayer);
            item.shootSpeed = 7;
            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
    }
}
