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

        public bool done; //Was originally supposed to apply client directly, but I ended up saving on player
        public string name = "default"; //Achievement name
        public string description = "Undefined"; //Achievement description
        public string rewardDesc = "";

        public Func<bool> condition;
        public Mod mod;
        public Texture2D texture;

        public ModAchievement(Mod mod)
        {
            this.mod = mod;
            setDefault();
            AutoLoad(mod);
        } 

        public virtual bool AutoLoad(Mod mod, [CallerFilePath] String classPath = "")
        {
            //ModAchievement achievement = (ModAchievement)Activator.CreateInstance(GetType());
            try
            {
                string t = (GetType().FullName).Replace('.', '/')
                           .Replace(mod.File.name, "")
                           .Remove(0, 1);

                texture = mod.GetTexture(t);
            }
            catch (Exception e)
            {
            }
            
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
