using System.ComponentModel;
using Terraria.ModLoader.Config;
using TUA.API.ModConfigUIelement;

namespace TUA.Configs
{
    [Label("Main menu config")]
    class CustomTitleMenuConfig : ModConfig
    {

        private readonly StringOptionElementSettable something;

        [DefaultValue(false)]
        [Label("Custom Main Menu")]
        [Tooltip("Allow you to enable TUA custom main menu")]
        public bool CustomMenu;

        [OptionStrings(new string[] { "Vanilla", "Stardust", "Solar", "Nebula", "Vortex"})]
        [DefaultValue("Vanilla" )]
        [Label("Main menu theme")]
        [CustomModConfigItem(typeof(OptionStringCustomSky))]
        public string NewMainMenuTheme;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            TUA.custom = this;
        }
    }
}
