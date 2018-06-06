using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace TerrariaUltraApocalypse.API
{
    class TUAModNPC : ModNPC
    {

        public override bool Autoload(ref string name)
        {
            if (name == "TUAModNPC")
            {
                return false;
            }
            return base.Autoload(ref name);
        }

        public override ModNPC NewInstance(NPC npcClone)
        {
            if (TUAWorld.UltraMode)
            {
                ultraScaleDifficylty(npcClone);
            }
            return base.NewInstance(npcClone);
        }

        //This method is used to do NPC scaling in Ultra mode
        public virtual void ultraScaleDifficylty(NPC npc) { }

       
    }
}