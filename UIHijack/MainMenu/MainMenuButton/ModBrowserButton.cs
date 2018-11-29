using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.UI;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton
{
    class ModBrowserButton : MenuButton
    {
        public ModBrowserButton(int xPosition, int yPosition) : base(Language.GetTextValue("tModLoader.MenuModBrowser"), xPosition, yPosition)
        {
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.PlaySound(10, -1, -1, 1);
            Main.menuMode = 10007;
        }
    }
}
