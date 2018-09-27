using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu
{
    class UITUACredit : UIState
    {
        private UIList list;

        private static List<String> devList;
        private static List<String> donator;
        private static List<String> patreon;

        public override void OnInitialize()
        {
            list = new UIList();
            
        }


    }
}
