using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.API.EventManager
{
    class MoonEventManagerWorld : ModWorld
    {
        internal static Dictionary<string, MoonEvent> moonEventList = new Dictionary<string, API.EventManager.MoonEvent>();

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            MoonEvent.ResetMoon();
            foreach (var moonEvent in moonEventList.Values)
            {
                moonEvent.Save(tag);
            }
            return base.Save();
        }

        public override void Load(TagCompound tag)
        {
            MoonEvent.ResetMoon();
            foreach (var moonEvent in moonEventList.Values)
            {
                moonEvent.Load(tag);
            };
        }

        public override void PostUpdate()
        {
            if (moonEventList.Values.Any(i => i.IsActive))
            {
                
                MoonEvent activeEvent = moonEventList.Values.Single(i => i.IsActive);
                if (Main.dayTime)
                {
                    

                }

                activeEvent.ReplaceMoon();
            }
        }

        public static bool Activate(string eventName)
        {
            if (moonEventList.ContainsKey(eventName) && moonEventList.Values.Any(i => !i.IsActive) && !Main.dayTime)
            {
                moonEventList[eventName].Activate();
            }
            return false;
        }

        public static bool IsActive(string eventName)
        {
            return (moonEventList.ContainsKey(eventName)) && moonEventList[eventName].IsActive;
        }
    }
}
