using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;
using TUA.API;
using TUA.API.UI;
using TUA.Localization;
using TUA.Utilities;

namespace TUA.LoreBook.UI
{
    class LoreUI : UIState
    {
        private LorePlayer instance;
        private CustomizableUIPanel mainPanel;

        private CustomizableUIPanel selectionPanel;

        private UIList entryList;
        private UIScrollbar scrollbar;
        private readonly UIElement xButton = new UIElement();

        private Texture2D xButtonTexture;

        private bool InLoreEntry = false;
        private string CurrentEntryName = "";

        private readonly List<LoreEntry> entriesList;

        public LoreUI()
        {
            entriesList = new List<LoreEntry>();
        }

        public void InitLoreUI(LorePlayer instance)
        {
            this.instance = instance;
        }

        public override void OnInitialize()
        {
            xButtonTexture = TUA.instance.GetTexture("Texture/X_ui");

            xButton.Width.Set(20f, 0f);
            xButton.Height.Set(22f, 0f);
            xButton.Left.Set(Main.screenWidth / 2f + 220f, 0f);
            xButton.Top.Set(Main.screenHeight / 2f - 330f, 0f);
            xButton.OnClick += (evt, listElem) => UIManager.CloseLoreUI();

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

            AddEntry("Dummy", "I'm the guide, I'm dummy", TUA.instance.GetTexture("Texture/LoreUI/Guide"), false);
            AddEntry("The heart of the wasteland", "1000 year ago, scientist tried to explore the wasteland after the accident, most of them never came back. As the time went, they saw an amalgamate forming in the middle in the highly radioactive environment. That amalgamate was formed with the body of the dead scientist that once explored the wasteland. \n \n \n//FIX ME - Any developer", TUA.instance.GetTexture("Texture/LoreUI/WastelandCore"), false);

            selectionPanel.Append(entryList);
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

        public void SwitchToEntry(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement is UIText text)
            {
                LoreEntry entry = entriesList.Single(i => i.Title == text.Text);
                CurrentEntryName = entry.Title;
                SetMainPanel(entry.Panel);
                InLoreEntry = true;
            }
        }

        
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {

            base.DrawChildren(spriteBatch);
            CalculatedStyle style = mainPanel.GetInnerDimensions();
            Vector2 textSize = ChatManager.GetStringSize(Main.fontDeathText, LocalizationManager.instance.GetTranslation("TUA.UI.LoreTitle"), new Vector2(1f, 1f));
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, LocalizationManager.instance.GetTranslation("TUA.UI.LoreTitle"),
                Main.screenWidth / 2 - textSize.X / 2, Main.screenHeight / 2 - 350, Color.LightGray,
                Color.Black, Vector2.Zero, 1f);
            spriteBatch.Draw(xButtonTexture, xButton.GetInnerDimensions().Position(), Color.White);
            if (InLoreEntry)
            {
                entriesList.Single(i => i.Title == CurrentEntryName).Draw(spriteBatch, mainPanel.GetInnerDimensions().Position());
            }

        }

        internal void SetMainPanel(CustomizableUIPanel panel = null)
        {
            mainPanel.RemoveAllChildren();
            mainPanel.Append(panel ?? selectionPanel);
            if (panel == null)
            {
                InLoreEntry = false;
            }
        }

