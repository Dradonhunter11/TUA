using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.Dev
{
    class SteamID64Checker
    {
        private List<string> SteamId64List;
        
        private static SteamID64Checker instance;
        private static string CurrentSteamID64 = "76561198062217769";

        public static SteamID64Checker getInstance()
        {
            if (instance == null)
            {
                instance = new SteamID64Checker();
            }

            return instance;
        }

        private SteamID64Checker()
        {
            SteamId64List = new List<string>();

            PropertyInfo SteamID64Info =
                typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo SteamID64 = SteamID64Info.GetAccessors(true)[0];
            CurrentSteamID64 = (string)SteamID64.Invoke(null, new object[] { });

            SteamId64List.Add("76561198062217769"); //Dradonhunter11
            SteamId64List.Add("76561197970658570"); //2grufs
            SteamId64List.Add("76561193945835208"); //DarkPuppey
            SteamId64List.Add("76561193830996047"); //Gator
            SteamId64List.Add("76561198098585379"); //Chinzilla00
        }

        public bool verifyID()
        { 
            return SteamId64List.Contains(CurrentSteamID64);
        }

        public void CopyIDToClipboard()
        {
            Thread thread = new Thread(() => Clipboard.SetText(CurrentSteamID64));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            
        }
    }
}
