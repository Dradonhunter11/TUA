using TUA.API;

namespace TUA.Raids.Script.WrathOfTheWasteland
{
    public abstract class Overgrowth : TUAModNPC
    {
        public override bool CloneNewInstances => false;

        public int active = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}
