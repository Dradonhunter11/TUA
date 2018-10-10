using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TerrariaUltraApocalypse;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerrariaUltraApocalypse.NPCs.Gods.EoA;
using TerrariaUltraApocalypse.Projectiles.EoA;

namespace TerrariaUltraApocalypse.NPCs.Gods.EoA
{
    class Eye_of_ApocalypseNew : ModNPC
    {
        public override bool CloneNewInstances { get { return true; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Apocalypse - God of destruction");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de l'apocalypse - Dieu de la destruction");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 400000;
            npc.damage = 50;
            npc.defense = 55;
            npc.dontTakeDamage = true;
            npc.knockBackResist = 0f;
            npc.width = 132;
            npc.height = 166;
            npc.value = Item.buyPrice(20, 0, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = MusicID.Boss2;
            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
        }

        public override void AI()
        {
            if (npc.ai[1] == 0)
            {
                
            }
        }
    }
}
