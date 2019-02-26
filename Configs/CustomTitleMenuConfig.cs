using System.ComponentModel;
using Terraria.ModLoader.Config;
using TUA.API.ModConfigUIelement;

namespace TUA.Configs
{
    [Label("Main menu config")]
    class CustomTitleMenuConfig : ModConfig
    {
        public override MultiplayerSyncMode Mode => MultiplayerSyncMode.UniquePerPlayer;

        private readonly StringOptionElementSettable something;

        [DefaultValue(false)]
        [Label("Custom Main Menu")]
        [Tooltip("Allow you to enable TUA custom main menu")]
        public bool customMenu;

        [OptionStrings(new string[] { "Vanilla", "Stardust", "Solar", "Nebula", "Vortex"})]
        [DefaultValue("Vanilla" )]
        [Label("Main menu theme")]
        [CustomModConfigItem(typeof(OptionStringCustomSky))]
        public string newMainMenuTheme;

        public override void PostAutoLoad()
        {        
            TerrariaUltraApocalypse.custom = this;
        }
    }
}
