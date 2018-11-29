using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.LiquidAPI.Test
{
    class WeirdLiquid : ModLiquid
    {
        public override Texture2D texture
        {
            get { return ModLoader.GetMod("TerrariaUltraApocalypse").GetTexture("Texture/water/BestWater2"); }
        }

        public override void PreDrawValueSet(ref bool bg, ref int style, ref float Alpha)
        {
            style = 12;
            Alpha = 0.2f;
        }

        public override float SetLiquidOpacity()
        {
            return 1f;
        }

        public override void PlayerInteraction(Player target)
        {
            Main.NewText("Touched test liquid");
        }

        public override void ItemInteraction(Item target)
        {
            Main.NewText("Item in weird liquid : " + target.Name);
        }
    }
}
