using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;
using TUA.API;
using TUA.API.UI;
using TUA.Localization;
using TUA.UI;

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

        private List<LoreEntry> entriesList;

        public void InitLoreUI(LorePlayer instance)
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

        internal void SetMainPanel(CustomizableUIPanel panel = null)
        {
            mainPanel.RemoveAllChildren();
            mainPanel.Append(panel ?? selectionPanel);
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

        public LoreEntry(UIElement backPanel, CustomizableUIPanel source, string title, string content, Texture2D texture = null, bool TextureOnAllPage = false, Func<bool> condition = null)
        {
            pages = new Dictionary<int, LorePage>();

            Panel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));
            Panel.Width.Set(400, 0);
            Panel.Height.Set(600, 0);
            Panel.Left.Set(0, 0);
            Panel.Top.Set(0, 0);

            Content = new UIText(content); 

            Initialize(content, texture, TextureOnAllPage);
        }

        private void Initialize(string content, Texture2D texture, bool TextureOnAllPage)
        {
            int lineAmount = 0;
            List<string> contentPerLine = Utils.WordwrapString(content, Main.fontDeathText, 400, 999, out lineAmount).ToList();
            string[] pageContent = new string[(texture != null) ? 10 : 15];
            int pageID = 0;
            for (int i = 0; i < contentPerLine.Count; i++)
            {
                pageContent[i % pageContent.Length] = contentPerLine[i];
                if (pageContent.IsFull())
                {
                    pages.Add(pageID, new LorePage(pageID, pageContent, (TextureOnAllPage || pageID == 0 && texture != null) ? texture : null));
                    pageID++;
                    pageContent = (TextureOnAllPage) ? new string[10] : new string[15];
                }
            }
        }

        public void NextPage(UIMouseEvent evt, UIElement targetElement)
        {
            int tempPageIndex = currentPageIndex + 1;
            if (!pages.ContainsKey(tempPageIndex))
            {
                return;
            }

            currentPageIndex++;
        }

        public void PreviousPage(UIMouseEvent evt, UIElement targetElement)
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

        private void Draw(SpriteBatch sb)
        {
            pages[currentPageIndex].Draw(sb, UIManager.GetLoreInstance().GetInnerDimensions().Position());
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
                if (texture.Width > 128 || texture.Height > 128)
                {
                    scale = new Vector2(128 / texture.Width, 128 / texture.Height);
                }

                Vector2 texturePosition = new Vector2(128 / 2 - texture.Width * scale.X / 2,
                    128 / 2 - texture.Height * scale.Y / 2);
                sb.Draw(texture, drawingPosition + texturePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }

            int stringY = (texture != null) ? 128 + 15 : 15;
            foreach (string str in text)
            {
                Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, str, drawingPosition.X, stringY, Color.White, Color.Black, Vector2.Zero, 0.5f);
                stringY += 12;
            }
        }
    }
}
