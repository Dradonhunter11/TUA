using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu
{
    class ModSettingsUI : UIState
    {
        private static List<MenuButton> modButtonList = new List<MenuButton>();
        private static int listIndex = 0;


        public override void OnInitialize()
        {
            
            UIElement overhaul = new UIElement();
            overhaul.OnClick += ExecuteOverhaulButton;
            UIElement tua = new UIElement();
            tua.OnClick += ExecuteTUAButton;
            UIElement back = new UIElement();
            back.OnClick += BackButton;
            if (ModLoader.GetMod("TerrariaOverhaul") != null)
            {
                AddMenuButton(overhaul, "Overhaul Settings");
            }
            AddMenuButton(tua, "Terraria Ultra Apocalypse Setting (W.I.P)");
            AddMenuButton(back, "back");
            initialize();
        }

        public void initialize()
        {
            foreach (MenuButton menuButton in modButtonList)
            {
                Append(menuButton);
            }
        }

        public static void AddMenuButton(UIElement ui, string name)
        {
            
            MenuButton newSettingButton = new MenuButton(name,0 ,0).isList(listIndex);

            Delegate del = null;
            FieldInfo fi = ui.GetType().GetField("OnClick",
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy |
                BindingFlags.IgnoreCase);

            if (fi != null)
            {
                Object value = fi.GetValue(ui);
                if (value is Delegate)
                {
                    del = (Delegate) value;
                    newSettingButton.OnClick += (MouseEvent)del;
                }

            }
            
            modButtonList.Add(newSettingButton);
            listIndex++;
        }


        public void ExecuteOverhaulButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 43300;
            Main.PlaySound(10, -1, -1, 1);
        }

        public void ExecuteTUAButton(UIMouseEvent evt, UIElement element)
        {

        }

        public void BackButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 0;
            Main.PlaySound(SoundID.MenuClose, -1, -1, 1);
        }
    }
}
