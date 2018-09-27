using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Backgrounds.Meteoridon
{
    class MeteoridonBG : ModSurfaceBgStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            
        }

        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Meteoridon/Meteoridon_BG");
        }

        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && BiomeLibrary.BiomeLibs.InBiome("Meteoridon");
        }
    }
}
