using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerrariaUltraApocalypse
{
    class TUAInterface : UIState
    {
        private UIElement area;
        private UITextPanel<string> hallowAltEnablerPanel;
        private UITextPanel<string> hallowAlt;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Width.Set(0, 0.8f);
            area.Height.Set(-210, 1f);
            area.Top.Set(200f, 0f);
            area.HAlign = 0.5f;

            hallowAltEnablerPanel = new UITextPanel<string>("Hallow alt : ", 1f, false);
            hallowAltEnablerPanel.Width.Set(10f, 0.8f);
            hallowAltEnablerPanel.Height.Set(20f, 1f);
            

            area.Append(hallowAltEnablerPanel);
        }
    }
}
