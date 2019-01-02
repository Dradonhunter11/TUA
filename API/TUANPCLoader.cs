using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API
{
    public static class TUANPCLoader
    {
        private static List<TUAGlobalNPC> classHook = new List<TUAGlobalNPC>();

        public static void addTUAGlobalNPC(TUAGlobalNPC newClass)
        {
            classHook.Add(newClass);
        }

        public static void ModifyNPCDialogButton(NPC npc, ref string button, ref string button2)
        {
            foreach (var tuaGlobalNpc in classHook)
            {
                tuaGlobalNpc.modifyNPCButtonChat(npc, ref button, ref button2);
            }
        }
    }
}
