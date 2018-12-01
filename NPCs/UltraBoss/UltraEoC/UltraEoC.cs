using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.NPCs.UltraBoss.UltraEoC
{
    class UltraEoC : TUAModNPC
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.EyeofCthulhu; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Eye of Cthulhu");
            DisplayName.AddTranslation(GameCulture.French, "Hyper Oeuil de cthulhu");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.EyeofCthulhu);
            npc.color = Color.LightPink;
            npc.damage = 80 * (int)(1 + TUAWorld.EoCDeath * 1.5);
            npc.defense = 100 * (int)(1 + TUAWorld.EoCDeath * 1.05);
            npc.lifeMax = 12500 * (1 + TUAWorld.EoCDeath * 2);
        }
    }
}
