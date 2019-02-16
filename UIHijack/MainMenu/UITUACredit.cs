using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using TUA.UIHijack.MainMenu.MainMenuButton;

namespace TUA.UIHijack.MainMenu
{
    internal class UITUACredit : UIState
    {
        private UIList list;
        private UIScrollbar scrollbar;

        private static readonly List<string> devList = new List<string>();
        private static readonly List<string> donator = new List<string>();
        private static readonly List<string> patreon = new List<string>();

        public override void OnInitialize()
        {
            Width.Set(600, 0f);
            Height.Set(600, 0f);
            Left.Set(Main.screenWidth / 2 - 300, 0f);
            Top.Set(Main.screenHeight / 2 - 150, 0f);

            UITextPanel<LocalizedText> backButtonTestPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
            backButtonTestPanel.Width.Set(200, 0f);
            backButtonTestPanel.Height.Set(150, 0);
            backButtonTestPanel.Left.Set(Main.screenWidth - 100, 0);
            backButtonTestPanel.Top.Set(Main.screenHeight / 2 - 250, 0);
            backButtonTestPanel.OnClick += backButtonEvent;

            list = new UIList();
            scrollbar = new UIScrollbar();
            scrollbar.Height.Set(-35, 1f);

            Append(backButtonTestPanel);
        }

        public void backButtonEvent(UIMouseEvent MouseEvent, UIElement element)
        {
            Main.MenuUI.SetState(TerrariaUltraApocalypse.instance.newMainMenu);
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }


    }
}