        internal void AddEntry(string title, string content, Texture2D texture = null, bool allPage = false, Func<bool> condition = null)
        {
            LoreEntry entry = new LoreEntry(title, content, texture, allPage, condition);
            entriesList.Add(entry);

            UIText text = new UIText(entry.Title);
            text.OnClick += SwitchToEntry;
            Add(text);
        }
    }

    class LoreEntry
    {
        public CustomizableUIPanel Panel;
        public UIText Content;
        public string Title;        

        public Func<bool> unlocked = () => true;
        public string Name => unlocked.Invoke() ? Title : "???";

        public Dictionary<int, LorePage> pages;

        public int currentPageIndex = 0;

        public UIElement back;
        public UIElement next;

        public Texture2D arrow; //28x21

        public LoreEntry(string title, string content, Texture2D texture = null, bool TextureOnAllPage = false, Func<bool> condition = null)
        {
            pages = new Dictionary<int, LorePage>();

            arrow = TUA.instance.GetTexture("Texture/UI/Arrow");

            Panel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));
            Panel.Width.Set(400, 0);
            Panel.Height.Set(600, 0);
            Panel.Left.Set(0, 0);
            Panel.Top.Set(0, 0);

            back = new UIElement();
            back.Width.Set(28, 0);
            back.Height.Set(21, 0);
            back.VAlign = 1;
            back.Top.Set(-5, 0);
            back.Left.Set(0, 0);
            back.OnClick += PreviousPage;

            next = new UIElement();
            next.Width.Set(28, 0);
            next.Height.Set(21, 0);
            next.HAlign = 1;
            next.VAlign = 1;
            next.Top.Set(-5, 0);
            next.Left.Set(5, 0);
            next.OnClick += NextPage; 

            Panel.Append(back);
            Panel.Append(next);

            this.Title = title;

            Initialize(content, texture, TextureOnAllPage);
        }

        private void Initialize(string content, Texture2D texture, bool TextureOnAllPage)
        {
            int lineAmount = 0;
            List<string> contentPerLine = Utils.WordwrapString(content, Main.fontDeathText, 600, 999, out lineAmount).ToList();
            string[] pageContent = new string[(texture != null) ? 10 : 15];
            int pageID = 0;
            for (int i = 0; contentPerLine[i] != null; i++)
            {
                pageContent[i % pageContent.Length] = contentPerLine[i];
                if (pageContent.IsFull() || contentPerLine[i + 1] == null)
                {
                    pages.Add(pageID, new LorePage(pageID, pageContent, (TextureOnAllPage || pageID == 0 && texture != null) ? texture : null));
                    pageID++;
                    pageContent = (TextureOnAllPage) ? new string[10] : new string[15];
                }
            }
        }

        internal void NextPage(UIMouseEvent evt, UIElement targetElement)
        {
            int tempPageIndex = currentPageIndex + 1;
            if (!pages.ContainsKey(tempPageIndex))
            {
                return;
            }

            currentPageIndex++;
        }

        internal void PreviousPage(UIMouseEvent evt, UIElement targetElement)
        {
            if (currentPageIndex == 0)
            {
                UIManager.GetLoreInstance().SetMainPanel();
            }
            else
            {
                currentPageIndex--;
            }
        }

        internal void Draw(SpriteBatch sb, Vector2 mainPanelPosition)
        {
            CalculatedStyle backArrow = back.GetInnerDimensions();
            CalculatedStyle nextArrow = next.GetInnerDimensions();

            sb.Draw(arrow, nextArrow.Position(), new Rectangle(0, 0, 28, 21), next.IsMouseHovering ?  Color.White : Color.White * 0.5f);
            sb.Draw(arrow, backArrow.Position(), new Rectangle(0, 21, 28, 21), back.IsMouseHovering ? Color.White : Color.White * 0.5f);

            pages[currentPageIndex].Draw(sb, mainPanelPosition);
        }
    }

    class LorePage
    {
        public Texture2D texture;
        public int lineNumber;
        public string[] text;

        public int pageID;
        public const int WIDTH = 350;
        public const int HEIGHT = 550;

        public LorePage(int pageID, string[] text, Texture2D texture = null)
        {
            this.text = text;
            this.texture = texture;
        }


        // TODO : PICTURE WILL BE RESIZED TO 128x128, done
        public void Draw(SpriteBatch sb, Vector2 drawingPosition)
        {
            if (texture != null)
            {
                Vector2 scale = Vector2.One;
                float scaleX = 1;
                float scaleY = 1;
                if (texture.Width > 380)
                {
                    scaleX = 380f / texture.Width;
                }

                if (texture.Height > 200)
                {
                    scaleY = 200f / texture.Height;
                }

                Vector2 texturePosition = new Vector2(380 / 2 - texture.Width * scaleX / 2,
                    200 / 2 - texture.Height * scaleY / 2);
                sb.Draw(texture, drawingPosition + texturePosition, null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 1f);
            }

            int stringY = (texture != null) ? 200 + 20 : 20;
            foreach (string str in text)
            {
                if (str == null)
                    break;
                Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, str, drawingPosition.X + 20, drawingPosition.Y + stringY, Color.White, Color.Black, Vector2.Zero, 0.5f);
                stringY += 20;
            }

            
        }
    }
}
