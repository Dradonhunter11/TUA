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

namespace TUA.NPCs.Gods.EoA
{
    public class Eye_of_Apocalypse : ModNPC
    {
        private Player target => Main.player[npc.target];
        

        public override bool CloneNewInstances => false;

        private int cutscenetimer = 1;
        private int cutscenePhase = 0;
        private float opacity = 0f;
        private float previousMusicVolume = -1f;
        

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
            npc.Opacity = opacity;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Exclusion_Zone");
            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
        }

        public override void AI()
        {
            rotateToPlayer();
            
            if (cutscenePhase != 6)
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

        public void rotateToPlayer()
        {
            npc.TargetClosest();
            Player p = target;
            float subit = (float)Math.PI / 2f;
            Vector2 distance = p.Center - npc.Center;
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
                        BaseUtility.Chat("<???> Who summoned this to this world again? It will be a pleasure to destroy them, like I did a long time ago.", Color.Black);
                        npc.GivenName = "???";
                        break;
                    case 1:
                        BaseUtility.Chat("<???> Human have sealed me a long time ago, but now I'm finally free and I'll bring this world back to what it was, the destruction era.");
                        break;
                    case 2:
                        BaseUtility.Chat("<Eye of Azathoth - God of destruction> I, the Eye of Azathoth, the god of destruction will bring the world once again to the state of when it was created.");
                        npc.GivenName = "Eye of Azathoth";
                        break;
                    case 3:
                        BaseUtility.Chat("<Eye of Azathoth - God of destruction> Once I was the main god of this world, but an old man completly crushed me and other god friend and trapped us into another dimension.");
                        break;
                    case 4:
                        BaseUtility.Chat("<Eye of Azathoth - God of destruction> But now I am back to seek revenge on the human, their spell and technology won't be able to stop me this time and everyone should perish under the plagues!");
                        break;
                    case 5:
                        TUA.instance.SetTitle("Eye of Azathoth", "The god of destruction", Color.Red, Color.Black, Main.fontDeathText, 300, 1, true);
                        BaseUtility.Chat("<Eye of Azathoth - God of destruction> We are taking our right back and we will conquer the world like we did a 1000 years ago, be ready to fight. ");
                        
                        break;
                }
                cutscenePhase++;
            }


            TUAPlayer.LockPlayerCamera(new Vector2(npc.Center.X - Main.screenWidth / 2, npc.Center.Y - Main.screenHeight / 2), true);

            Dust.NewDust(npc.Center, 5, 5, DustID.Electric, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 255,
                Color.White * opacity, 1f);
        }

        

        private void SpawnCircleDust(int radius, int dustType)
        {
            float x = 0;
            float y = 0;
            for (double circle = 0.0; circle < 360.0; circle += 2.0)
            {
                x = (float) (npc.Center.X + Math.Cos(circle) * radius);
                y = (float) (npc.Center.Y + Math.Sin(circle) * radius);

                Dust dust = Main.dust[Dust.NewDust(new Vector2(x, y), 2, 2, dustType, 0, 0, 0, Color.Black, 0.5f)];
                dust.noGravity = true;
                dust.fadeIn = 0f;
            }
        }
    }
}
