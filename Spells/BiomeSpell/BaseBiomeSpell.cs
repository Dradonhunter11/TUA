using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using TUA.API;

namespace TUA.Spells.BiomeSpell
{
    abstract class BaseBiomeSpell : TUAModLegacyItem, ISpell
    {
        public abstract bool Cast(Player player);

        public virtual bool GetColor(out Color color)
        {
            color = default(Color);
            return false;
        }

        public sealed override bool UseItem(Player player) => Cast(player);

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage,
            ref float knockBack)
        {
            BaseBiomeSpellProjectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY),
                item.shoot, damage, knockBack, player.whoAmI).modProjectile as BaseBiomeSpellProjectile;
            GetColor(out proj.color);
            return true;
        }
    }
}
