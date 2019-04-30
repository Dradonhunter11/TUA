using System.Collections.Generic;
using Terraria.Localization;
using TUA.API;

namespace TUA.Localization
{
    class LocalizationManager
    {

        private Dictionary<string, TranslationWrapper> manager;

        public static readonly LocalizationManager instance = new LocalizationManager();

        private LocalizationManager()
        {
            manager = new Dictionary<string, TranslationWrapper>();
            initLocalization();
        }

        public void AddNewTranslationToManager(string key, string defaultString)
        {
            TranslationWrapper translation = new TranslationWrapper(TUA.instance.CreateTranslation(key));TUA.instance.CreateTranslation(key);
            translation.SetDefault(defaultString);
            manager.Add(key, translation);
        }

        public void AddTranslation(GameCulture language, string key, string defaultString)
        {
            // if (manager.ContainsKey(key))
            {
                manager[key].AddTranslation(language, defaultString);
            }
        }

        public string GetTranslation(string key)
        {
            GameCulture culture = Language.ActiveCulture;
            // if (manager.ContainsKey(key))
            {
                // If `manager[key].GetTranslation(culture)` != null, return `manager[key].GetTranslation(culture)`
                // If `manager[key].GetTranslation(culture)` == null, return `manager[key].GetDefault()`
                return manager[key].GetTranslation(culture) ?? manager[key].GetDefault();
            }
            return "Unknown Translation";
        }

        public TranslationWrapper GetRawTranslation(string key)
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
            AddNewTranslationToManager("TUA.UI.LoreTitle", "Lore book");
        }
    }
}
