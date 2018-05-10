using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse
{
    public static class ModifyPath 
    {
        public static String patch = ModLoader.ModSourcePath;

        public static void modifyModPath() {
            patch += "/Tapocalypse";
        }
    }
}
