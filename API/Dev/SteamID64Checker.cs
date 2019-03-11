using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.Social;

namespace TUA.API.Dev
{
    class SteamID64Checker
    {
        private static List<string> SteamId64List = new List<string>
        {
            "76561198062217769", //Dradonhunter11
            "76561197970658570", //2grufs
            "76561193945835208", //DarkPuppey
            "76561193830996047", //Gator
            "76561198098585379", //Chinzilla00
            "76561198265178242", //Demi
            "76561193989806658", //SDF
            "76561198193865502", //Agrair
            "76561198108364775"  //HumanGamer
        };

        private static SteamID64Checker instance;
        internal static string CurrentSteamID64;

        public static SteamID64Checker Instance
        {
            get
            {
                instance = instance ?? new SteamID64Checker();

                return instance;
            }
        }

        private SteamID64Checker()
        {
            
            PropertyInfo SteamID64Info =
                typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo SteamID64 = SteamID64Info.GetAccessors(true)[0];
            CurrentSteamID64 = (string)SteamID64.Invoke(null, new object[] { });
        }

        public bool VerifyDevID() => SteamId64List.Contains(CurrentSteamID64);

        public bool CheckSpecificID(string ID) => CurrentSteamID64 == ID;

        public void CopyIDToClipboard()
        {
        }
    }
}
