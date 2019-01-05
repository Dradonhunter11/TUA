using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Localization
{
    class LocalizationManager
    {

        private Dictionary<String, TranslationWrapper> manager;

        public static readonly LocalizationManager instance = new LocalizationManager();

        private LocalizationManager()
        {
            manager = new Dictionary<string, TranslationWrapper>();
            initLocalization();
        }

        public void AddNewTranslationToManager(String key, String defaultString)
        {
            TranslationWrapper translation = new TranslationWrapper(TerrariaUltraApocalypse.instance.CreateTranslation(key));TerrariaUltraApocalypse.instance.CreateTranslation(key);
            translation.SetDefault(defaultString);
            manager.Add(key, translation);
        }

        public void AddTranslation(GameCulture language, String key, String defaultString)
        {
            if (manager.ContainsKey(key))
            {
                manager[key].AddTranslation(language, defaultString);
            }
        }

        public String GetTranslation(String key)
        {
            GameCulture culture = Language.ActiveCulture;
            if (manager.ContainsKey(key))
            {
                if (manager[key].GetTranslation(culture) == null)
                {
                    return manager[key].GetDefault();
                }

                return manager[key].GetTranslation(culture);
            }
            return "Unknown Translation";
        }

        public TranslationWrapper GetRawTranslation(String key)
        {
            GameCulture culture = Language.ActiveCulture;
            if (manager.ContainsKey(key))
            {
                return manager[key];
            }
            return null;
        }

        private void initLocalization()
        {
            AddNewTranslationToManager("TUA.UI.RaidsSelect", "Raids Selection");
            AddTranslation(GameCulture.French, "TUA.UI.RaidsSelect", "Sélection des raids");
            AddNewTranslationToManager("TUA.UI.Select", "Select");
            AddTranslation(GameCulture.French, "TUA.UI.Select", "Sélectionner");
        }
    }
}
