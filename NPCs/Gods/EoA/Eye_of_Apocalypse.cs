using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Linq;


namespace TUA.NPCs.Gods.EoA
{
    public class EyeOApocalypse : ModNPC
    {
        private Player Target => Main.player[npc.target];
        private int[] playersHit;

        public override bool CloneNewInstances => false;

        private int cutscenetimer;
        private int cutscenePhase;
        private float opacity;
        private float previousMusicVolume;

        private int animationTimer = 20;
        private int animationFrame = 0;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Apocalypse - God of Destruction");
            DisplayName.AddTranslation(GameCulture.French, "Oeil de l'apocalypse - Dieu de la destruction");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 400000;
            npc.damage = 50;
            npc.defense = 55;
            npc.dontTakeDamage = true;
            npc.knockBackResist = 0f;
            npc.width = 124;
            npc.height = 154;
            npc.value = Item.buyPrice(20, 0, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.Opacity = opacity;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Exclusion_Zone");
            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }

            cutscenetimer = 1;
            cutscenePhase = 0;
            opacity = 0;
            previousMusicVolume = -1;
            playersHit = new int[0];
        }

        public override void AI()
        {
            if (npc.localAI[0] != 1f)
            {
                RotateToPlayer();

                if (cutscenePhase != 7)
                {
                    if (cutscenePhase >= 1)
                    {
                        SpawnCircleDust(10 * 16, DustID.Shadowflame);
                    }

                    ExecuteCutscene();
                    return;
                }
                TUAPlayer.LockPlayerCamera(null, false);
                Main.musicVolume = previousMusicVolume;
                if (npc.ai[1] == 0)
                {

                }
            }
        }

        public void RotateToPlayer()
        {
            npc.TargetClosest();
            float subit = (float)Math.PI / 2f;
            Vector2 distance = Target.Center - npc.Center;
            npc.rotation = (float)Math.Atan2(distance.Y, distance.X) - subit;
        }

        private void ExecuteCutscene()
        {
            if (previousMusicVolume == -1f)
            {
                previousMusicVolume = Main.musicVolume;
            }

            Main.musicVolume = 0f;
            cutscenetimer--;
            if (cutscenetimer == 0)
            {
                cutscenetimer = 300;
                opacity += 0.3f;
                npc.Opacity = opacity;
                switch (cutscenePhase)
                {
                    case 0:
                        TUA.instance.SetTitle("Eye of Azathoth", "The god of destruction", Color.Red, Color.Black, Main.fontDeathText, 300, 1, true);
                        TUA.BroadcastMessage("<???> Who hath summoned this to this wretched world? " +
                            "It will be a pleasure to destroy them, like I did a years ago.", Color.Black);
                        npc.GivenName = "???";
                        break;
                    case 1:
                        TUA.BroadcastMessage("<???> Humans sealed me eons ago, but now I'm finally free " +
                            "and I'll bring this world back to its most glorious stage, the era of destruction!");
                        break;
                    case 2:
                        npc.GivenName = "Eye of Azathoth";
                        TUA.BroadcastMessage($"<{npc.GivenName}> I, the Eye of Azathoth, the divine incarnation of destruction, " +
                            "will bring the world back to the times when humanity was nothing more than a speck on the earth.");
                        break;
                    case 3:
                        TUA.BroadcastMessage($"<{npc.GivenName}> Once I reigned supreme over all other gods of this world , " +
                            "but that despicable man completely defeated me, impossibly, " +
                            "and somehow me and my brotherhood was rendered powerless and trapped in another dimension.");
                        break;
                    case 4:
                        TUA.BroadcastMessage($"<{npc.GivenName}> I'll never forgive that human, never.");
                        break;
                    case 5:
                        TUA.BroadcastMessage($"<{npc.GivenName}> But now I am back to seek revenge on the humans, " +
                            "their worthless spells and so-called \"technology\" won't be able to stop me this time " +
                            "and everyone will perish under the plagues! I will allow all of humanity to be crushed by " +
                            "our divine power!");
                        break;
                    case 6:
                        TUA.instance.SetTitle("Eye of Azathoth", "The god of destruction", Color.Red, Color.Black, Main.fontDeathText, 300, 1, true);
                        TUA.BroadcastMessage($"<{npc.GivenName}> We are taking our throne back " +
                            "and we will conquer the world like we did a millenia ago, I hope you're ready to fight Terraria, " +
                            "otherwise it would be boring for me.");
                        break;
                }
                cutscenePhase++;
            }


            TUAPlayer.LockPlayerCamera(new Vector2(npc.Center.X - Main.screenWidth / 2, npc.Center.Y - Main.screenHeight / 2), true);

            Dust.NewDust(npc.Center, 5, 5, DustID.Electric, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 255,
                Color.White * opacity, 1f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.myPlayer == target.whoAmI && !playersHit.Contains(target.whoAmI) && target.statLife < target.statLife * .2)
            {
                switch (Main.rand.Next(5))
                {
                    case 0:
                        Main.NewText($"<{npc.GivenName}> No offense {target.name}, but you're struggling to keep up. " +
                            "Don't tell me you're out already!?");
                        break;
                    case 1:
                        Main.NewText($"<{npc.GivenName}> Come on, do better than that {target.name}!");
                        break;
                    case 2:
                        Main.NewText($"<{npc.GivenName}> How does it feel to know your place in this world?");
                        break;
                    case 3:
                        Main.NewText($"<{npc.GivenName}> Chide yourself in the afterlife for being so weak!");
                        break;
                    case 4:
                        Main.NewText($"<{npc.GivenName}> Tell me, how does it feel to be so close to death? I'm curious to know how " +
                            "absolutely terrifying to have your heart be beating so fast.");
                        break;
                    default:
                        Main.NewText($"<{npc.GivenName}> Tell me, you poor human, how does it feel to know " +
                            "that you'll never reach my caliber of power?");
                        break;
                }
                Array.Resize(ref playersHit, playersHit.Length + 1);
                playersHit[playersHit.Length - 1] = target.whoAmI;
            }
        }

        private void SpawnCircleDust(int radius, int dustType)
        {
            float x = 0;
            float y = 0;
            for (double circle = 0.0; circle < 360.0; circle += 2.0)
            {
                x = (float)(npc.Center.X + Math.Cos(circle) * radius);
                y = (float)(npc.Center.Y + Math.Sin(circle) * radius);

                Dust dust = Main.dust[Dust.NewDust(new Vector2(x, y), 2, 2, dustType, 0, 0, 0, Color.Black, 0.5f)];
                dust.noGravity = true;
                dust.fadeIn = 0f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            animationTimer--;
            if (animationTimer == 0)
            {
                animationTimer = 20;
                animationFrame++;
                if (animationFrame == 4)
                {
                    animationFrame = 0;
                }

                npc.frame.Y = frameHeight * animationFrame;
            }
        }
    }
}
