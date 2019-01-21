using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TUA.API
{
    class TranslationWrapper
    {
        private ModTranslation translation;

        internal TranslationWrapper(ModTranslation translation)
        {
            this.translation = translation;
        }

        public void SetDefault(String value)
        {
            translation.SetDefault(value);
        }

        public String GetDefault()
        {
            return translation.GetDefault();
        }

        public String GetTranslation(GameCulture culture)
        {
            return translation.GetTranslation(culture);
        }

        public void AddTranslation(GameCulture culture, String value)
        {
            translation.AddTranslation(culture, value);
        }

        public override string ToString()
        {
            GameCulture culture = Language.ActiveCulture;
            if (culture == GameCulture.English || translation.GetTranslation(culture) == null)
            {
                return translation.GetDefault();
            }

            return translation.GetTranslation(culture);
        }
    }
}
