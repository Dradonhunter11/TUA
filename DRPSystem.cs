using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using drpc;
using drpc.drpc;
using log4net;
using Terraria;
using Terraria.ModLoader.IO;

namespace TUA
{
    // Almost all of this is from Overhaul.
    public static class DRPSystem
    {
        private const string AppID = "528086919670792233";

        private static DiscordRP.RichPresence presence;

        private static DiscordRP.EventHandlers handlers;

        private static Assembly asm;

        public static void Init()
        {

            handlers = new DiscordRP.EventHandlers();
            //asm.GetType("DiscordRP").GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { AppID, handlers, true, (string)null});
            DiscordRP.Initialize(AppID, ref handlers, true, (string)null);
            Reset(true);
            Update();
            
        }

        public static void Update()
        {
            if (Main.gameMenu & Main.rand.NextBool(100))
            {
                Reset(false);
                return;
            }


            // https://discordapp.com/developers/docs/rich-presence/how-to#updating-presence-update-presence-payload-fields
            /*
            if (!NPC.downedSlimeKing | !NPC.downedBoss1)
            {
                presence.smallImageText = "Tier 0: Pre Eye of Cthulhu/King Slime";
            }
            if (NPC.downedBoss1 & NPC.downedSlimeKing)
            {
                presence.smallImageText = "Tier 1: Eye of Cthulhu/King Slime";
            }
            if (NPC.downedBoss2)
            {
                presence.smallImageText = Main.ActiveWorldFileData.HasCrimson ? 
                    "Tier 2: Brain of Cthulhu"
                    : "Tier 2: Eater of Worlds";
            }
            if (NPC.downedBoss3)
            {
                presence.smallImageText = "Tier 3: Skeletron";
            }
            if (Main.hardMode)
            {
                presence.smallImageText = "Tier 4: Hardmode";
            }
            if (NPC.downedPlantBoss)
            {
                presence.smallImageText = "Tier 5: Plantera";
            }
            */
        }

        public static void Kill()
        {
            DiscordRP.Shutdown();
        }

        private static void Reset(bool timestamp)
        {
            presence.largeImageText = "TUA";
            presence.largeImageKey = "logo";
            presence.smallImageText = null;
            presence.smallImageKey = null;
            presence.details = "In Main Menu " + ((Environment.Is64BitProcess) ? "(64bit)" : "(32bit)");
            presence.state = null; 
            if (timestamp)
            {
                presence.startTimestamp = Convert.ToInt64((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            }
            DiscordRP.UpdatePresence(ref presence);
        }
    }
}
