using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using TerrariaUltraApocalypse.API.ModConfigUIelement;

namespace TerrariaUltraApocalypse.Configs
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
