using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    abstract class InvasionStage : BaseStage
    {
        public abstract List<int> NPCList { get; }

    }
}
