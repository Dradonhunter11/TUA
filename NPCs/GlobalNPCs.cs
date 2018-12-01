using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.NPCs
{
    class GlobalNPCs : GlobalNPC
    {   
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
                TUAWorld.EoCPostML = true;
                NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType<UEoC>());
            }
        }
    }

    class UEoC : TUAModNPC
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.EyeofCthulhu; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Eye of Cthulhu");
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.EyeofCthulhu);
            npc.color = Color.LightPink;
            npc.damage = 80 * (int)(1 + TUAWorld.EoCDeath * 1.5);
            npc.defense = 100 * (int)(1 + TUAWorld.EoCDeath * 1.05);
            npc.lifeMax = 12500 * (int)(1 + TUAWorld.EoCDeath * 2);
        }
    }
}