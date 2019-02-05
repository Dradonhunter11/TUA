using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.NPCs.NewBiome.Wasteland.MutatedMass
{
    [AutoloadBossHead]
    class HeartOfTheWasteland : TUAModNPC
    {
        public bool SleepState { private get; set; }

        private static readonly string HEAD_PATH = "TUA/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head";

        public override string Texture {
            get { return "Terraria/NPC_" + 548; }
        }

        public override string BossHeadTexture {
            get { return "TUA/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head0"; }
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
            SleepState = true;

            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
        }


        public override void AI()
        {
            if (SleepState)
            {
                npc.dontTakeDamage = true;
                return;
            }
            /*
             * Can someone explain what this is for - Agrair
            npc.boss = true;
            npc.immortal = false;
            */
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.DistanceSQ(npc.position) < 22500) // 150 tiles
                {
                    if (!Main.dedServ) Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAway{Main.rand.Next(4)}", npc.GivenName),
                        new Color(66, 244, 116));
                    player.position = npc.position +=
                        new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(-15, 15));
                    var point = player.position.ToTileCoordinates();
                    if (!Main.dedServ && Main.tile[point.X, point.Y].nactive())
                        Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAwayStuck", npc.GivenName),
                            new Color(66, 244, 116));
                    // Item6 is magic mirror
                    if (!Main.dedServ) Main.PlaySound(SoundID.Item6, npc.position);
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void BossHeadSlot(ref int index)
        {
            if (SleepState)
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
