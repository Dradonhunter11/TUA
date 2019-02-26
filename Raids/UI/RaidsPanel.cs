using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace TUA.Raids.UI
{
    class RaidsPanel : UIPanel
    {
        internal byte RaidsType { get;  }
        private bool highlight = false;


        public RaidsPanel(byte raid)
        {
            this.RaidsType = raid;
        }

        public String GetRaidsName()
        {
            return RaidsID.raidsName[RaidsType];
        }

        public override void OnInitialize()
        {

        }

        public override void Click(UIMouseEvent evt)
        {
            if (this.Parent.Parent.Parent.Parent is RaidsUI raidUI)
            {
                if (raidUI.currentlySelectedRaids != null)
                {
                    raidUI.previousRaidsPanel = raidUI.currentlySelectedRaids;
                    raidUI.previousRaidsPanel.highlight = false;
                }
                raidUI.currentlySelectedRaids = this;
                highlight = true;
            }
            base.Click(evt);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle style = GetInnerDimensions();

            if (highlight)
            {
                BackgroundColor = Color.Blue * 0.8f;
            }
            else
            {
                BackgroundColor = Color.White * 0.2f;
            }

            if (IsMouseHovering)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, GetRaidsName(),
                    new Vector2(style.X - 5f, style.Y - 5f), Color.Yellow, 0f, Vector2.Zero, Vector2.One);
            }
            else
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, GetRaidsName(),
                    new Vector2(style.X - 5f, style.Y - 5f), Color.White, 0f, Vector2.Zero, Vector2.One);
            }
        }
    }
}
