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
using TUA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TUA.NPCs.Gods.EoA;
using TUA.Projectiles.EoA;

namespace TUA.NPCs.Gods.EoA
{


    [AutoloadBossHead]
    class Eye_of_Apocalypse : ModNPC
    {
        public override bool CloneNewInstances { get { return true; } }


        private static bool cloneActive = false;
        private static int phase = 1;


        private int timer = 500;

        private int attackDelay = 100;

        private int currentFrame = 1;
        private int animationTimer = 50;

        private string target = "player";

        private Vector2 npcTarget = Vector2.Zero;

        private Vector2 CenterPosition;
        private int magnitude = 600;
        private bool magnitudeSwitch = false;


        private float maxVelocity = 0;
        private float currentVelocity = 0;
        private float targetVelocity = 0;
        private float targetMagnetude = 0;

        float theta = (float)Math.PI;


        private EoAHeal currentDamageSource;

        private string[] engQuote =
            {"You really think you can defeat a god?",
             "I'm the apocalypse, you are nothing...",
             "Why you summoned me when you know you are dead",
             "You think to be ready for the ultra mode? There won't be a comeback, the ancient god will be mad",
             "All this word is corrupted, so you are. We are the master of this world",
             "This world soon gonna be destroyed... by the god"};

        private string[] freQuote =
        {"Tu penses vraiment pouvoir tuer un dieu?",
         "Je suis l'apocalypse, tu es rien...",
         "Pourquoi m'invoquer quand t'a déja perdu.",
         "Tu pense être prêt pôur le mode ultra? Une fois fini, il n'aura pas de retour, les dieux anciens seront en colère",
         "Ce monde est corrompu, tout comme toi. Nous sommes les maîtres du monde",
         "Ce monde sera bientôt détruit... par les dieux"};


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Apocalypse - God of destruction");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de l'apocalypse - Dieu de la destruction");
            Main.npcFrameCount[npc.type] = 5;
        }

        private static Eye_of_Apocalypse_clone[] clone;

        private void initClone()
        {
            Player p = GetPlayer(npc);
            int clone1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0, npc.whoAmI);
            clone[0] = Main.npc[clone1].modNPC as Eye_of_Apocalypse_clone;
            clone[0].setPos("left");
            int clone2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0, npc.whoAmI);
            clone[1] = Main.npc[clone2].modNPC as Eye_of_Apocalypse_clone;
            clone[1].setPos("right");
            int clone3 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0, npc.whoAmI);
            clone[2] = Main.npc[clone3].modNPC as Eye_of_Apocalypse_clone;
            clone[2].setPos("bottom");
            cloneActive = true;
        }



        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 400000;
            npc.damage = 50;
            npc.defense = 55;
            npc.knockBackResist = 0f;
            npc.width = 132;
            npc.height = 166;
            npc.value = Item.buyPrice(20, 0, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.buffImmune[24] = true;
            music = MusicID.Boss2;
            clone = new Eye_of_Apocalypse_clone[3];
            cloneActive = false;
        }

        public static Player GetPlayer(NPC npc)
        {
            Player player = Main.player[npc.target];
            return player;
        }

        public int getMagnitude()
        {
            return magnitude;
        }

        public Vector2 getCenterPosition()
        {
            return CenterPosition;
        }

        public float getTetha()
        {
            return theta;
        }

        public Eye_of_Apocalypse_clone[] getClone()
        {
            return clone;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            bossLifeScale = 1f;
            numPlayers = 1;
            npc.lifeMax = 400000;
            return;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(magnitude);
            writer.Write(magnitudeSwitch);
            writer.Write(theta);
            writer.Write(cloneActive);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            magnitude = reader.Read();
            magnitudeSwitch = reader.ReadBoolean();
            theta = reader.ReadSingle();
            cloneActive = reader.ReadBoolean();
        }

        public override bool PreAI()
        {
            

            if (Main.netMode == 0)
            {
                Main.LocalPlayer.GetModPlayer<TUAPlayer>().noImmunityDebuff = true;
            }
            else
            {
                for (int i = 0; i < Main.player.Length - 1; i++)
                {
                    Main.player[i].GetModPlayer<TUAPlayer>().noImmunityDebuff = true;
                }
            }
            
            return base.PreAI();
        }

        //NEW AI
        public override void AI()
        {
            Player p = GetPlayer(npc);
            float subit = (float)Math.PI / 2f;
            Vector2 distance = p.Center - npc.Center;
            npc.rotation = (float)Math.Atan2(distance.Y, distance.X) - subit;

            if (!cloneActive && Main.netMode != 1)
            {
                initClone();
            }

            if (phase == 1)
            {
                if (CenterPosition == Vector2.Zero)
                {
                    setCirclePoint();
                }
                spin();
                //teleportPlayer();
                if (attackDelay == 0)
                {
                    attack(true, false, false, false);
                    attackDelay = 500;
                }

                attackDelay--;
            }
        }

        private void attack(bool main, bool clone1, bool clone2, bool clone3)
        {
            int projectile =
                Projectile.NewProjectile(npc.Center, Vector2.One, mod.ProjectileType("SmallerBeam"), 1, 0f);
            SmallerBeam beam = Main.projectile[projectile].modProjectile as SmallerBeam;
            beam.setMaster(npc.modNPC);
        }

        private void phase1(Player p)
        {
            
        }

        public void spin()
        {
            Vector2 center = CenterPosition;
            theta += (float)Math.PI / 360;
            center.X += (float)Math.Cos(theta) * magnitude;
            center.Y += (float)Math.Sin(theta) * magnitude;
            npc.velocity = npc.DirectionTo(center) * Vector2.Distance(center, npc.Center) / 10;

            if (magnitude > 800)
            {
                magnitudeSwitch = true;
            }
            else if (magnitude < 400)
            {
                magnitudeSwitch = false;
            }

            if (magnitudeSwitch)
            {
                magnitude--;
            }
            else
            {
                magnitude++;
            }
        }

        private void setCirclePoint()
        {
            npc.TargetClosest();
            if (npc.target != 255)
            {
                CenterPosition = GetPlayer(npc).Center;
            }
        }

        private void teleportPlayer()
        {
            foreach (Player p in Main.player)
            {
                if (Vector2.Distance(p.Center, CenterPosition) > magnitude)
                {
                    p.Center = CenterPosition;
                }
            }
        }

        

        private void phase2(Player p)
        {

        }

        private void phase3(Player p)
        {

        }


    

        public override void FindFrame(int frameHeight)
        {
            
            if (animationTimer == 0)
            {
                if (phase != 3)
                {
                    npc.frame.Y = frameHeight * currentFrame;
                    currentFrame++;
                    if (currentFrame == 2)
                    {
                        currentFrame = 0;
                    }
                }
                animationTimer = 25;
            }
            animationTimer--;
        }


        private string getTranslatedQuote()
        {
            if (Language.ActiveCulture == GameCulture.French)
            {
                return freQuote[Main.rand.Next(freQuote.Length - 1)];
            }
            return engQuote[Main.rand.Next(engQuote.Length)];
        }

        

        /**************************************/
        /**            TAPI AI               **/
        /**************************************/
        /*public override void AI()
        {
            timer--;
            if (timer == 0) {
                timer = 500;
                Main.NewText(getTranslatedQuote(), Color.DarkRed);
            }

            Player pla = GetPlayer(npc);

            if (npc.ai[2] > 0 && !Main.dayTime && !pla.dead)
            {
                npc.ai[2] = 0;
            }

            if (!Main.dayTime && !pla.dead && pla.active && npc.ai[2] == 0)
            {
                if (npc.ai[0] == 0f)
                {
                    npc.dontTakeDamage = false;
                    float subit = (float)Math.PI / 2f;
                    Player player = GetPlayer(npc);
                    Vector2 abovePlayer = player.Center + new Vector2(0f, -250f);

                    npc.ai[1]--;
                    bool shouldTeleport = false;
                    bool shouldCharge = false;
                    if (npc.ai[1] < -150f && npc.ai[1] >= -400f)
                    {
                        shouldTeleport = true;
                    }
                    else if (npc.ai[1] < -400f && npc.ai[1] >= -800f)
                    {
                        shouldCharge = true;
                    }
                    else if (npc.ai[1] < -800f)
                    {
                        npc.ai[1] = 250f;
                        npc.netUpdate = true;
                    }

                    if (!shouldTeleport && !shouldCharge)
                    {
                        float speed = 5f;
                        Vector2 move = abovePlayer - npc.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        if (magnitude > speed)
                        {
                            move *= speed / magnitude;
                        }
                        npc.velocity = move;
                        if (Main.netMode != 1 && npc.ai[1] % 25f == 0f)
                        {
                            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCDef.byName["Notch:ServantOfTheApocalypse"].type, 0);
                        }
                    }

                    Vector2 distance = player.Center - npc.Center;
                    float angle = (float)Math.Atan2(distance.Y, distance.X);
                    npc.rotation = angle - subit;

                    if (shouldTeleport && (npc.ai[1] + 151f) % 111f == 0f)
                    {
                        npc.velocity.Y = 0;

                        int num1 = Main.rand.Next(2);

                        for (int q = 0; q < 45; q++)
                        {
                            Vector2 randpos = new Vector2(npc.position.X + Main.rand.Next(-75, 76), npc.position.Y + Main.rand.Next(-75, 76));
                            int dustID = Dust.NewDust(randpos, 16, 16, 64, 0f, 0f, 0, Color.Yellow, 2.4f);
                            Main.dust[dustID].noGravity = true;
                        }

                        if (num1 == 0)
                        {
                            npc.position = new Vector2(player.Center.X + (float)Main.rand.Next(150, 550), player.Center.Y - (float)Main.rand.Next(150, 550));
                        }
                        else if (num1 == 1)
                        {
                            npc.position = new Vector2(player.Center.X - (float)Main.rand.Next(150, 550), player.Center.Y - (float)Main.rand.Next(150, 550));
                        }
                        npc.netUpdate = true;

                        for (int r = 0; r < 30; r++)
                        {
                            Vector2 randpos = new Vector2(npc.Center.X + Main.rand.Next(-75, 76), npc.Center.Y + Main.rand.Next(-75, 76));
                            int dustID = Dust.NewDust(randpos, 16, 16, 64, 0f, 0f, 0, Color.Yellow, 2.4f);
                            Main.dust[dustID].noGravity = true;
                        }
                    }

                    if (shouldTeleport && Main.netMode != 1)
                    {
                        float angle1 = npc.rotation + subit;
                        Vector2 velocity = new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1));
                        velocity *= 10f;
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, ProjDef.byName["Notch:ApocalypticFire"].type, 34, 0, Main.myPlayer);
                    }

                    if (shouldCharge)
                    {
                        float speed = 8f;
                        Vector2 move = player.Center - npc.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        if (magnitude > speed)
                        {
                            move *= speed / magnitude;
                        }
                        npc.velocity = move;
                    }

                    if (npc.life <= npc.lifeMax / 2)
                    {
                        int npcID = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 35, 125, 0);
                        
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 35, 126, 0);
                        
                        Main.NewText("Brothers, come and defend your  master!", 255, 40, 150);
                        spawnTwins = false;
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    
                }
                else
                {
                    float subit = (float)Math.PI / 2f;
                    Player player = GetPlayer(npc);

                    Vector2 distance = player.Center - npc.Center;
                    float angle = (float)Math.Atan2(distance.Y, distance.X);
                    npc.rotation = angle - subit;

                    int tpSpeed = npc.life / 1000;

                    if (tpSpeed < 45)
                    {
                        tpSpeed = 45;
                    }

                    if (npc.ai[1] > tpSpeed)
                    {
                        if (Main.netMode != 1)
                        {
                            float angle1 = npc.rotation + subit;
                            Vector2 velocity = new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1));
                            velocity *= 10f;
                            //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, ProjDef.byName["Notch:ApocalypticFire"].type, 34, 0, Main.myPlayer);
                        }

                        npc.velocity.Y = 0;

                        int num1 = Main.rand.Next(2);

                        for (int q = 0; q < 45; q++)
                        {
                            Vector2 randpos = new Vector2(npc.Center.X + Main.rand.Next(-75, 76), npc.Center.Y + Main.rand.Next(-75, 76));
                            int dustID = Dust.NewDust(randpos, 16, 16, 64, 0f, 0f, 0, Color.Yellow, 2.4f);
                            Main.dust[dustID].noGravity = true;
                        }

                        if (num1 == 0)
                        {
                            npc.position = new Vector2(player.Center.X + (float)Main.rand.Next(150, 550), player.Center.Y - (float)Main.rand.Next(150, 550));
                        }
                        else if (num1 == 1)
                        {
                            npc.position = new Vector2(player.Center.X - (float)Main.rand.Next(150, 550), player.Center.Y - (float)Main.rand.Next(150, 550));
                        }
                        npc.netUpdate = true;

                        for (int r = 0; r < 30; r++)
                        {
                            Vector2 randpos = new Vector2(npc.position.X + Main.rand.Next(-75, 76), npc.position.Y + Main.rand.Next(-75, 76));
                            int dustID = Dust.NewDust(randpos, 16, 16, 64, 0f, 0f, 0, Color.Yellow, 2.4f);
                            Main.dust[dustID].noGravity = true;
                        }

                        if (Main.netMode != 1)
                        {
                            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCDef.byName["Notch:ServantOfTheApocalypse"].type, 0);
                        }

                        if (npc.life <= npc.lifeMax / 3 && !spawnArmy)
                        {
                            Main.NewText("Army of nightmare, I summon you!");
                            for (int i = -10; i < 10; i++)
                            {
                                int npcID = NPC.NewNPC((int)npc.Center.X - i, (int)npc.Center.Y + i, 4, 0);
                                
                            }
                            spawnArmy = true;
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;

                        }

                        npc.ai[1] = 0;
                    }

                    npc.ai[1]++;
                }
            }
            else
            {
                npc.dontTakeDamage = true;
                npc.velocity.X = 0;
                npc.velocity.Y = 0;

                if (npc.ai[2] < 150)
                {
                    npc.rotation += rotspeed;

                    rotspeed += 0.05f;
                }
                else if (npc.ai[2] >= 150 && npc.ai[2] < 300)
                {
                    float subit = (float)Math.PI / 2f;
                    npc.rotation += rotspeed;

                    int DustyDust = Dust.NewDust(npc.Center, 16, 16, 64, Main.rand.Next(0, 5), Main.rand.Next(0, 5), 0, Color.Orange, 2f);
                    Main.dust[DustyDust].noGravity = true;

                    float angle1 = npc.rotation + subit;
                    Vector2 velocity = new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1));
                    velocity *= 10f;
                    if (Main.netMode != 1)
                    {
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, ProjDef.byName["Notch:ApocalypticFire"].type, 34, 0, Main.myPlayer);
                    }
                }
                else if (npc.ai[2] >= 300)
                {
                    Player player = Main.player[npc.target];
                    float speed2 = 1700f;
                    Vector2 move = new Vector2(0, 0) - npc.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > speed2)
                    {
                        move *= speed2 / magnitude;
                    }
                    npc.velocity = move;
                    for (int pl = 0; pl < Main.npc.Length; pl++)
                    {
                        if (Main.npc[pl].type == mod.NPCType("EyeOfApocalypse"))
                        {
                            Main.npc[pl].active = false;
                            break;
                        }
                    }
                }
                npc.ai[2]++;
                rotspeed += 0.000000003f;
            }
        }*/

    }
}
