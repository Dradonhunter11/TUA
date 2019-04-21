using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Void
{
    class VoidRecipe : ModRecipe
    {

        private int _voidAffinityRequired = 100;

        public VoidRecipe(Mod mod) : base(mod)
        {
        }

        public void SetAmountOfRequiredVoidAffinity(int amount)
        {
            _voidAffinityRequired = amount;
        }

        public override bool RecipeAvailable()
        {
            return Main.LocalPlayer.GetModPlayer<VoidPlayer>().voidAffinity >= _voidAffinityRequired;
        }
        

    }
}
