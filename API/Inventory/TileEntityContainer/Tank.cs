using Microsoft.Xna.Framework;

namespace TUA.API.Inventory.TileEntityContainer
{
    public interface ITank
    {
        bool SetCurrentAmount(int capacity);

        int SetMaxCapacity(int maxCapacity);

        int GetCurrentAmount();

        int GetMaxCapacity();

        Color GetLiquidColor();

        int CurrentAmount { get; set; }
    }
}
