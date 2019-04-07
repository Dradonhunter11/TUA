using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                            StaticManager<Type>.GetItem("TMain").GetMethod("StopSandstorm", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        else
                        {
                            StaticManager<Type>.GetItem("TMain").GetMethod("StartSandstorm", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        Sandstorm.Happening = !Sandstorm.Happening;
                        BaseUtility.Chat("Sandstorm toggled " + ((Sandstorm.Happening) ? "on" : "off"));
                        break;
                    case "Rain":
                        if (Main.raining)
                        {
                            StaticManager<Type>.GetItem("TMain").GetMethod("StopRain", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }
                        else
                        {
                            StaticManager<Type>.GetItem("TMain").GetMethod("StartRain", BindingFlags.NonPublic | BindingFlags.Static)
                                .Invoke(null, new object[] { });
                        }

                        BaseUtility.Chat("Rain toggled " + ((Main.raining) ? "on" : "off"));
                        break;
                    case "SlimeRain":
                        Main.slimeRain = !Main.slimeRain;
                        Main.slimeRainTime = (Main.slimeRain) ? 54000.0 : 0;
                        BaseUtility.Chat("Slime rain toggled " + ((Main.slimeRain) ? "on" : "off"));
                        break;
                    case "GoblinArmy":
                        Main.invasionType = ((Main.invasionType == 1) ? 0 : 1);
                        BaseUtility.Chat("Goblin army toggled " + ((Main.invasionType == 1) ? "on" : "off"));
                        break;
                    case "FrostLegion":
                        Main.invasionType = ((Main.invasionType == 2) ? 0 : 2);
                        BaseUtility.Chat("Frost legion toggled " + ((Main.invasionType == 2) ? "on" : "off"));
                        break;
                    case "PirateInvasion":
                        Main.invasionType = ((Main.invasionType == 3) ? 0 : 3);
                        BaseUtility.Chat("Pirate invasion toggled " + ((Main.invasionType == 3) ? "on" : "off"));
                        break;
                    case "MartianMadness":
                        Main.invasionType = ((Main.invasionType == 4) ? 0 : 4);
                        BaseUtility.Chat("Martian madness toggled " + ((Main.invasionType == 4) ? "on" : "off"));
                        break;
                    case "Eclipse":
                        Main.dayTime = true;
                        Main.time = 0;
                        Main.eclipse = !Main.eclipse;
                        BaseUtility.Chat("Solar eclipse toggled " + ((Main.eclipse) ? "on" : "off"));
                        break;
                    case "BloodMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.bloodMoon = !Main.bloodMoon;
                        BaseUtility.Chat("Blood moon toggled " + ((Main.bloodMoon) ? "on" : "off"));
                        break;
                    case "FrostMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.snowMoon = !Main.snowMoon;
                        BaseUtility.Chat("Frost moon toggled " + ((Main.snowMoon) ? "on" : "off"));
                        break;
                    case "PumpkinMoon":
                        Main.dayTime = false;
                        Main.time = 0;
                        Main.pumpkinMoon = !Main.pumpkinMoon;
                        BaseUtility.Chat("Pumpkin moon toggled " + ((Main.pumpkinMoon) ? "on" : "off"));
                        break;
                    default:
                        BaseUtility.Chat("Event is not supported by the command currently");
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
