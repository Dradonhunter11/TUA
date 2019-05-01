using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TUA
{
    public abstract class TUAInterface : UIState
    {
        protected UIElement _area;
        protected UITextPanel<string> hallowAlt;

        public override void OnInitialize()
        {
            _area = new UIElement();
            _area.Width.Set(0, 0.8f);
            _area.Height.Set(-210, 1f);
            _area.Top.Set(200f, 0f);
            _area.HAlign = 0.5f;

            hallowAlt = new UITextPanel<string>("Hallow alt : ", 1f, false);
            hallowAlt.Width.Set(10f, 0.8f);
            hallowAlt.Height.Set(20f, 1f);

            _area.Append(hallowAlt);
        }
    }
}
