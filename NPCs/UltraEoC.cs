using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using BiomeLibrary;

namespace TUA.NPCs
{
    class UltraEoC : GlobalNPC
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
                NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType<UltraBoss.UltraEoC.UltraEoC>());
            }
        }
    }
}
