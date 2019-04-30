using System.Collections.Generic;

namespace TUA.Raids.Script.Stage
{
    abstract class InvasionStage : BaseStage
    {
        public abstract List<int> NPCList { get; }

    }
}
