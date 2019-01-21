using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.CustomScreenShader
{
    class SolarWorld : ModWorld
    {
        public static bool solarMist = false;
        public static int solarMistTimer = 0;

        public override void Load(TagCompound tag)
        {
            base.Load(tag);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("solarMist", solarMist);
            tag.Add("solarMistTimer", solarMistTimer);
            return base.Save();
        }

        public override void PostUpdate()
        {
            ScreenFog.Update(TerrariaUltraApocalypse.SolarFog);

            if (solarMistTimer <= 0)
            {
                solarMist = !solarMist;
                solarMistTimer = Main.rand.Next(3600 * 24, 3600 * 96);
            }

            solarMistTimer--;
        }

        public override void PostDrawTiles()
        {
            if (Dimlibs.Dimlibs.getPlayerDim() == "Solar" && solarMist && Main.netMode == 0)
            {
                ScreenFog.Draw(TerrariaUltraApocalypse.SolarFog, 0.3f, 0.1f);
            }
            
        }
    }
}
