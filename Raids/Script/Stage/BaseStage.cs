using System.Collections.Generic;

namespace TerrariaUltraApocalypse.Raids.Script.Stage
{
    public abstract class BaseStage
    {
        protected bool completed = false;
        public abstract List<string> Quote { get; }

        public abstract void Start();
        public abstract bool CheckCondition();
        public abstract void Reward();

        public virtual bool FailCondition()
        {
            return false;
        }
    }
}
