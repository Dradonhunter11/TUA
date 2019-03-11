using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TUA.API.ModConfigUIelement
{
    abstract class StringOptionElementSettable : RangeElement
    {
        //private Func<string> _TextDisplayFunction;

        private Func<string> _GetValue;
        private Func<int> _GetIndex;
        private Action<int> _SetValue;
        string[] options;

        public override int NumberTicks => options.Length;
        public override float TickIncrement => 1f / (options.Length - 1);

        public MethodInfo SetPendingChanges;

        public StringOptionElementSettable(PropertyFieldWrapper memberInfo, object item, int orderIgnore, IList array = null, int index = -1) : base(memberInfo, item, orderIgnore, array)
        {
            OptionStringsAttribute optionsAttribute = ConfigManager.GetCustomAttribute<OptionStringsAttribute>(memberInfo, item, array);
            options = optionsAttribute.optionLabels;
            SetPendingChanges =
                UIModConfigUIType.GetMethod("SetPendingChanges", BindingFlags.Public | BindingFlags.Instance);
            IList<String> array2 = array as IList<String>;
            ILog yup = LogManager.GetLogger("Yup");
            yup.Info(optionsAttribute);
            yup.Info(SetPendingChanges);
            _TextDisplayFunction = () => memberInfo.Name + ": " + _GetValue();
            _GetValue = () => DefaultGetValue();
            _GetIndex = () => DefaultGetIndex();
            _SetValue = (int value) => DefaultSetValue(value);

            if (array != null)
            {
                
                _GetValue = () => array2[index];
                _SetValue = (int value) => { array2[index] = options[value]; SetPendingChanges.Invoke(UIModConfigInstance, new Object[] {true}); };
                _TextDisplayFunction = () => index + 1 + ": " + array2[index];
            }

            if (labelAttribute != null)
            {
                this._TextDisplayFunction = () => labelAttribute.Label + ": " + _GetValue();
            }

            _GetProportion = () => DefaultGetProportion();
            _SetProportion = (float proportion) => DefaultSetProportion(proportion);
        }

        void DefaultSetValue(int index)
        {
            if (!memberInfo.CanWrite) return;
            memberInfo.SetValue(item, options[index]);

            SetPendingChanges.Invoke(UIModConfigInstance, new Object[] { true });
        }

        string DefaultGetValue()
        {
            return (string)memberInfo.GetValue(item);
        }

        int DefaultGetIndex()
        {
            return Array.IndexOf(options, _GetValue());
        }

        float DefaultGetProportion()
        {
            return _GetIndex() / (float)(options.Length - 1);
        }

        void DefaultSetProportion(float proportion)
        {
            _SetValue((int)(Math.Round(proportion * (options.Length - 1))));
        }

        public void SetManuallyOption(String[] list)
        {
            this.options = list;
        }

        public void SetManuallyOption(List<String> list)
        {
            SetManuallyOption(list.ToArray());
        }
    }

    abstract class RangeElement : ConfigElement
    {
        internal bool drawTicks;
        public abstract int NumberTicks { get; }
        public abstract float TickIncrement { get; }
        protected Func<float> _GetProportion;
        protected Action<float> _SetProportion;
        protected object UIModConfigInstance;
        protected Type UIModConfigUIType;

        public RangeElement(PropertyFieldWrapper memberInfo, object item, int orderIgnore, IList array = null, int index = -1) : base(memberInfo, item, (IList)array)
        {
            drawTicks = Attribute.IsDefined(memberInfo.MemberInfo, typeof(DrawTicksAttribute));
            ILog logger = LogManager.GetLogger("I exist");
            try
            {
                UIModConfigInstance = typeof(Main).Assembly.GetType("Terraria.ModLoader.Interface")
                    .GetField("modConfig", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                
                UIModConfigUIType = typeof(Main).Assembly.GetType("Terraria.ModLoader.Config.UI.UIModConfig");
            }
            catch (Exception e)
            {
                logger.Info(e.Message, e);
            }

            logger.Info(UIModConfigInstance);
            logger.Info(UIModConfigUIType);
        }

        public float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
        {
            perc = Utils.Clamp(perc, -.05f, 1.05f);
            if (colorMethod == null)
            {
                colorMethod = new Utils.ColorLerpMethod(Utils.ColorLerp_BlackToWhite);
            }
            Texture2D colorBarTexture = Main.colorBarTexture;
            Vector2 vector = new Vector2((float)colorBarTexture.Width, (float)colorBarTexture.Height) * scale;
            IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - (float)((int)vector.X);
            Rectangle rectangle = new Rectangle((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
            Rectangle destinationRectangle = rectangle;
            int num = 167;
            float num2 = (float)rectangle.X + 5f * scale;
            float num3 = (float)rectangle.Y + 4f * scale;
            if (drawTicks)
            {
                int numTicks = NumberTicks;
                if (numTicks > 1)
                {
                    for (int tick = 0; tick < numTicks; tick++)
                    {
                        float percent = tick * TickIncrement;
                        if (percent <= 1f)
                            sb.Draw(Main.magicPixel, new Rectangle((int)(num2 + num * percent * scale), rectangle.Y - 2, 2, rectangle.Height + 4), Color.White);
                    }
                }
            }
            sb.Draw(colorBarTexture, rectangle, Color.White);
            for (float num4 = 0f; num4 < (float)num; num4 += 1f)
            {
                float percent = num4 / (float)num;
                sb.Draw(Main.colorBlipTexture, new Vector2(num2 + num4 * scale, num3), null, colorMethod(percent), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            rectangle.Inflate((int)(-5f * scale), 2);
            //rectangle.X = (int)num2;
            //rectangle.Y = (int)num3;
            bool flag = rectangle.Contains(new Point(Main.mouseX, Main.mouseY));
            if (lockState == 2)
            {
                flag = false;
            }
            if (flag || lockState == 1)
            {
                sb.Draw(Main.colorHighlightTexture, destinationRectangle, Main.OurFavoriteColor);
            }
            sb.Draw(Main.colorSliderTexture, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), null, Color.White, 0f, new Vector2(0.5f * (float)Main.colorSliderTexture.Width, 0.5f * (float)Main.colorSliderTexture.Height), scale, SpriteEffects.None, 0f);
            if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
            {
                IngameOptions.inBar = flag;
                return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
            }
            IngameOptions.inBar = false;
            if (rectangle.X >= Main.mouseX)
            {
                return 0f;
            }
            return 1f;
        }

        private static RangeElement rightLock;
        private static RangeElement rightHover;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            float num = 6f;
            int num2 = 0;
            //IngameOptions.rightHover = -1;
            rightHover = null;
            if (!Main.mouseLeft)
            {
                //IngameOptions.rightLock = -1;
                rightLock = null;
            }
            //if (IngameOptions.rightLock == this._sliderIDInPage)
            if (rightLock == this)
            {
                num2 = 1;
            }
            //else if (IngameOptions.rightLock != -1)
            else if (rightLock != null)
            {
                num2 = 2;
            }
            CalculatedStyle dimensions = base.GetDimensions();
            float num3 = dimensions.Width + 1f;
            Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
            bool flag2 = base.IsMouseHovering;
            if (num2 == 1)
            {
                flag2 = true;
            }
            if (num2 == 2)
            {
                flag2 = false;
            }
            Vector2 vector2 = vector;
            vector2.X += 8f;
            vector2.Y += 2f + num;
            vector2.X -= 17f;
            Main.colorBarTexture.Frame(1, 1, 0, 0);
            vector2 = new Vector2(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
            IngameOptions.valuePosition = vector2;
            float obj = DrawValueBar(spriteBatch, 1f, this._GetProportion(), num2);
            //if (IngameOptions.inBar || IngameOptions.rightLock == this._sliderIDInPage)
            if (IngameOptions.inBar || rightLock == this)
            {
                rightHover = this;
                //IngameOptions.rightHover = this._sliderIDInPage;
                if (PlayerInput.Triggers.Current.MouseLeft && rightLock == this)
                //if (PlayerInput.Triggers.Current.MouseLeft && IngameOptions.rightLock == this._sliderIDInPage)
                {
                    this._SetProportion(obj);
                }
            }
            if (rightHover != null && rightLock == null)
            //if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
            {
                //IngameOptions.rightLock = IngameOptions.rightHover;
                rightLock = rightHover;
            }
        }
    }

    internal class OptionStringCustomSky : StringOptionElementSettable
    {
        public OptionStringCustomSky(PropertyFieldWrapper memberInfo, object item, int orderIgnore, IList array = null, int index = -1) : base(memberInfo, item, orderIgnore, array)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Dictionary<String, CustomSky> temp2 = (Dictionary<string, CustomSky>)typeof(SkyManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SkyManager.Instance);
            Dictionary<String, Overlay> temp = (Dictionary<string, Overlay>)typeof(OverlayManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Overlays.Scene);
            List<String> allKey = temp2.Keys.ToList();
            List<String> allKey2 = temp.Keys.ToList();
            //Removing monolith and calamity background as most of them doesn't work anyway on the main menu 
            allKey = allKey.Where(i => !i.Contains("Monolith") && !i.Contains("CalamityMod")).ToList();
            allKey.Insert(0, "Vanilla");
            for (int k = 0; k < allKey2.Count; k++)
            {
                string key = allKey2[k];
                if (allKey.Any(i => i == key))
                {
                    continue;
                }
                allKey.Add(key);
            }

            for (int i = 0; i < allKey.Count; i++)
            {
                string key = allKey[i];
                if (key != TUA.custom.newMainMenuTheme && SkyManager.Instance[key] != null)
                    SkyManager.Instance[key].Deactivate();
            }

            SetManuallyOption(allKey);
        }
    }
}

