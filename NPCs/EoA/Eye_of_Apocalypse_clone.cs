using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.NPCs.EoA
{
    [AutoloadBossHead]
    class Eye_of_Apocalypse_clone : ModNPC
    {
        private int masterID;
        private String pos;
        private string target = "player";
        private int currentFrame = 1;
        private int animationTimer = 50;
        private int phase = 1;
        private int arenaCenterX;
        private int arenaCenterY;

        public void setMasterID(int masterID) {
            this.masterID = masterID;
        }

        public String getPos() {
            return pos;
        }

        public void setPos(String pos)
        {
            this.pos = pos;
        }

        public void setPhase(int phase) {
            this.phase = phase;
        }

        public void setTarget(string target) {
            this.target = target;
        }

        public Player GetPlayer(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            return player;
        }

        public void receiverArenaCoordinate(int x, int y) {
            arenaCenterX = x;
            arenaCenterY = y;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of apocalypse clone");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de l'apocalypse - Clone");
            Main.npcFrameCount[npc.type] = 6;
            
        }

        public override void AI()
        {
            Player p = GetPlayer(npc);
            setPositonFromPlayer(p);
            float subit = (float)Math.PI / 2f;
            Vector2 distance = p.Center - npc.Center;
            npc.rotation = (float)Math.Atan2(distance.Y, distance.X) - subit;

            if (npc.ai[2] > 0 && !Main.dayTime && !p.dead)
            {
                npc.ai[2] = 0;
            }


        }

        public void setPositonFromPlayer(Player p) {
            if (target == "player")
            {
                if (pos == "left")
                {
                    npc.position = new Vector2((int)(p.position.X + 360), (int)p.position.Y);
                }
                else if (pos == "right")
                {
                    npc.position = new Vector2((int)(p.position.X - 360), (int)p.position.Y);
                }
                else if (pos == "bottom")
                {
                    npc.position = new Vector2((int)(p.position.X), (int)p.position.Y + 360);
                }
                else if (pos == "top")
                {
                    npc.position = new Vector2((int)(p.position.X), (int)p.position.Y - 360);
                }
            }
            else if (target == "arena")
            {
                if (pos == "left")
                {
                    npc.position = new Vector2(arenaCenterX + 300, arenaCenterY - 50);
                }
                else if (pos == "right")
                {
                    npc.position = new Vector2(arenaCenterX - 420, arenaCenterY - 50);
                }
                else if (pos == "bottom")
                {
                    npc.position = new Vector2(arenaCenterX, arenaCenterY + 300);
                }
                else if (pos == "top")
                {
                    npc.position = new Vector2(arenaCenterX, arenaCenterY - 420);
                }
            }
            
        }

        public override void SetDefaults()
        {
            
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.damage = 50;
            npc.defense = 55;
            npc.knockBackResist = 0f;
            npc.width = 110;
            npc.height = 166;
            npc.value = Item.buyPrice(0, 20, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.buffImmune[24] = true;
            music = MusicID.Boss2;
            npc.dontTakeDamage = true;
        }

        public override void FindFrame(int frameHeight)
        {
            if (animationTimer == 0) {
                npc.frame.Y = frameHeight * currentFrame;
                currentFrame++;
                if (currentFrame == 3)
                {
                    currentFrame = 1;
                }
                animationTimer = 25;
            }
            animationTimer--;
        }
    }
}