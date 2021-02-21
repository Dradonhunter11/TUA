using System.Collections.Generic;

namespace TUA.Raids.Script.Stage
{
    abstract class BaseRaids
    {
        public List<BaseStage> stageList = new List<BaseStage>();

        public async virtual void DialogAsync(int stage = 0)
        {

        }
    }
}
