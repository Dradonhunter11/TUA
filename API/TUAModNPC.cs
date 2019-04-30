using BiomeLibrary.API;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.Items.Meteoridon;
using TUA.NPCs.NewBiome.Wasteland.MutatedMass;


namespace TUA.API
{
    public abstract class TUAModNPC : ModNPC
    {
        public override ModNPC NewInstance(NPC npcClone)
        {
            if (TUAWorld.UltraMode)
                UltraScaleDifficulty(npcClone);

            return base.NewInstance(npcClone);
        }

        //This method is used to do NPC scaling in Ultra mode
        public virtual void UltraScaleDifficulty(NPC npc) { }

        public static void Awaken()
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.modNPC is HeartOfTheWasteland)
                {
                    HeartOfTheWasteland boss = npc.modNPC as HeartOfTheWasteland;
                    boss.IsSleeping = false;
                }
            }
        }

        protected void ApplyDamageCap(ref int damage, int damageCap)
        {
            if (damage >= damageCap)
            {
                damage = damageCap;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (npc.type == NPCID.Steampunker && mod.GetBiome("Meteoridon").InBiome())
            {
                shop.item[nextSlot] = mod.GetItem<BrownSolution>().item;
                nextSlot++;
            }
        }

        public override bool CloneNewInstances => false;
    }
}