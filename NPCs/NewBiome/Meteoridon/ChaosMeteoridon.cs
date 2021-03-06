﻿using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.NPCs.NewBiome.Meteoridon
{
    class ChaosMeteoridon : ModNPC
    {
        private List<Meteoride> aliveMinion = new List<Meteoride>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Meteoridon");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.height = 110;
            npc.width = 104;
            npc.lifeMax = 10000;
            npc.lavaImmune = true;
            npc.value = Item.buyPrice(0, 15, 0, 0);
            npc.damage = 40;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.knockBackResist = 100;
        }

        private void spawnClone()
        {
            for (int i = 0; i < 6; i++)
            {
                aliveMinion.Add(new Meteoride());
            }
        }
    }
}
