using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace TUA.API.Dev
{
    class SteamID64Checker
    {
        public static string CurrentSteamID64;

        private static List<string> SteamId64List;

        public static void Initiate()
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
            System.Windows.Forms.Clipboard.SetText(CurrentSteamID64);
        }

        public static bool VerifyID() => SteamId64List.Contains(CurrentSteamID64);
    }
}
