using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUA.API
{
    public sealed class Ref<T>
    {
        private readonly T[] array;
        private readonly int index;

        public ref T Value => ref array[index];

        public Ref(T value = default)
        {
            array = new[] { value };
            index = 0;
        }

        public Ref(T[] array, int index)
        {
            this.array = array;
            this.index = index;
        }
    }
}
