using BiomeLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.Items;
using TUA.Items.Meteoridon;
using TUA.NPCs;
using TUA.NPCs.NewBiome.Wasteland.MutatedMass;
using TUA.NPCs.UltraBoss.UltraEoC;

// TO DO: Move it to the global folder, also make a global folder if it's not a thing already
namespace TUA.API
{
    public abstract class TUAGlobalNPC : GlobalNPC
    {

        //This method is used to do NPC scaling in Ultra mode
        public void UltraScaleDifficulty(NPC npc)
        {

        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Steampunker && mod.GetBiome("Meteoridon").InBiome(Main.LocalPlayer))
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<BrownSolution>());
                nextSlot++;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu && NPC.downedMoonlord && !TUAWorld.EoCPostMLDowned)
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
                TUAWorld.EoCPostMLDowned = true;
                NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<UltraEoC>());
                return;
            }
        }

        public override bool CloneNewInstances => false;
    }
}