using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUA
{
    class RecipeForma
    {
        public string[] ingredient;
        public int[] number;

        public RecipeForma(string[] ing, int[] num)
        {
            this.ingredient = ing;
            this.number = num;
        }
    }
}
