using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace TUA.API.Dev
{
    class SteamID64Checker
    {
        private List<string> SteamId64List;
        
        private static SteamID64Checker instance;
        private static string CurrentSteamID64;

        public static SteamID64Checker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SteamID64Checker();
                }

                return instance;
            }
        }

        private SteamID64Checker()
        {
            PropertyInfo SteamID64Info =
                typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo SteamID64 = SteamID64Info.GetAccessors(true)[0];
            CurrentSteamID64 = (string)SteamID64.Invoke(null, new object[] { });

            SteamId64List = new List<string>
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
        }

        public bool VerifyDevID() => SteamId64List.Contains(CurrentSteamID64);

        public bool CheckSpecificID(string ID) => CurrentSteamID64 == ID;

        public void CopyIDToClipboard()
        {
        }
    }
}
