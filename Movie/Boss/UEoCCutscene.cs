using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Cinematics;
using TUA.BaseMod.Base;
using TUA.NPCs.Gods.EoA;

namespace TUA.Movie.Boss
{
    class UEoCCutscene : Film
    {
        private NPC _UEoC;
        private EyeOApocalypse _MysteriousGod;
        private Vector2 EoCDeathPoint;
        private Vector2 _CameraTarget;
        private float _speed;

        public UEoCCutscene(Vector2 startingPoint)
        {
            EoCDeathPoint = startingPoint;
            _CameraTarget = startingPoint;
            _CameraTarget.X += 500;
            _speed = 1f;
            
            AppendKeyFrames(new FrameEvent[]
            {
                new FrameEvent(SpawnUEoC),
                new FrameEvent(UEoCInitialDialog), 
            });
            AppendSequence(60, UpdateCamera);
            AppendKeyFrames(new FrameEvent[]
            {
                new FrameEvent(SpawnMysteriousGod),
                new FrameEvent(InitialMysteriousGodDialog), 
            });
            AppendSequence(60, UpdateMysteriousGodOpacityInitial);
            AppendKeyFrame(UEoCfirstTalk);
        }

        private void SpawnUEoC(FrameEventData evt)
        {
            _UEoC = Main.npc[NPC.NewNPC((int) EoCDeathPoint.X, (int) EoCDeathPoint.Y, TUA.instance.NPCType("UltraEoC"))];
            _UEoC.GivenName = "Eye of Cthulhu";
            _UEoC.immortal = true;
            _UEoC.dontTakeDamage = true;
            _UEoC.takenDamageMultiplier = 0f;
            _UEoC.knockBackResist = 0f;
            _UEoC.immune[255] = 100000;
        }

        private void UEoCInitialDialog(FrameEventData evt)
        {
            BaseUtility.Chat("<Eye of Cthulhu> ... Not again, I'm too weak to defeat the terrarian, sorry lord but I cannot avenge you...");
            _UEoC.Opacity -= 0.25f;
        }

        private void UpdateCamera(FrameEventData evt)
        {
            
            Vector2 cameraPosition = Main.screenPosition;
            //Vector2 difference = cameraPosition - _CameraTarget;
            //difference = Vector2.Normalize(difference);
            //var dif = difference * (float) TUA.gameTime.ElapsedGameTime.TotalSeconds;
            cameraPosition = Vector2.Lerp(cameraPosition, _CameraTarget, 0.6f);
            //cameraPosition += difference * (float) TUA.gameTime.ElapsedGameTime.TotalSeconds * 0.4f;
            if ((cameraPosition - _CameraTarget).Length() < _speed)
            {
                cameraPosition = _CameraTarget;
            }
            TUAPlayer.LockPlayerCamera(cameraPosition, true);
        }

        

        private void SpawnMysteriousGod(FrameEventData evt)
        {
            _MysteriousGod =
                Main.npc[NPC.NewNPC((int)EoCDeathPoint.X + 1000, (int)EoCDeathPoint.Y, TUA.instance.NPCType("Eye_of_Apocalypse"))].modNPC as EyeOApocalypse;
            _MysteriousGod.npc.color = Color.Black;
            _MysteriousGod.npc.GivenName = "???";
            _MysteriousGod.npc.immortal = true;
            _MysteriousGod.npc.dontTakeDamage = true;
            _MysteriousGod.npc.takenDamageMultiplier = 0f;
            _MysteriousGod.npc.knockBackResist = 0f;
            _MysteriousGod.npc.immune[255] = 100000;
            _MysteriousGod.npc.Opacity = 0;
            _MysteriousGod.npc.rotation = _UEoC.rotation * -1;
            _MysteriousGod.npc.localAI[0] = 1f;
        }

        private void InitialMysteriousGodDialog(FrameEventData evt)
        {
            if (Main.netMode == 0)
            {
                string gender = Main.LocalPlayer.Male ? "him" : "her";
                BaseUtility.Chat($"<???> You failure of a minion, you can't even avenge your lord properly. But I am willing to give you a chance to beat {gender}.");
            }
            else
            {
                BaseUtility.Chat($"<???> You failure of a minion, you can't even avenge your lord properly. But I am willing to give you a chance to beat them.");
            }
        }

        private void UpdateMysteriousGodOpacityInitial(FrameEventData evt)
        {
            //_MysteriousGod.npc.Opacity += 0.05f;
        }

        private void UEoCfirstTalk(FrameEventData evt)
        {
            BaseUtility.Chat("<Eye of Cthulhu> Who are you???");
        }

        private void FreeLock(FrameEventData evt)
        {
            TUAPlayer.LockPlayerCamera(null, false);
        }

    }
}
