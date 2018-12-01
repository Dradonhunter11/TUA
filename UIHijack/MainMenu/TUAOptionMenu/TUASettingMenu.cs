using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu.TUAOptionMenu
{
    class TUASettingMenu : UIState
    {
        private static List<MenuButton> optionList = new List<MenuButton>();

        public override void OnInitialize()
        {
            MenuButton creditButton = new MenuButton("credits",0 ,0);
            creditButton.isList(0);
            MenuButton backButton = new MenuButton("back", 0, 0);
            backButton.isList(1);
            backButton.OnClick += backButtonEvent;

            Append(creditButton);
            Append(backButton);
        }

        public void backButtonEvent(UIMouseEvent MouseEvent, UIElement element)
        {
            Main.MenuUI.SetState(TUAOptionMenuButton.settingUI);
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }
    }
}
