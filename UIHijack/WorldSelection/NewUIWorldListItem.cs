using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ReLogic.OS;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace TerrariaUltraApocalypse.UIHijack.WorldSelection
{
    public class NewUIWorldListItem : UIPanel
    {
        private WorldFileData _data;
        private Texture2D _dividerTexture;
        private Texture2D _innerPanelTexture;
        private UIImage _worldIcon;
        private UIText _buttonLabel;
        private UIText _deleteButtonLabel;
        private Texture2D _buttonCloudActiveTexture;
        private Texture2D _buttonCloudInactiveTexture;
        private Texture2D _buttonFavoriteActiveTexture;
        private Texture2D _buttonFavoriteInactiveTexture;
        private Texture2D _buttonPlayTexture;
        private Texture2D _buttonSeedTexture;
        private Texture2D _buttonDeleteTexture;
        private UIImageButton _deleteButton;

        private TagCompound _TUAWorldData;
        private TagCompound _SacredToolWorldData;
        private TagCompound _EnigmaModData;
        private TagCompound _CalamityWorldData;
        private TagCompound _FargosModWorldData;

        private Dictionary<string, object> worldData = new Dictionary<string, object>(); // World data name and it's value

        public bool IsFavorite
        {
            get
            {
                return this._data.IsFavorite;
            }
        }

        public NewUIWorldListItem(WorldFileData data, int snapPointIndex)
        {
            try
            {
                string path = Path.ChangeExtension(data.Path, ".twld");
                if (File.Exists(path))
                {
                    var buf = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
                    var tag = TagIO.FromStream(new MemoryStream(buf));
                    _TUAWorldData = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "TerrariaUltraApocalypse" && m.Get<string>("name") == "TUAWorld");
                    _EnigmaModData = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "Laugicality" && m.Get<string>("name") == "LaugicalityWorld");
                    _CalamityWorldData = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "CalamityMod" && m.Get<string>("name") == "CalamityWorld");
                    _FargosModWorldData = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "Fargowiltas" && m.Get<string>("name") == "FargoWorld");
                    //_SacredToolWorldData = tag.GetList<TagCompound>("modData").FirstOrDefault(((TagCompound m) => m.GetString("mod") == "SacredTools"))
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Log(e.Message);
            }

            this._data = data;
            this.LoadTextures();
            this.InitializeAppearance();
            this._worldIcon = new UIImage(this.GetIcon());
            this._worldIcon.Left.Set(4f, 0f);
            this._worldIcon.OnDoubleClick += new UIElement.MouseEvent(this.PlayGame);
            base.Append(this._worldIcon);
            float num = 4f;
            UIImageButton uIImageButton = new UIImageButton(this._buttonPlayTexture);
            uIImageButton.VAlign = 1f;
            uIImageButton.Left.Set(num, 0f);
            uIImageButton.OnClick += new UIElement.MouseEvent(this.PlayGame);
            base.OnDoubleClick += new UIElement.MouseEvent(this.PlayGame);
            uIImageButton.OnMouseOver += new UIElement.MouseEvent(this.PlayMouseOver);
            uIImageButton.OnMouseOut += new UIElement.MouseEvent(this.ButtonMouseOut);
            base.Append(uIImageButton);
            num += 24f;
            UIImageButton uIImageButton2 = new UIImageButton(this._data.IsFavorite ? this._buttonFavoriteActiveTexture : this._buttonFavoriteInactiveTexture);
            uIImageButton2.VAlign = 1f;
            uIImageButton2.Left.Set(num, 0f);
            uIImageButton2.OnClick += new UIElement.MouseEvent(this.FavoriteButtonClick);
            uIImageButton2.OnMouseOver += new UIElement.MouseEvent(this.FavoriteMouseOver);
            uIImageButton2.OnMouseOut += new UIElement.MouseEvent(this.ButtonMouseOut);
            uIImageButton2.SetVisibility(1f, this._data.IsFavorite ? 0.8f : 0.4f);
            base.Append(uIImageButton2);
            num += 24f;
            if (SocialAPI.Cloud != null)
            {
                UIImageButton cloudUiImageButton = new UIImageButton(this._data.IsCloudSave ? this._buttonCloudActiveTexture : this._buttonCloudInactiveTexture);
                cloudUiImageButton.VAlign = 1f;
                cloudUiImageButton.Left.Set(num, 0f);
                cloudUiImageButton.OnClick += new UIElement.MouseEvent(this.CloudButtonClick);
                cloudUiImageButton.OnMouseOver += new UIElement.MouseEvent(this.CloudMouseOver);
                cloudUiImageButton.OnMouseOut += new UIElement.MouseEvent(this.ButtonMouseOut);
                cloudUiImageButton.SetSnapPoint("Cloud", snapPointIndex, null, null);
                base.Append(cloudUiImageButton);
                num += 24f;
            }
            if (Main.UseSeedUI && this._data.WorldGeneratorVersion != 0uL)
            {
                UIImageButton uIImageButton4 = new UIImageButton(this._buttonSeedTexture);
                uIImageButton4.VAlign = 1f;
                uIImageButton4.Left.Set(num, 0f);
                uIImageButton4.OnClick += new UIElement.MouseEvent(this.SeedButtonClick);
                uIImageButton4.OnMouseOver += new UIElement.MouseEvent(this.SeedMouseOver);
                uIImageButton4.OnMouseOut += new UIElement.MouseEvent(this.ButtonMouseOut);
                uIImageButton4.SetSnapPoint("Seed", snapPointIndex, null, null);
                base.Append(uIImageButton4);
                num += 24f;
            }
            UIImageButton uIImageButton5 = new UIImageButton(this._buttonDeleteTexture);
            uIImageButton5.VAlign = 1f;
            uIImageButton5.HAlign = 1f;
            uIImageButton5.OnClick += new UIElement.MouseEvent(this.DeleteButtonClick);
            uIImageButton5.OnMouseOver += new UIElement.MouseEvent(this.DeleteMouseOver);
            uIImageButton5.OnMouseOut += new UIElement.MouseEvent(this.DeleteMouseOut);
            this._deleteButton = uIImageButton5;
            if (!this._data.IsFavorite)
            {
                base.Append(uIImageButton5);
            }
            num += 4f;
            this._buttonLabel = new UIText("", 1f, false);
            this._buttonLabel.VAlign = 1f;
            this._buttonLabel.Left.Set(num, 0f);
            this._buttonLabel.Top.Set(-3f, 0f);
            base.Append(this._buttonLabel);
            this._deleteButtonLabel = new UIText("", 1f, false);
            this._deleteButtonLabel.VAlign = 1f;
            this._deleteButtonLabel.HAlign = 1f;
            this._deleteButtonLabel.Left.Set(-30f, 0f);
            this._deleteButtonLabel.Top.Set(-3f, 0f);
            base.Append(this._deleteButtonLabel);
            uIImageButton.SetSnapPoint("Play", snapPointIndex, null, null);
            uIImageButton2.SetSnapPoint("Favorite", snapPointIndex, null, null);
            uIImageButton5.SetSnapPoint("Delete", snapPointIndex, null, null);
            WorldPreLoader.loadWorld(_data.Path,_data.IsCloudSave, worldData, data);
        }

        private void LoadTextures()
        {
            this._dividerTexture = TextureManager.Load("Images/UI/Divider");
            this._innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");
            this._buttonCloudActiveTexture = TextureManager.Load("Images/UI/ButtonCloudActive");
            this._buttonCloudInactiveTexture = TextureManager.Load("Images/UI/ButtonCloudInactive");
            this._buttonFavoriteActiveTexture = TextureManager.Load("Images/UI/ButtonFavoriteActive");
            this._buttonFavoriteInactiveTexture = TextureManager.Load("Images/UI/ButtonFavoriteInactive");
            this._buttonPlayTexture = TextureManager.Load("Images/UI/ButtonPlay");
            this._buttonSeedTexture = TextureManager.Load("Images/UI/ButtonSeed");
            this._buttonDeleteTexture = TextureManager.Load("Images/UI/ButtonDelete");
        }

        private void InitializeAppearance()
        {
            this.Height.Set(150f, 0f);
            this.Width.Set(0f, 1f);
            base.SetPadding(6f);
            this.BorderColor = new Color(89, 116, 213) * 0.7f;
        }

        private Texture2D GetIcon()
        {
            return TextureManager.Load("Images/UI/Icon" + (this._data.IsHardMode ? "Hallow" : "") + (this._data.HasCorruption ? "Corruption" : "Crimson"));
        }

        private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            if (this._data.IsFavorite)
            {
                this._buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
                return;
            }
            this._buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
        }

        private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            if (this._data.IsCloudSave)
            {
                this._buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
                return;
            }
            this._buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
        }

        private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            this._buttonLabel.SetText(Language.GetTextValue("UI.Play"));
        }

        private void SeedMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            this._buttonLabel.SetText(Language.GetTextValue("UI.CopySeed", this._data.SeedText));
        }

        private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            this._deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
        }

        private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            this._deleteButtonLabel.SetText("");
        }

        private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            this._buttonLabel.SetText("");
        }

        private void CloudButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (this._data.IsCloudSave)
            {
                this._data.MoveToLocal();
            }
            else
            {
                this._data.MoveToCloud();
            }
            ((UIImageButton)evt.Target).SetImage(this._data.IsCloudSave ? this._buttonCloudActiveTexture : this._buttonCloudInactiveTexture);
            if (this._data.IsCloudSave)
            {
                this._buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
                return;
            }
            this._buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
        }

        private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < Main.WorldList.Count; i++)
            {
                if (Main.WorldList[i] == this._data)
                {
                    Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                    Main.selectedWorld = i;
                    Main.menuMode = 9;
                    return;
                }
            }
        }

        private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement != evt.Target)
            {
                return;
            }
            this._data.SetAsActive();
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
            Main.GetInputText("");
            if (Main.menuMultiplayer && SocialAPI.Network != null)
            {
                Main.menuMode = 889;
            }
            else if (Main.menuMultiplayer)
            {
                Main.menuMode = 30;
            }
            else
            {
                Main.menuMode = 10;
            }
            if (!Main.menuMultiplayer)
            {
                WorldGen.playWorld();
            }
        }

        private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            this._data.ToggleFavorite();
            ((UIImageButton)evt.Target).SetImage(this._data.IsFavorite ? this._buttonFavoriteActiveTexture : this._buttonFavoriteInactiveTexture);
            ((UIImageButton)evt.Target).SetVisibility(1f, this._data.IsFavorite ? 0.8f : 0.4f);
            if (this._data.IsFavorite)
            {
                this._buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
                base.RemoveChild(this._deleteButton);
            }
            else
            {
                this._buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
                base.Append(this._deleteButton);
            }
            UIList uIList = this.Parent.Parent as UIList;
            if (uIList != null)
            {
                uIList.UpdateOrder();
            }
        }

        private void SeedButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Platform.Current.Clipboard = this._data.SeedText;
            this._buttonLabel.SetText(Language.GetTextValue("UI.SeedCopied"));
        }

        public override int CompareTo(object obj)
        {
            NewUIWorldListItem uIWorldListItem = obj as NewUIWorldListItem;
            if (uIWorldListItem == null)
            {
                return base.CompareTo(obj);
            }
            if (this.IsFavorite && !uIWorldListItem.IsFavorite)
            {
                return -1;
            }
            if (!this.IsFavorite && uIWorldListItem.IsFavorite)
            {
                return 1;
            }
            if (this._data.Name.CompareTo(uIWorldListItem._data.Name) != 0)
            {
                return this._data.Name.CompareTo(uIWorldListItem._data.Name);
            }
            return this._data.GetFileName(true).CompareTo(uIWorldListItem._data.GetFileName(true));
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            this.BackgroundColor = new Color(73, 94, 171);
            this.BorderColor = new Color(89, 116, 213);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            this.BorderColor = new Color(89, 116, 213) * 0.7f;
        }

        private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
        {
            spriteBatch.Draw(this._innerPanelTexture, position, new Rectangle?(new Rectangle(0, 0, 8, this._innerPanelTexture.Height)), Color.White);
            spriteBatch.Draw(this._innerPanelTexture, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this._innerPanelTexture.Height)), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(this._innerPanelTexture, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this._innerPanelTexture.Height)), Color.White);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
           
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            CalculatedStyle dimensions = this._worldIcon.GetDimensions();
            float num = dimensions.X + dimensions.Width;
            Color color = this._data.IsValid ? Color.White : Color.Red;
            Utils.DrawBorderString(spriteBatch, this._data.Name, new Vector2(num + 6f, dimensions.Y - 2f), color, 1f, 0f, 0f, -1);
            spriteBatch.Draw(this._dividerTexture, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((base.GetDimensions().X + base.GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
            Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
            float num2 = 100f;
            this.DrawPanel(spriteBatch, vector, num2);
            //string text = this._data.IsExpertMode ? Language.GetTextValue("UI.Expert") : Language.GetTextValue("UI.Normal");
            string text = this._data.IsExpertMode ? Language.GetTextValue("UI.Expert") : Language.GetTextValue("UI.Normal");
            float x = Main.fontMouseText.MeasureString(text).X;
            float x2 = num2 * 0.5f - x * 0.5f;
            //Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, 3f), this._data.IsExpertMode ? new Color(217, 143, 244) : Color.White, 1f, 0f, 0f, -1);
            Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, 3f), this._data.IsExpertMode ? new Color(217, 0, 0) : Color.White, 1f, 0f, 0f, -1);
            vector.X += num2 + 5f;
            float num3 = 150f;
            if (!GameCulture.English.IsActive)
            {
                num3 += 40f;
            }
            this.DrawPanel(spriteBatch, vector, num3);
            string textValue = Language.GetTextValue("UI.WorldSizeFormat", this._data.WorldSizeName);
            float x3 = Main.fontMouseText.MeasureString(textValue).X;
            float x4 = num3 * 0.5f - x3 * 0.5f;
            Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, 3f), Color.White, 1f, 0f, 0f, -1);
            vector.X += num3 + 5f;
            float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
            this.DrawPanel(spriteBatch, vector, num4);
            string arg;
            if (GameCulture.English.IsActive)
            {
                arg = this._data.CreationTime.ToString("d MMMM yyyy");
            }
            else
            {
                arg = this._data.CreationTime.ToShortDateString();
            }
            string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
            float x5 = Main.fontMouseText.MeasureString(textValue2).X;
            float x6 = num4 * 0.5f - x5 * 0.5f;
            Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White, 1f, 0f, 0f, -1);
            vector.X += num4 + 5f;


            Vector2 vector2 = new Vector2(num + 6f, innerDimensions.Y + 60f);
            if (_EnigmaModData != null)
            {
                TagCompound data = _EnigmaModData.Get<TagCompound>("data");
                var downed = _EnigmaModData.GetList<string>("downed");
                DrawPanel(spriteBatch, vector2, 90);
                bool help = data.GetBool("etherial");
                string enigmaEtherial = ((help) ? "[c/48D7DA:Etherial]" : "Tangible");
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, enigmaEtherial, vector2 + new Vector2(10f, 3f),
                    Color.White, 0f, Vector2.Zero, Vector2.One);
                //Utils.DrawBorderString(spriteBatch, enigmaEtherial, vector2 + new Vector2(10f, 3f), Color.White, 1f, 0f, 0f, -1);
            }

            vector2.X += 100;
            if (_CalamityWorldData != null)
            {
                TagCompound calamityData = _CalamityWorldData.Get<TagCompound>("data");

                string revengeAndDeath = "normal";
                IList<string> list = calamityData.GetList<string>("downed");
                if (list.Contains("revenge"))
                {
                    revengeAndDeath = "[c/FA0000:Revengance";
                    if (list.Contains("death"))
                    {
                        revengeAndDeath += "] [c/990000:Death";
                    }

                    if (list.Contains("defiled"))
                    {
                        revengeAndDeath += "] [c/551a8b:defiled";
                    }

                    revengeAndDeath += " mode]";

                    DrawPanel(spriteBatch, vector2, (int)(revengeAndDeath.Length * 4.5));
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, revengeAndDeath, vector2 + new Vector2(10f, 3f),
                        Color.White, 0f, Vector2.Zero, Vector2.One);
                }

            }

            vector2.X += 285;
            if (_FargosModWorldData != null)
            {
                TagCompound fargosData = _FargosModWorldData.Get<TagCompound>("data");
                string masochist = "[c/006400:Not masochist]";
                IList<string> downed = fargosData.GetList<string>("downed");
                if (downed.Contains("masochist"))
                {
                    masochist = "[c/FF69B4:Masochist]";
                }

                DrawPanel(spriteBatch, vector2, 25 * 5);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, masochist, vector2 + new Vector2(5f, 3f),
                    Color.White, 0f, Vector2.Zero, Vector2.One);
            }

            if (IsMouseHovering)
            {
                NewUIWorldSelect.currentDictionary = worldData;
            }
        }
    }
}
