﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerrariaUltraApocalypse.API.Achievements.AchievementTemplate;

namespace TerrariaUltraApocalypse.API.Achievements
{
    class AchievementPlayer : ModPlayer
    {
        public List<string> doneAchievementList = new List<string>();

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            foreach (string achievmentName in doneAchievementList)
            {
                tag.Add(achievmentName, true);
            }
            return tag;
        }


        public override void Load(TagCompound tag)
        {
            AchievementManager.GetInstance().loadAchievement(tag, this);
        }

        public void addAchievementToList(ModAchievement achievement)
        {
            doneAchievementList.Add(achievement.name);
        }

        public bool checkAchievement(String name)
        {
            foreach (string s in doneAchievementList)
            {
                if (s == name)
                {
                    return true;
                }
            }
            return false;
        }

        public override void PreUpdate()
        {
            AchievementManager.GetInstance().CheckAchievement(this);
        }
    }
}