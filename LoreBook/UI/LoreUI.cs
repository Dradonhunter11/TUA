﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;
using TUA.API.UI;
using TUA.Localization;

namespace TUA.LoreBook.UI
{
    class LoreUI : UIState
    {
        internal LorePlayer instance;
        internal CustomizableUIPanel mainPanel;

        internal CustomizableUIPanel selectionPanel;

        internal UIList entryList;
        internal UIScrollbar scrollbar;
        internal UIElement xButton = new UIElement();

        internal Texture2D xButtonTexture;

        internal bool InLoreEntry = false;

        internal List<LoreEntry> entriesList;

        public LoreUI(LorePlayer instance)
        {
            this.instance = instance;
            entriesList = new List<LoreEntry>();
        }

        public override void OnInitialize()
        {
            xButtonTexture = TUA.instance.GetTexture("Texture/X_ui");

            xButton.Width.Set(20f, 0f);
            xButton.Height.Set(22f, 0f);
            xButton.Left.Set(Main.screenWidth / 2f + 220f, 0f);
            xButton.Top.Set(Main.screenHeight / 2f - 330f, 0f);
            xButton.OnClick += CloseButtonClicked;

            mainPanel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));
            mainPanel.Width.Set(400, 0);
            mainPanel.Height.Set(600, 0);
            mainPanel.Left.Set(Main.screenWidth / 2 - 200, 0);
            mainPanel.Top.Set(Main.screenHeight / 2 - 300, 0);
            mainPanel.SetPadding(0);

            selectionPanel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));
            selectionPanel.Width.Set(400, 0);
            selectionPanel.Height.Set(600, 0);
            selectionPanel.Left.Set(0, 0);
            selectionPanel.Top.Set(0, 0);
            selectionPanel.SetPadding(0);

            entryList = new UIList();
            entryList.Left.Set(5f, 0);
            entryList.Top.Set(30f, 0);
            entryList.Width.Set(350f, 0);
            entryList.Height.Set(550f, 0);

            scrollbar = new UIScrollbar();
            scrollbar.Top.Set(-50f, 0);
            scrollbar.Left.Set(50f, 0f);
            scrollbar.HAlign = 1f;

            entryList.SetScrollbar(scrollbar);

            selectionPanel.Append(entryList);
            //mainPanel.Append(selectionPanel);
            mainPanel.Append(selectionPanel);
            Append(xButton);
            Append(mainPanel);
        }

        public void Add(UIElement item)
        {
            entryList._items.Add(item);
            UIElement _innerList = (UIElement) typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(entryList);
            _innerList.Append(item);
            _innerList.Recalculate();
            typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(entryList, _innerList);
        }

        public void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            TUA.loreInterface.IsVisible = false;
            TUA.loreInterface.SetState(null);
        }

        public void SwitchToEntry(UIMouseEvent evt, UIElement listeningElement)
        {
            mainPanel.RemoveAllChildren();
            if (listeningElement is UIText text)
            {
                LoreEntry entry = entriesList.Single(i => i.Title == text.Text);
                mainPanel.Append(entry.Panel);
            }
        }

        
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            
            CalculatedStyle style = mainPanel.GetInnerDimensions();
            Vector2 textSize = ChatManager.GetStringSize(Main.fontDeathText, LocalizationManager.instance.GetTranslation("TUA.UI.LoreTitle"), new Vector2(1f, 1f));
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, LocalizationManager.instance.GetTranslation("TUA.UI.LoreTitle"),
                Main.screenWidth / 2 - textSize.X / 2, Main.screenHeight / 2 - 350, Color.LightGray,
                Color.Black, Vector2.Zero, 1f);
            spriteBatch.Draw(xButtonTexture, xButton.GetInnerDimensions().Position(), Color.White);
            base.DrawChildren(spriteBatch);
        }


    }

    class LoreEntry
    {
        public CustomizableUIPanel Panel;
        public UIText Content;
        public string Title;
        public UIScrollbar scrollbar;
        

        public Func<bool> unlocked = () => true;
        public string Name => (unlocked.Invoke()) ? Title : "???";
        public Texture2D topPicture = null;

        public Dictionary<int, LorePage> pages;

        public LoreEntry(UIElement backPanel, CustomizableUIPanel source, string title, string content, Func<bool> condition = null)
        {
            pages = new Dictionary<int, LorePage>();

            Panel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));
            Panel.Width.Set(400, 0);
            Panel.Height.Set(600, 0);
            Panel.Left.Set(0, 0);
            Panel.Top.Set(0, 0);

            Content = new UIText(content); 

            Initialize();

            
        }

        private void Initialize()
        {

        }



    }

    class LorePage
    {
        public Texture2D texture = null;
        public int lineNumber;
        public string text;

        public int pageID;
        public const int WIDTH = 350;
        public const int HEIGHT = 550;
        public Vector2 position;

        public LorePage(int pageID, string[] text, Vector2 position, Texture2D texture = null)
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
