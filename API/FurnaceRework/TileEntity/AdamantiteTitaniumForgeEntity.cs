using Terraria;

namespace TUA.API.FurnaceRework.TileEntity
{
    class AdamantiteTitaniumForgeEntity : BaseFurnaceEntity
    {

        public override void SetValue(ref int maxEnergy, ref int cookTimer, ref string furnaceName)
        {
            maxEnergy = 500;
            cookTimer = 30;
            if (!Main.gameMenu)
            {
                Tile t = Main.tile[this.Position.X, this.Position.Y];
                if (t.frameX >= 54)
                {
                    furnaceName = "Titanium Forge";
                }
                else
                {
                    furnaceName = "Adamantite Forge";
                }
            }
        }
    }
}
