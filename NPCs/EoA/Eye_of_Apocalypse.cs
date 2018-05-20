using System;
using System.Collections.Generic;
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
using TerrariaUltraApocalypse.NPCs.EoA;

namespace TerrariaUltraApocalypse.NPCs.EoA
{


    [AutoloadBossHead]
    class Eye_of_Apocalypse : ModNPC
    {

        private String pos = null;
        private static int teleportTimer = 5000;
        private static bool cloneActive = false;
        private static int phase = 1;
        private static bool spawnWall = false;
        private static List<Tile> arena;
        public static int phaseTimer = 50000;

        public static int arenaCenterX;
        public static int arenaCenterY;

        private int timer = 500;

        private int attackDelay = 100;
        private int phase1Attack = 30;
        private bool phase1pause = false;

        private int currentFrame = 1;
        private int animationTimer = 50;

        private string target = "player";


        private EoAHeal currentDamageSource;

        private String[] engQuote =
            {"You really think you can defeat a god?",
             "I'm the apocalypse, you are nothing...",
             "Why you summoned me when you know you are dead",
             "You think to be ready for the ultra mode? There won't be a comeback, the ancient god will be mad",
             "All this word is corrupted, so you are. We are the master of this world",
             "This world soon gonna be destroyed... by the god"};

        private String[] freQuote =
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
            Main.npcFrameCount[npc.type] = 6;

        }

        private static Eye_of_Apocalypse_clone[] clone;

