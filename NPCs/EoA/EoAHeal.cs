using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.NPCs.EoA;

namespace TerrariaUltraApocalypse.NPCs.EoA
{
    class EoAHeal : ModNPC
    {

        private Eye_of_Apocalypse owner;

        private int currentFrame = 0;
        private int animationTimer = 50;
        private int particleTimer = 10;

        public void setOwner(ModNPC owner)
        {
            this.owner = owner as Eye_of_Apocalypse;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of apocalypse Life Orb");
            DisplayName.AddTranslation(GameCulture.French, "Orbe de soin");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 1000;
            npc.damage = 0;
            npc.defense = 0;
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 28;
            npc.value = Item.buyPrice(0, 0, 0, 0);
            npc.npcSlots = 15f;
            npc.noGravity = true;
        }

        public override void AI()
        {
            if (particleTimer == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(new Vector2(npc.Center.X - 2, npc.Center.Y), 4, 4, DustID.Fire, 0f, -2f, 0, Color.LightGreen);
                    Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), 4, 4, DustID.Fire, 0f, -2f, 0, Color.LightGreen);
                    Dust.NewDust(new Vector2(npc.Center.X + 2, npc.Center.Y), 4, 4, DustID.Fire, 0f, -2f, 0, Color.LightGreen);
                }
                particleTimer = 10;
            }
            particleTimer--;
            
        }

        public override bool CheckDead()
        {
            owner.setTakeDamage();
            return base.CheckDead();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            float k = 0f;
            double d = 100000;

            target.HealEffect(500, true);
        }
        /*
        public override void FindFrame(int frameHeight)
        {
            if (animationTimer == 0) {
                currentFrame = (currentFrame == 1) ? 0 : 1;
                frame.Y = frameHeight * currentFrame;
                animationTimer = 50;
            }
            animationTimer--;
        }*/
    }
}
