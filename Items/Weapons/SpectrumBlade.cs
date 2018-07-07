using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Items.Weapons
{
    class SpectrumBlade : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 56;
            item.height = 62;
            item.melee = true;
            item.damage = 99999;
            item.useTime = 5;
            item.useStyle = 1;
        }



        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor,
            Vector2 origin, float scale)
        {
            item.color.R = (byte)Main.DiscoR;
            item.color.G = (byte)Main.DiscoG;
            item.color.B = (byte)Main.DiscoB;
            item.color.A = 255;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            item.color.R = (byte)Main.DiscoR;
            item.color.G = (byte)Main.DiscoG;
            item.color.B = (byte)Main.DiscoB;
            item.color.A = 255;
        }
    }
}
