using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TUA.UIHijack.MainMenu.MainMenuButton
{
    class ModsButton : MenuButton
    {
        public ModsButton(int xPosition, int yPosition) : base("Mods list", xPosition, yPosition)
        {
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 10000;
            Main.PlaySound(10, -1, -1, 1);
        }
    }
}
