using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.Items.Misc;

namespace TUA.API
{
    /// <summary>
    /// Keep or remove?
    /// </summary>
    public abstract class TUATile : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Origin = new Point16(1, 2);
        }


        public override bool Autoload(ref string name, ref string texture)
        {
            if (name == "TUABlock" || name == "Capacitor")
            {
                return false;
            }

            return base.Autoload(ref name, ref texture);
        }

        public sealed override void RightClick(int i, int j)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<DebugStick>())
            {
                ReadData(i, j);
            }
            NewRightClick(i, j);
            
        }

        public virtual void NewRightClick(int x, int y)
        {

        }

        public virtual void ReadData(int x, int y)
        {

        }

    }
}