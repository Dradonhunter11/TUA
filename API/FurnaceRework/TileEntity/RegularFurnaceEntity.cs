namespace TUA.API.FurnaceRework.TileEntity
{
    class RegularFurnaceEntity : BaseFurnaceEntity
    {

        public override void SetValue(ref int maxEnergy, ref int cookTimer, ref string furnaceName)
        {
            cookTimer = 120;
            maxEnergy = 50;
            furnaceName = "Furnace";
        }
    }
}
