using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using TUA.API.Dev;

using TUA.Utilities;

namespace TUA.Commands
{
    class ToggleEvent : ModCommand
    {
        public override bool Autoload(ref string name)
        {
            if (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
            {
                return true;
            }

            return false;
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (SteamID64Checker.Instance.VerifyDevID())
            {
                switch (args[0])
                {
                    case "Sandstorm":
                        if (Sandstorm.Happening)
                        {
                            ReflManager<Type>.GetItem("TMain").GetMethod("StopSandstorm", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        else
                        {
                            ReflManager<Type>.GetItem("TMain").GetMethod("StartSandstorm", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        Sandstorm.Happening = !Sandstorm.Happening;
                        TUA.BroadcastMessage("Sandstorm toggled " + ((Sandstorm.Happening) ? "on" : "off"));
                        break;
                    case "Rain":
                        if (Main.raining)
                        {
                            ReflManager<Type>.GetItem("TMain").GetMethod("StopRain", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        else
                        {
                            ReflManager<Type>.GetItem("TMain").GetMethod("StartRain", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }

                        TUA.BroadcastMessage("Rain toggled " + ((Main.raining) ? "on" : "off"));
                        break;
                    case "SlimeRain":
                        Main.slimeRain = !Main.slimeRain;
                        Main.slimeRainTime = (Main.slimeRain) ? 54000.0 : 0;
                        TUA.BroadcastMessage("Slime rain toggled " + ((Main.slimeRain) ? "on" : "off"));
                        break;
                    case "GoblinArmy":
                        Main.invasionType = ((Main.invasionType == 1) ? 0 : 1);
                        TUA.BroadcastMessage("Goblin army toggled " + ((Main.invasionType == 1) ? "on" : "off"));
                        break;
                    case "FrostLegion":
                        Main.invasionType = ((Main.invasionType == 2) ? 0 : 2);
                        TUA.BroadcastMessage("Frost legion toggled " + ((Main.invasionType == 2) ? "on" : "off"));
                        break;
                    case "PirateInvasion":
                        Main.invasionType = ((Main.invasionType == 3) ? 0 : 3);
                        TUA.BroadcastMessage("Pirate invasion toggled " + ((Main.invasionType == 3) ? "on" : "off"));
                        break;
                    case "MartianMadness":
                        Main.invasionType = ((Main.invasionType == 4) ? 0 : 4);
                        TUA.BroadcastMessage("Martian madness toggled " + ((Main.invasionType == 4) ? "on" : "off"));
                        break;
                    case "Eclipse":
                        Main.dayTime = true;
                        Main.time = 0;
                        Main.eclipse = !Main.eclipse;
                        TUA.BroadcastMessage("Solar eclipse toggled " + ((Main.eclipse) ? "on" : "off"));
                        break;
                    case "BloodMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.bloodMoon = !Main.bloodMoon;
                        TUA.BroadcastMessage("Blood moon toggled " + ((Main.bloodMoon) ? "on" : "off"));
                        break;
                    case "FrostMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.snowMoon = !Main.snowMoon;
                        TUA.BroadcastMessage("Frost moon toggled " + ((Main.snowMoon) ? "on" : "off"));
                        break;
                    case "PumpkinMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.pumpkinMoon = !Main.pumpkinMoon;
                        TUA.BroadcastMessage("Pumpkin moon toggled " + ((Main.pumpkinMoon) ? "on" : "off"));
                        break;
                    default:
                        TUA.BroadcastMessage("Event is not supported by the command currently");
                        break;
                }
            }
        }

        public override string Command => "ToggleEvent";
        public override CommandType Type => CommandType.World;

        public override string Description => "Toggle event manually, current available event list : \n" +
                                              "Sandstorm\n" +
                                              "Rain\n" +
                                              "SlimeRain\n" +
                                              "GoblinArmy\n" +
                                              "FrostLegion\n" +
                                              "PirateInvasion\n" +
                                              "MartianMadness\n" +
                                              "Eclipse\n" +
                                              "BloodMoon\n" +
                                              "FrostMoon\n" +
                                              "PumpkinMoon";
    }
}
