using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace TUA.API.Dev
{
    internal class SteamID64Checker
    {
        private static readonly List<string> _steamId64List = new List<string>
        {
            "76561198062217769", // Dradonhunter11
            "76561197970658570", // 2grufs
            "76561193945835208", // DarkPuppey
            "76561193830996047", // Gator
            "76561198098585379", // Chinzilla00
            "76561198265178242", // Demi
            "76561193989806658", // SDF
            "76561198193865502", // Agrair
            "76561198108364775", // HumanGamer
            "76561198046878487" // Webmilio
        };

        private static SteamID64Checker _instance;
        public static string currentSteamID64;

        public static SteamID64Checker Instance
        {
            get
            {
                _instance = _instance ?? new SteamID64Checker();

                return _instance;
            }
        }

        private SteamID64Checker()
        {
            
            PropertyInfo steamID64Info =
                typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic);

            MethodInfo steamID64 = steamID64Info.GetAccessors(true)[0];
            currentSteamID64 = (string)steamID64.Invoke(null, new object[] { });
        }

        public bool VerifyDevID() => _steamId64List.Contains(currentSteamID64);
    }
}
