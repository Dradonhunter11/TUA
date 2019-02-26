﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;
using TUA.API;
using TUA.API.UI;
using TUA.Localization;

namespace TUA.Raids.UI
{
    internal class RaidsUI : UIState
    {
        private UIPanel mainPanel;
        private UIPanel descriptionPanel;

        private UIList raidsList;
        private UIScrollbar scrollbar;

        private UIScrollingText raidsDescription;

        private UITextPanel<TranslationWrapper> selectTextPanel;

        private UIElement xButton;

        internal RaidsPanel currentlySelectedRaids = new RaidsPanel(RaidsID.None);
        internal RaidsPanel previousRaidsPanel;

        private Texture2D xButtonTexture;

        public override void OnInitialize()
        {
            xButtonTexture = TerrariaUltraApocalypse.instance.GetTexture("Texture/X_ui");

            xButton = new UIElement();
            xButton.Width.Set(20f, 0f);
            xButton.Height.Set(22f, 0f);
            xButton.Left.Set(Main.screenWidth / 2f + 300f, 0f);
            xButton.Top.Set(Main.screenHeight /2f - 125f, 0f);
            xButton.OnClick += Close;

            selectTextPanel = new UITextPanel<TranslationWrapper>(LocalizationManager.instance.GetRawTranslation("TUA.UI.Select"), 0.5f, true)
            {
                DrawPanel = true,
                TextScale = 0.5f
            };
            selectTextPanel.Width.Set(100f, 0f);
            selectTextPanel.Left.Set(Main.screenWidth / 2f - selectTextPanel.Width.Pixels / 2, 0f);
            selectTextPanel.Top.Set(Main.screenHeight / 2f + 260f, 0f);
            selectTextPanel.OnClick += SetCurrentRaids;

            mainPanel = new UIPanel();
            mainPanel.Width.Set(300f, 0);
            mainPanel.Height.Set(350f, 0);
            mainPanel.Left.Set(Main.screenWidth / 2f - 305f, 0f);
            mainPanel.Top.Set(Main.screenHeight / 2f - 100f, 0f);

            descriptionPanel = new UIPanel();
            descriptionPanel.Width.Set(300f, 0);
            descriptionPanel.Height.Set(350f, 0);
            descriptionPanel.Left.Set(Main.screenWidth / 2f + 5f, 0f);
            descriptionPanel.Top.Set(Main.screenHeight / 2f - 100f, 0f);

            raidsDescription = new UIScrollingText();
            raidsDescription.Top.Set(2f, 0f);
            raidsDescription.Left.Set(5, 0f);
            raidsDescription.Height.Set(330f, 0f);
            raidsDescription.Width.Set(250f, 0f);
            raidsDescription.SetText("No raids are currently selected");
            descriptionPanel.Append(raidsDescription);

            scrollbar = new UIScrollbar();
            scrollbar.Height.Set(-5f, 1f);
            scrollbar.Top.Set(5, 0);
            scrollbar.SetView(100f, 100f);
            scrollbar.HAlign = 1f;

            raidsList = new UIList();
            raidsList.Top.Set(10f, 0f);
            raidsList.Left.Set(-5f, 0f);
            raidsList.Height.Set(300f, 0);
            raidsList.Width.Set(250f, 0f);
            raidsList.SetScrollbar(scrollbar);

            mainPanel.Append(raidsList);
            mainPanel.Append(scrollbar);

            Append(mainPanel);
            Append(descriptionPanel);
            Append(selectTextPanel);
            Append(xButton);
            AddRaids();

            #region Get Description In Shitcode Way
            StringBuilder builder = new StringBuilder();
            switch (currentlySelectedRaids.RaidsType)
            {
                case RaidsID.TheGreatHellRide:
                    builder.AppendLine("The great hell ride");
                    builder.AppendLine("Have you heard the legend of the great hell ride?");
                    builder.AppendLine("From what I've heard, it's a lot of fun and at the end you have to fight a giant wall!");
                    builder.AppendLine("Sadly, I've not ever seen the infamous wall and I'll probably never get to, but you can do something about that.");
                    builder.AppendLine("- The Guide");
                    break;
                case RaidsID.TheWrathOfTheWasteland:
                    builder.AppendLine("The wrath of the wasteland");
                    builder.AppendLine("Some weird vibration got emitted from the core of the wasteland, the hearth might become angry again.");
                    builder.AppendLine("Your goal is calm down the heart of the wasteland and make sure it doesn't wake up again.");
                    builder.AppendLine("But first, you'll need to do some task as you need specific stuffs to calm it down");
                    builder.AppendLine("- The Guide and the Infinity Traveler");
                    break;
                case RaidsID.TheEyeOfDestruction:
                    builder.AppendLine("The hunt beyond the void");
                    builder.AppendLine("A long time ago, I fought this god. He was one of the most powerful one I ever fought mainly because it could control the element of the void.");
                    builder.AppendLine("This eye, also known as the eye of apocalypse, got out of his sleep after you killed the Ultra eye of Cthulhu.");
                    builder.AppendLine("If he does what he did 1000 years ago, the world might be destroyed... Check the moon, because that's your biggest concern for now");
                    builder.AppendLine("- The Tnfinity Traveler");
                    break;
                case RaidsID.None:
                    builder.AppendLine("There are no active raids currently!");
                    break;
            }
            raidsDescription.SetText(builder.ToString());
            #endregion
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle style = raidsList.GetInnerDimensions();
            base.DrawSelf(spriteBatch);
            Vector2 textSize = ChatManager.GetStringSize(Main.fontMouseText, LocalizationManager.instance.GetTranslation("TUA.UI.RaidsSelect"), new Vector2(1.5f, 1.5f));
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, LocalizationManager.instance.GetTranslation("TUA.UI.RaidsSelect"),
                new Vector2(Main.screenWidth / 2 - textSize.X / 2, Main.screenHeight / 2 - 150), Color.White,
                0f, Vector2.Zero, new Vector2(1.5f, 1.5f));
            spriteBatch.Draw(xButtonTexture, xButton.GetInnerDimensions().Position(), Color.White);
            this.Recalculate();
        }

