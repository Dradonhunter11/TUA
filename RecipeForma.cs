using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse
{
    class RecipeForma
    {
        public String[] ingredient;
        public int[] number;

        public RecipeForma(String[] ing, int[] num)
        {
            this.ingredient = ing;
            this.number = num;
        }
    }
}
