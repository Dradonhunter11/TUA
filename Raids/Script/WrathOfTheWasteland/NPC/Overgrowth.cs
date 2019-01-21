using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Raids.Script.WrathOfTheWasteland.NPC
{
    internal abstract class Overgrowth : TUAModNPC
    {
        public override bool CloneNewInstances => false;


        public int active = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}
