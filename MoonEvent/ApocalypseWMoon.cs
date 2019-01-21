using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.MoonEvent
{
    class ApocalypseWMoon : ModWorld
    {
        private static Texture2D[] apocalypseMoon = new Texture2D[3];

        public override void PreUpdate()
        {
            apocalypseMoon[0] = mod.GetTexture("Texture/Moon/ApoMoon");
            apocalypseMoon[1] = mod.GetTexture("Texture/Moon/ApoMoon");
            apocalypseMoon[2] = mod.GetTexture("Texture/Moon/ApoMoon");
            if (TUAWorld.apocalypseMoon) {
                Main.moonTexture = apocalypseMoon;
            }
        }

    }
}
