using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.LiquidAPI.Test
{
    class PlutonicWaste : ModLiquid
    {
        public override Texture2D texture
        {
            get { return ModLoader.GetMod("TerrariaUltraApocalypse").GetTexture("Texture/water/BestWater"); }
        }

        public override void PreDrawValueSet(ref bool bg, ref int style, ref float Alpha)
        {
            style = 12;
            Alpha = 0.2f;
        }

        public override float SetLiquidOpacity()
        {
            return 0.5f;
        }

        public override void PlayerInteraction(Player target)
        {
            PlayerDeathReason reason = PlayerDeathReason.ByCustomReason(target.name + " learned that waste is dangerous and can kill.");
            //target.KillMe(reason, 1000, 1);
        }

        public override void NpcInteraction(NPC target)
        {
            if (target.type == NPCID.GreenSlime || target.type == NPCID.BlueSlime || target.type == NPCID.PurpleSlime)
            {
                Vector2 position = target.Center;
                target.active = false;
                NPC.NewNPC((int) position.X, (int) position.Y,
                    ModLoader.GetMod("TerrariaUltraApocalypse").NPCType("MutatedSludge"));
            }
        }
    }
}
