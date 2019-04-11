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
                HookEndpointManager.Add(StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser").GetMethod("PopulateModBrowser", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
            remove
            {
                HookEndpointManager.Remove(StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser").GetMethod("PopulateModBrowser", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
        }
    }
}
