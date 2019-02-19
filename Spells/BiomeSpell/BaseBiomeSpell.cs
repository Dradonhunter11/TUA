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
    }
}
