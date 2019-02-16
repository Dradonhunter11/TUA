using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    abstract class BaseRaids
    {
        public List<BaseStage> stageList = new List<BaseStage>();

        public async virtual void dialog(int stage = 0)
        {

        }
    }
}