        private static void initClone(Mod mod, NPC npc)
        {
            Player p = GetPlayer(npc);
            int clone1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0);
            clone[0] = Main.npc[clone1].modNPC as Eye_of_Apocalypse_clone;
            clone[0].setPos("left");
            int clone2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0);
            clone[1] = Main.npc[clone2].modNPC as Eye_of_Apocalypse_clone;
            clone[1].setPos("right");
            int clone3 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Eye_of_Apocalypse_clone"), 0);
            clone[2] = Main.npc[clone3].modNPC as Eye_of_Apocalypse_clone;
            clone[2].setPos("bottom");
        }



        public override void SetDefaults()
        {

            npc.aiStyle = -1;
            npc.lifeMax = 400000;
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
            pos = null;
            clone = new Eye_of_Apocalypse_clone[3];
            cloneActive = false;
        }

        public static bool spawnTwins = false;
        public static bool spawnArmy = false;

        public static Player GetPlayer(NPC npc)
        {
            Player player = Main.player[npc.target];
            return player;
        }

        public String getPos()
        {
            return pos;
        }

        public Eye_of_Apocalypse_clone[] getClone()
        {
            return clone;
        }

        public void setPos(String pos)
        {
            this.pos = pos;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            bossLifeScale = 1f;
            numPlayers = 1;
            npc.lifeMax = 400000;
            return;
        }


        //NEW AI
        public override void AI()
        {
            Player p = GetPlayer(npc);
            float subit = (float)Math.PI / 2f;
            Vector2 distance = p.Center - npc.Center;
            npc.rotation = (float)Math.Atan2(distance.Y, distance.X) - subit;

            if (getPos() == null)
            {
                initClone(mod, npc);
                setPos("top");
            }
            if (p.active && !p.dead)
            {
                if (phase == 1)
                {
                    if (timer == 0)
                    {
                        swapClone(npc, Main.rand.Next(3));
                        timer = 5000;
                    }
                    timer--;

                    if (phase == 1 && !phase1pause)
                    {
                        phase1(p);
                    }
                    if (changePhase(300000, 30) || changePhase(200000, 60))
                    {
                        phase = 1;
                        phase1pause = false;
                    }
                }
            }

            if (p.dead)
            {
                clearArena(p);
            }
            setPositonFromPlayer(p);
        }

        private void swapClone(NPC npc, int cloneID)
        {
            int dust = Dust.NewDust(npc.Center, 50, 50, DustID.Fire, 6f, 6f, 100, Color.OrangeRed, 2);
            Main.dust[dust].noGravity = true;
            String oldpos = pos;

            setPos(getClone()[cloneID].getPos());
            getClone()[cloneID].setPos(oldpos);
        }


        private void phase1(Player p)
        {
            spawnArena(p);

            p.AddBuff(mod.BuffType("NoMountDebuff"), 1, false);
            if (attackDelay == 0)
            {
                //Main.NewText("Arena center (" + arenaCenterX + "; " + arenaCenterY + ")");
                p.AddBuff(mod.BuffType("NoMountDebuff"), 1, false);
                int height = Main.rand.Next(5, 25);
                int h = Main.rand.Next(5, 13);


                if (phase1Attack <= 10)
                {
                    for (int i = 0; i < height; i++)
                    {
                        spawnBottomLaser(1, i, 3f, 0);
                    }
                    attackDelay = 100;
                }
                else if (phase1Attack > 10 && phase1Attack <= 20)
                {
                    for (int i = 0; i < height; i++)
                    {
                        spawnBottomLaser(0, i, -3f, 0);
                    }
                    attackDelay = 100;
                }
                else if (phase1Attack > 20 && phase1Attack <= 30)
                {
                    height = Main.rand.Next(5, 15);
                    for (int i = 0; i < height; i++)
                    {
                        spawnBottomLaser(0, i, -3f, 0);
                        spawnBottomLaser(1, i, 3f, 0);
                    }
                    attackDelay = 125;
                }

                if (phase1Attack >= 31 && phase1Attack < 40)
                {
                    for (int i = 0; i < h; i++)
                    {
                        spawnBottomLaser(0, i, -3f, 0);
                        spawnTopLaser(1, i, 3f, 0);
                    }
                    attackDelay = 150;
                }
                else if (phase1Attack >= 40 && phase1Attack < 50)
                {
                    for (int i = 0; i > h; i++)
                    {

                    }
                }
                phase1Attack++;
            }

            if (phase1Attack >= 31 && phase1Attack < 40 && attackDelay % 10 == 0)
            {
                spawnFloorFireBall(1, 4, -2f, 1.5f, 3f);
                spawnFloorFireBall(1, 28, 2f, 1.5f, 3f);

                spawnFloorFireBall(1, 28, -2f, 1.5f, 3f);
                spawnFloorFireBall(1, 4, 2f, 1.5f, 3f);
            }

            if (phase1Attack == 30 || phase1Attack == 60)
            {
                spawnOrb();
                attackDelay = 50;
            }
            Main.NewText(phase1Attack);
            attackDelay--;
            //spawnSideFireBall(0, Main.rand.Next(0, 25), 1f, 7f);

        }

        private void spawnArena(Player p)
        {
            if (!spawnWall)
            {
                placeArena(p);
                spawnWall = true;
                npc.dontTakeDamage = true;
                if (phase1Attack == 0)
                {
                    Main.NewText("<Eye of Apocalypse> : Terrarian, get ready to suffer. As the god of destruction, you should be erased from this world. You scealed our master and you'll pay for it! Now that the moon lord is dead, we are all awake and are ready to unslead the god of element.", Color.Purple);
                }
                else if (phase1Attack == 31)
                {
                    Main.NewText("<Eye of Apocalypse> : How fun is it to be trap like a vulgar fly? I hope you are having fun here because you'll be trapped for the eternity!", Color.Purple);
                }

                Main.NewText("Your soul feels heavy in all the sudden...", Color.DarkCyan);
                target = "arena";
                setCloneTarget("arena");
                sendArenaCoordinate();
            }
        }

        private void phase2(Player p)
        {

        }

        private void phase3(Player p)
        {

        }

        private bool changePhase(int lifePhase, int requiredPhase)
        {
            Main.NewText(phase1Attack);
            if (npc.life < lifePhase && phase1Attack == requiredPhase)
            {
                phase1Attack++;
                Main.NewText("test");
                return true;
            }
            return false;
        }

        //EoA will never use this method by itself, but it's used by the heal orb to bring him in the vulnerable phase
        public void setTakeDamage()
        {
            if (phase == 1)
            {
                Main.NewText("<Eye of Apocalypse> : Don't think this will be this easy, this is FAR from my final form.", Color.Purple);
            }
            npc.dontTakeDamage = false;
            target = "player";
            setCloneTarget("player");
            spawnWall = false;

            clearArena(GetPlayer(npc));
        }

        public void setCloneTarget(string target)
        {
            for (int i = 0; i < 3; i++)
            {
                clone[i].setTarget(target);
            }
        }

        public void sendArenaCoordinate()
        {
            for (int i = 0; i < 3; i++)
            {
                clone[i].receiverArenaCoordinate(arenaCenterX, arenaCenterY);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            damage *= 4;
            base.OnHitByProjectile(projectile, damage, knockback, crit);
        }

        public override void FindFrame(int frameHeight)
        {
            if (animationTimer == 0)
            {
                if (phase != 3)
                {
                    npc.frame.Y = frameHeight * currentFrame;
                    currentFrame++;
                    if (currentFrame == 3)
                    {
                        currentFrame = 1;
                    }
                }
                animationTimer = 25;
            }
            animationTimer--;
        }

        public override bool CheckDead()
        {
            if (phase1pause)
            {
                npc.HealEffect(400000, true);
                npc.life = 400000;
                phase = 2;
                npc.dontTakeDamage = true;
                return false;
            }
            return true;
        }

        private String getTranslatedQuote()
        {
            if (Language.ActiveCulture == GameCulture.French)
            {
                return freQuote[Main.rand.Next(freQuote.Length - 1)];
            }
            return engQuote[Main.rand.Next(engQuote.Length)];
        }

        public void setPositonFromPlayer(Player p)
        {
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

        public void placeArena(Player p)
        {
            int initX = (int)(p.position.X) / 16;
            int initY = (int)(p.position.Y) / 16;

            Main.NewText("Current X - " + initX + "| Current Y - " + initY);

            int centerX = (int)(p.position.X + (float)(p.width / 2)) / 16;
            int centerY = (int)(p.position.Y + (float)(p.height / 2)) / 16;

            arenaCenterX = centerX * 16;
            arenaCenterY = centerY * 16;

            int halfLength = p.width * 16 / 2 / 16 + 1;
            for (int x = centerX - 16; x <= centerX + 16; x++)
            {
                for (int y = centerY - 16; y <= centerY + 16; y++)
                {
                    Main.tile[x, y].active(false);
                    if ((x == centerX - 16 || x == centerX + 16 || y == centerY - 16 || y == centerY + 16))
                    {
                        Main.tile[x, y].type = (ushort)mod.TileType("Arena");
                        Main.tile[x, y].active(true);
                    }


                }
            }
        }

        private void clearArena(Player p)
        {
            int centerX = (int)(p.position.X + (float)(p.width / 2)) / 16;
            int centerY = (int)(p.position.Y + (float)(p.height / 2)) / 16;

            arenaCenterX = 0;
            arenaCenterY = 0;
            spawnWall = false;

            for (int x = centerX - 16; x <= centerX + 16; x++)
            {
                for (int y = centerY - 16; y <= centerY + 16; y++)
                {
                    Main.tile[x, y].active(false);
                }
            }
        }

        public void spawnBottomLaser(int side, int y, float speedX, int speedY)
        {
            Projectile.NewProjectile(arenaCenterX + ((side == 0) ? 256 : -256), (arenaCenterY + 256) - y * 16, speedX, speedY, mod.ProjectileType("EoALaserWall"), 50, 0);
        }

        public void spawnTopLaser(int side, int y, float speedX, int speedY)
        {
            Projectile.NewProjectile(arenaCenterX + ((side == 0) ? 256 : -256), (arenaCenterY - 256) + y * 16, speedX, speedY, mod.ProjectileType("EoALaserWall"), 50, 0);
        }

        public void spawnOrb()
        {
            int pellet = NPC.NewNPC(arenaCenterX - 256 + Main.rand.Next(10, 500), arenaCenterY + 256 - Main.rand.Next(10, 500), mod.NPCType("EoAHeal"));
            currentDamageSource = Main.npc[pellet].modNPC as EoAHeal;
            currentDamageSource.setOwner(npc.modNPC);
            phase1pause = true;
        }

        public void spawnSideFireBall(int side, int y, float speedX, float speedY)
        {
            int projectile = Projectile.NewProjectile(arenaCenterX + ((side == 0) ? 256 : -256), (arenaCenterY + 256) - (y * 16), (int)speedX, (int)speedY, mod.ProjectileType("FireBall"), 50, 0f);
            Main.projectile[projectile].ai[0] = 1;
        }

        public void spawnFloorFireBall(int side, int x, float speedX, float speedY, float lifeTime = 0)
        {
            int projectile = Projectile.NewProjectile((arenaCenterX + 256) - (x * 16), arenaCenterY + ((side == 0) ? 256 : -256), (int)speedX, (int)speedY, mod.ProjectileType("FireBall"), 50, 0f);
            Main.projectile[projectile].ai[0] = 0;
            if (lifeTime != 0)
            {
                Main.projectile[projectile].timeLeft *= (int)lifeTime;
            }
        }

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
