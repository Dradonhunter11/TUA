using Terraria.ModLoader;

namespace TUA.Buff
{
    public abstract class TUABuff : ModBuff
    {
        private readonly string _displayName, _description;

        protected TUABuff(string displayName) : this(displayName, "")
        {
        }

        protected TUABuff(string displayName, string description)
        {
            _displayName = displayName;
            _description = description;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            DisplayName.SetDefault(_displayName);
            Description.SetDefault(_description);
        }
    }
}