using Microsoft.Xna.Framework;

namespace TUA.API.CustomInventory.TileEntityContainer
{
    interface ITank
    {
        int CurrentAmount { get; set; }
        bool SetCurrentAmount(int capacity);
        int SetMaxCapacity(int maxCapacity);
        int GetCurrentAmount();
        int GetMaxCapacity();
        Color GetLiquidColor();
    }
}
