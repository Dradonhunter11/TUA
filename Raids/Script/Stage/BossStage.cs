using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    public abstract class BossStage : BaseStage
    {
        public abstract int bossID { get; }
    }
}
