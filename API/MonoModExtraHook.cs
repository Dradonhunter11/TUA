using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;

namespace TUA.API
{
    internal class MonoModExtraHook
    {
        public delegate void orig_populatebrowser(object instance);
        public delegate void hook_populatebrowser(orig_populatebrowser orig, object threadContext);

        public static event hook_populatebrowser populatebrowser_Hook
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
