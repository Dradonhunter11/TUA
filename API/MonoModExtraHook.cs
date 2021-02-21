using System;
using System.Reflection;
using MonoMod.RuntimeDetour.HookGen;
using TUA.Utilities;

namespace TUA.API
{
    public class CustomMMHooker
    {
        public delegate void orig_populatebrowser(object instance);
        public delegate void hook_populatebrowser(orig_populatebrowser orig, object threadContext);

        public static event hook_populatebrowser Populatebrowser_Hook
        {
            add
            {
                HookEndpointManager.Add(ReflManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.ModBrowser.UIModBrowser").GetMethod("PopulateModBrowser", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
            remove
            {
                HookEndpointManager.Remove(ReflManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser.ModBrowser").GetMethod("PopulateModBrowser", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
        }
    }
}
