using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TerrariaUltraApocalypse;
using Microsoft.Xna.Framework;
using BiomeLibrary;
using TerrariaUltraApocalypse.Items.Misc.Spawner;
using TerrariaUltraApocalypse.NPCs.NewBiome.Meteoridon;
using TerrariaUltraApocalypse.NPCs.UltraBoss.UltraEoC;

namespace TerrariaUltraApocalypse.NPCs
{
    class TUAGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu && NPC.downedMoonlord && !TUAWorld.EoCPostML)
            {
                npc.position.X = npc.position.X + (npc.width / 2);
                npc.position.Y = npc.position.Y + (npc.height / 2);
                npc.width = 100;
                npc.height = 100;
                npc.position.X = npc.position.X - (npc.width / 2);
                npc.position.Y = npc.position.Y - (npc.height / 2);
                Vector2 spawnAt = npc.Center + new Vector2(0f, npc.height / 2f);
                Main.NewText("You thought that was all I had?", Color.Red);
                //Insert French Here...
                TUAWorld.EoCPostML = true;
                NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType<UltraEoC>());
            }
        }
    }
}
