using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    public abstract class BaseStage
    {
        protected bool completed = false;
        public abstract List<String> Quote { get; }

        public abstract void Start();
        public abstract bool CheckCondition();
        public abstract void Reward();

        public virtual bool FailCondition()
        {
            return false;
        }
    }
}
