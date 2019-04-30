using Terraria;
using Terraria.ModLoader;

namespace TUA.Backgrounds.Solar
{
    class SolarUnderBG : ModUgBgStyle
    {
        public override bool ChooseBgStyle()
        {

            if (Dimlibs.Dimlibs.getPlayerDim() == "solar" && Main.LocalPlayer.Center.Y > Main.rockLayer)
            {
                //Main.spriteBatch.Draw(mod.GetTexture("Backgrounds/Solar/SolarUnderUG0"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(70, 0, 0) * 0.9f);
            }

            return Dimlibs.Dimlibs.getPlayerDim() == "solar";
        }

        public override void FillTextureArray(int[] textureSlots)
        {
            //textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            //textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            textureSlots[5] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            textureSlots[4] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            textureSlots[6] = mod.GetBackgroundSlot("Backgrounds/Solar/SolarUnderUG0");
            //Main.hellBackStyle;
        }
    }
}
