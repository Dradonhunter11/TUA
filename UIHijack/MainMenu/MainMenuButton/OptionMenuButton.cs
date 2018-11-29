using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton
{
    class OptionMenuButton : MenuButton
    {
        public OptionMenuButton(int xPosition, int yPosition) : base("Settings", xPosition, yPosition)
        {
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 11;
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }
    }
}