        private void AddRaids()
        {
            RaidsPanel rp;
            
            //if (Main.ActiveWorldFileData.HasCorruption)
            //{
                rp = new RaidsPanel(RaidsID.TheGreatHellRide);

            //}
            //else
            //{

                
            //}
            rp.Height.Set(30f, 0);
            rp.Width.Set(240, 0f);
            rp.Top.Set(5f, 0f);
            rp.Left.Set(5f, 0f);
            raidsList.Add(rp);
            rp = new RaidsPanel(RaidsID.TheWrathOfTheWasteland);
            rp.Height.Set(30f, 0);
            rp.Width.Set(240, 0f);
            rp.Top.Set(5f, 0f);
            rp.Left.Set(5f, 0f);
            raidsList.Add(rp);
            rp = new RaidsPanel(RaidsID.TheEyeOfDestruction);
            rp.Height.Set(30f, 0);
            rp.Width.Set(240, 0f);
            rp.Top.Set(5f, 0f);
            rp.Left.Set(5f, 0f);
            raidsList.Add(rp);
        }

        public override void Update(GameTime gameTime)
        {
            selectTextPanel.TextColor = selectTextPanel.IsMouseHovering ? Color.Yellow : Color.White;
        }

        public void SetCurrentRaids(UIMouseEvent evt, UIElement el)
        {
            if (currentlySelectedRaids.RaidsType == 0)
            {
                return;
            }
            RaidsWorld.currentRaid = currentlySelectedRaids.RaidsType;
            BaseUtility.Chat(Main.LocalPlayer.name + " has started [" + RaidsID.raidsName[currentlySelectedRaids.RaidsType] + "] raids!");
        }

        public void Close(UIMouseEvent evt, UIElement el)
        {
            TerrariaUltraApocalypse.raidsInterface.IsVisible = false;
            Main.npcChatText = "I'll be able to help you in your future raids! After all, I'm the guide." 
                + (Main.rand.NextBool() ? " :smile:" : "");
        }
    }
}
