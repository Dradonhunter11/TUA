using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementTemplate
{
    abstract class ModAchievement
    {
        public bool done;
        public string name = "default";
        public string description = "Undefined";
        public Func<bool> condition;
        public Mod mod;
        public Texture2D texture;

        public ModAchievement()
        {
            setDefault();
        } 

        public virtual bool AutoLoad(Mod mod, [CallerFilePath] String classPath = "")
        {
            ModAchievement achievement = (ModAchievement)Activator.CreateInstance(GetType());

            return true;
        }

        public abstract void reward(Player p);

        public bool Condition()
        {
            return condition.Invoke();
        }

        public void checkCondition(AchievementPlayer p)
        {
            bool c = Condition();
            if (c && !p.checkAchievement(name))
            {
                p.addAchievementToList(this);
                reward(p.player);
                achievementCompletion(p.player);
            }
        }

        public virtual void setDefault() { }

        public virtual void load(TagCompound tag)
        {
            done = tag.GetBool(name);
        }

        public virtual void save(TagCompound tag)
        {
            tag.Add(name, done);
        }

        public void achievementCompletion(Player p)
        {
            Main.NewText(p.name + " has completed [" + name + "]");
        }
    }
}
