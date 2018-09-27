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
        private Eye_of_Apocalypse master;
        private int masterID;
        private String pos;
        private string target = "player";
        private int currentFrame = 1;
        private int animationTimer = 50;
        private int phase = 1;


        public String getPos()
        {
            return pos;
        }

        public void setPos(String pos)
        {
            this.pos = pos;
            npc.target = Main.LocalPlayer.whoAmI;
        }

        public float getMagnitude()
        {
            return master.getMagnitude();
        }

        public Vector2 getCenterPoint()
        {
            return master.getCenterPosition();
        }

        public Player GetPlayer(NPC npc)
        {
            Player player = Main.player[npc.target];
            return player;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(pos);
            writer.Write(masterID);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            pos = reader.ReadString();
            masterID = reader.ReadInt32();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of apocalypse clone");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de l'apocalypse - Clone");
            Main.npcFrameCount[npc.type] = 5;

        }

        public override bool PreAI()
        {
            if (master == null)
            {
                masterID = (int) npc.ai[0];
                master = Main.npc[masterID].modNPC as Eye_of_Apocalypse;
                
            }
            return true;
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

            spin();

        }

        public void spin()
        {
            Vector2 center;
            if (Main.netMode != 1)
            {
                center = master.getCenterPosition();
            }
            else
            {
                Eye_of_Apocalypse eoa = Main.npc[masterID].modNPC as Eye_of_Apocalypse;
                center = eoa.getCenterPosition();
            } 


            float magnitude = master.getMagnitude();
            center.X += (float)Math.Cos(master.getTetha() + setPositonFromPlayer(GetPlayer(npc)) * (Math.PI / 2)) * magnitude;
            center.Y += (float)Math.Sin(master.getTetha() + setPositonFromPlayer(GetPlayer(npc)) * (Math.PI / 2)) * magnitude;

            npc.velocity = npc.DirectionTo(center) * Vector2.Distance(center, npc.Center) / 10;
        }

        
        public int setPositonFromPlayer(Player p)
        {
            switch (getPos())
            {
                case "left" :
                    return 1;
                case "right":
                    return 2;
                case "top":
                    return 3;
                case "bottom":
                    return -1;
                default:
                    break;
            }
            return 2;
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
            if (animationTimer == 0)
            {
                npc.frame.Y = frameHeight * currentFrame;
                currentFrame++;
                if (currentFrame == 2)
                {
                    currentFrame = 0;
                }
                animationTimer = 25;
            }
            animationTimer--;
        }
    }
}