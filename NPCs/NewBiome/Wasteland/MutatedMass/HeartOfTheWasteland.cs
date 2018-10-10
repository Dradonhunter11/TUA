using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.NPCs.NewBiome.Wasteland.MutatedMass
{
    [AutoloadBossHead]
    class HeartOfTheWasteland : TUAModNPC
    {
        private bool isSleeping;

        private static readonly string HEAD_PATH = "TerrariaUltraApocalypse/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head";

        public override string Texture {
            get { return "Terraria/NPC_" + 548; }
        }

        public override string BossHeadTexture {
            get { return "TerrariaUltraApocalypse/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head0"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the wasteland");
            Main.npcFrameCount[npc.type] = 1;
        }


        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 32;
            npc.lifeMax = 9000;
            npc.damage = 60;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 20, 50, 25);
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.boss = true;
            npc.immortal = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            isSleeping = true;
        }


        public override void AI()
        {

            if (isSleeping)
            {
                npc.dontTakeDamage = true;
                return;
            }
            npc.boss = true;
            npc.immortal = false;

        }

        public override bool CheckActive()
        {
            return false;
        }

        public void setSleepState(bool sleepState)
        {
            isSleeping = sleepState;
        }

        public override void BossHeadSlot(ref int index)
        {
            if (isSleeping)
            {
                index = NPCHeadLoader.GetBossHeadSlot(HEAD_PATH + "0");
            }
            else
            {
                index = NPCHeadLoader.GetBossHeadSlot(HEAD_PATH + "1");
            }
        }


    }
}
