using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TUA.API.CustomInventory.TileEntityContainer
{
    interface ITank
    {
        int Capacity { get; set; }
        bool SetCapacity(int capacity);
        int SetMaxCapacity(int maxCapacity);
        int GetCapacity();
        int GetMaxCapacity();
        Color GetLiquidColor();

    }
}
