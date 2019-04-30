using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Main;

namespace TUA.Dimension.Solar
{
    class SolarWorld : ModWorld
    {
        public bool MeteorRain = false;
        public bool VolcanoTremor = false;
        public bool PillarCrashEvent = false;
        public bool SolarFog = false;
        public bool ashRain = false;


        public int eventTimer = 1;
        public int activeEventTimer = 1;


        public override void PreUpdate()
        {
            if (Dimlibs.Dimlibs.getPlayerDim() == "Solar")
            {
                eventTimer--;
                if (eventTimer == 0)
                {
                    eventTimer = rand.Next(54000, 72000);
                    SelectEvent();
                }
            }
        }

        public override void PostUpdate()
        {
            base.PostUpdate();
        }

        private void SelectEvent()
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    PillarCrashEvent = true;
                    BaseUtility.Chat("A pillar has crashed into the atmosphere, a massive fog cover the area around the pillar");
                    CrashPillar();
                    break;
                case 1:
                    VolcanoTremor = true;
                    BaseUtility.Chat("A volcano tremor is happening!");
                    VolcanoTremorInitialize();
                    break;
                case 2:
                    MeteorRain = true;
                    BaseUtility.Chat("Meteor are falling from the sky");
                    MeteorRainInitialize();
                    break;
                case 3:
                    SolarFog = true;
                    BaseUtility.Chat("A massive fog is surrounding the surface");
                    FogInitialize();
                    break;
            }
        }

        public void CrashPillar()
        {
            //Write pillar crash logic here
        }

        public void VolcanoTremorInitialize()
        {
            //Write init code here
        }

        public void MeteorRainInitialize()
        {
            //write init code here
        }

        public void VolcanoTremorUpdate()
        {
            //Write update logic here for the volcano tremor
        }

        public void MeteorRainUpdate()
        {
            //Write update logic here for the meteor rain
        }

        public void FogInitialize()
        {

        }

        public void FogUpdate()
        {

        }

        public bool PillarDetection()
        {
            Player player = LocalPlayer;
            return npc.Any(i =>
                i.type == NPCID.LunarTowerSolar && Vector2.Distance(i.position, player.position) < 9600);
        }

        public override void PostDrawTiles()
        {
            
            if (Dimlibs.Dimlibs.getPlayerDim() == "Solar" && PillarCrashEvent)
            {
                if (PillarDetection())
                {
                    //Write solar pillar crash dark screen code here
                }
            }
        }
    }
}
