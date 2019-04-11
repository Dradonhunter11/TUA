using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.NPCs.UltraBoss.UltraEoC
{
    public class UltraEoC : TUAModNPC
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.EyeofCthulhu; } }

        public bool CloneCharge = false; //Post draw stuff, Ignore
        public float CloneOpacity = 0.0f;

        public int CutsceneTimer = 0;
        public int CutscenePhase = 0;
        public bool IsCutsceneExecuting = false;
        public int CutsceneDustTimer = 20;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Eye of Cthulhu");
            DisplayName.AddTranslation(GameCulture.French, "Hyper Oeuil de cthulhu");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.EyeofCthulhu);
            npc.aiStyle = -1;
            npc.damage = 80 * (int)(1 + TUAWorld.EoCDeath * 1.5);
            npc.defense = 100 * (int)(1 + TUAWorld.EoCDeath * 1.05);
            npc.lifeMax = 12500 * (1 + TUAWorld.EoCDeath * 2);
        }

        public override bool PreAI()
        {
            if (npc.target == 255 || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
                if (npc.target == 255)
                {
                    npc.active = false;
                }
            }

            return base.PreAI();
        }

        public override void AI()
        {
        }

        private void ExecuteCutscene()
        {
            Vector2 centerPosition = new Vector2(npc.position.X + 300 , npc.Center.Y);
            npc.velocity = Vector2.Zero;
            
            TUAPlayer.LockPlayerCamera(new Vector2(npc.position.X + 300 - Main.screenWidth / 2, npc.Center.Y - Main.screenHeight / 2), true);
            


            if (CutsceneTimer == 0)
            {
                switch (CutscenePhase)
                {
                    case 0:
                        npc.GivenName = "Eye of Cthulhu";
                        BaseUtility.Chat("<Eye of Cthulhu> I'm sorry, I failed you master... The terrarian are much more stronger than I thought");
                        CutsceneTimer = 200;
                        CutscenePhase++;
                        break;
                    case 1:
                        BaseUtility.Chat("<???> You failure of a minion, you can't even do your job properly. I was sealed for 1000 year ago and when I wake up I find that the terrarian are more strong than what they were during the god reign time.");
                        CutsceneTimer = 300;
                        CutscenePhase++;
                        break;
                    case 2:
                        BaseUtility.Chat("<Eye of Cthulhu> But master, those are not the same from 1000 year ago, those area new race.");
                        CutsceneTimer = 250;
                        CutscenePhase++;
                        break;
                    case 3:
                        BaseUtility.Chat("<???> I do not care about what they are or what they became, I can still smell the presence of the traveler around the world, the one that made us perish.");
                        BaseUtility.Chat("<???> Anyway, enough talking for now, I'll give you one last chance and don't fail on me. Here is a fraction of my power");
                        CutsceneTimer = 400;
                        CutscenePhase++;
                        break;
                    case 4:
                        BaseUtility.Chat("<Ultra Eye of Cthulhu> Master, I promise I will do this request, the terrarian will be destroyed.");
                        TUA.instance.SetTitle("Ultra Eye of Cthulhu", "Not what you were expecting?", Color.White, Color.DarkRed, Main.fontDeathText, 600, 0.8f, true);
                        TUAPlayer.LockPlayerCamera(null, false);
                        IsCutsceneExecuting = false;
                        CutscenePhase++;
                        npc.GivenName = "Ultra Eye of Cthulhu";
                        break;
                }
            }

            if (CutscenePhase == 4)
            {
                npc.rotation -= 0.5f;
                if (CutsceneDustTimer == 0)
                {
                    Dust.NewDust(npc.Center, 2, 2, DustID.Shadowflame, Main.rand.Next(-4, 4), Main.rand.Next(-4, 4),
                        0, Color.White, 2f);
                    CutsceneDustTimer = 2;
                }

                CutsceneDustTimer--;
            }
            else
            {
                rotateToPosition(centerPosition);
            }

            CutsceneTimer--;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            /*if (IsCutsceneExecuting)
            {
                Vector2 EoAIllusionDrawingPosition = npc.position - Main.screenPosition + new Vector2(600f, 0f);
                npc.TargetClosest();
                float subit = (float)Math.PI / 2f;
                Vector2 distance = EoAIllusionDrawingPosition - npc.Center;
                spriteBatch.Draw(TUA.instance.GetTexture("NPCs/Gods/EoA/Eye_of_Apocalypse"), EoAIllusionDrawingPosition, new Rectangle(0, 0, 132, 166), Color.Black, (float)Math.Atan2(distance.Y, distance.X) - subit, new Vector2(npc.frame.X, npc.frame.Y), npc.scale, SpriteEffects.None, 1f);
            }*/

            if (CloneCharge)
            {
                CloneOpacity = (npc.life < npc.lifeMax / 2) ? 1.0f : 0.5f;
                Vector2 cloneDrawingPosition; // = GetCloneDrawingPosition(Main.player[npc.target].Center, Vector2.Distance(npc.Center, Main.player[npc.target].Center));
                cloneDrawingPosition = npc.Center - GetXYDistance(Main.player[npc.target].Center, npc.Center) * 2;
                spriteBatch.Draw(ModContent.GetTexture(Texture), cloneDrawingPosition, null, Color.White * CloneOpacity, npc.rotation * -1, new Vector2(npc.frame.X, npc.frame.Y), npc.scale, SpriteEffects.None, 1f);
            }
            
            base.PostDraw(spriteBatch, drawColor);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (CloneCharge)
            {
                return false;
            }
            

            return base.DrawHealthBar(hbPosition, ref scale, ref position);
        }

        public Vector2 GetCloneDrawingPosition(Vector2 targetPosition, float distance)
        {
            int quadrant = GetQuadrant(targetPosition, npc.Center, distance);

            switch (quadrant)
            {
                case 1:
                    return new Vector2(npc.Center.X - distance * 2, npc.Center.Y + distance * 2);
                case 2:
                    return new Vector2(npc.Center.X - distance * 2, npc.Center.Y - distance * 2);
                case 3:
                    return new Vector2(npc.Center.X + distance * 2, npc.Center.Y - distance * 2);
                case 4:
                    return new Vector2(npc.Center.X - distance * 2, npc.Center.Y + distance * 2);
                default:
                    return npc.Center;
            }
        }

        public int GetQuadrant(Vector2 TargetCenter, Vector2 NpcCenter, float distance)
        {
            if (TargetCenter == NpcCenter)
            {
                return 0;
            }

            int value = (int)(Math.Pow((npc.Center.X - TargetCenter.X), 2) + Math.Pow((npc.Center.Y - TargetCenter.Y), 2));

            if (value > Math.Pow(distance, 2))
            {
                return -1;
            }

            if (TargetCenter.X > NpcCenter.X && TargetCenter.Y >= NpcCenter.Y)
            {
                return 1;
            }

            if (TargetCenter.X <= NpcCenter.X && TargetCenter.Y > NpcCenter.Y)
            {
                return 2;
            }

            if (TargetCenter.X < NpcCenter.X && TargetCenter.Y <= NpcCenter.Y)
            {
                return 3;
            }

            if (TargetCenter.X >= NpcCenter.X && TargetCenter.Y < NpcCenter.Y)
            {
                return 4;
            }

            return 0;
        }

        public Vector2 GetXYDistance(Vector2 TargetPosition, Vector2 SourcePosition)
        {
            return TargetPosition - SourcePosition;
        }

        public void rotateToPosition(Vector2 position)
        {
            npc.TargetClosest();   
            float subit = (float)Math.PI / 2f;
            Vector2 distance = position - npc.Center;
            npc.rotation = (float)Math.Atan2(distance.Y, distance.X) - subit;
        }
    }
}
