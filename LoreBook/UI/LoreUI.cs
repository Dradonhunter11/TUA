using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using TUA.API.UI;

namespace TUA.LoreBook.UI
{
    class LoreUI : UIState
    {
        internal LorePlayer instance;
        internal CustomizableUIPanel mainPanel;

        public LoreUI(LorePlayer instance)
        {
            this.instance = instance;

            mainPanel = new CustomizableUIPanel(TUA.instance.GetTexture("Texture/UI/panel"));

        }
    }
}
