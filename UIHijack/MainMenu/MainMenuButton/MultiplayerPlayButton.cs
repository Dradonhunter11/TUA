using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TUA.UIHijack.MainMenu.MainMenuButton
{
    class MultiplayerPlayButton : MenuButton
    {
        public MultiplayerPlayButton(int xPosition, int yPosition) : base("Multiplayer", xPosition, yPosition)
        {

        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 12;
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }
    }
}
