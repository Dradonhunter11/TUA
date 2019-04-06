using Terraria;
using System;
using TUA.API.EventManager;
using System.Linq;

namespace TUA.Discord
{
    public class DRPBossMessage
	{
        public readonly string Header;
        public readonly string Message;
        private readonly Func<bool> CanCallFunc;

        public DRPBossMessage(string header, string message, Func<bool> ccf = null)
        {
            Header = header;
            Message = message;
            CanCallFunc = ccf ?? 
                delegate { return !Main.npc.Any(i => i.boss) 
                            && !MoonEventManagerWorld.moonEventList.Any(i => i.Value.IsActive); };
        }

        public bool CanCall() => CanCallFunc();
    }
}
