using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.API.EventManager
{
    
    internal abstract class MoonEvent : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        

        public bool IsActive = false;

        public bool downed = false;

        public int score = 0;

        private int waveCount = 0;

        public Dictionary<int, List<Tuple<int, float, int>>> enemyWave = new Dictionary<int, List<Tuple<int, float, int>>>();
        public abstract List<int> scoreTresholdLimitPerWave { get; }
        public abstract string EventName { get; }
        public abstract int MaxWave { get; }

        public abstract void Initialize();
        

        public virtual Texture2D moonTexture => ModContent.GetTexture("Terraria/Moon_" + Main.moonType);

        public MoonEvent()
        {
            Initialize();
            if (!MoonEventManagerWorld.moonEventList.ContainsKey(EventName))
            {
                MoonEventManagerWorld.moonEventList.Add(EventName, this);
            }
        }

        public void Activate()
        {
            Reset();
            IsActive = true;
        }

        public virtual void Reset()
        {
            waveCount = 0;
            score = 0;
            IsActive = false;
        }

        public void AddEnemy(int wave, int enemyType, float chance, int point)
        {
            if (enemyWave.ContainsKey(wave))
            {
                enemyWave[wave].Add(Tuple.Create(enemyType, chance, point));
                return;
            }
            enemyWave.Add(wave, new List<Tuple<int, float, int>>() { Tuple.Create(enemyType, chance, point) });
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (IsActive)
            {
                pool.Clear();
                foreach (var enemy in enemyWave[waveCount])
                {
                    
                    pool.Add(enemy.Item1, enemy.Item2);
                }
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (IsActive)
            {
                MoonEventManagerWorld.moonEventList[EventName].TrueCheckDead(npc);
                
            }
            return base.CheckDead(npc);
        }

        public bool TrueCheckDead(NPC npc)
        {
            int NPCID = npc.type;
            if (enemyWave[waveCount].Any(i => i.Item1 == NPCID))
            {
                score += enemyWave[waveCount].Single(i => i.Item1 == NPCID).Item3;
                BaseUtility.Chat(this.Name);
            }

            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (IsActive && MoonEventManagerWorld.moonEventList[EventName].score >= MoonEventManagerWorld.moonEventList[EventName].scoreTresholdLimitPerWave[waveCount])
            {
                TruePostAI(npc);
            }           
            base.PostAI(npc);
        }

        public void TruePostAI(NPC npc)
        {
            

            MoonEventManagerWorld.moonEventList[EventName].score = 0;
            MoonEventManagerWorld.moonEventList[EventName].waveCount++;
            Message(waveCount);
            if (waveCount > MaxWave)
            {
                OnDefeat();
            }
        }

        public virtual void Message(int wave)
        {
            return;
        }

        public virtual void Save(TagCompound tag)
        {
            tag.Add(this.GetType().Name + "Downed", downed);
        }

        public virtual void Load(TagCompound tag)
        {
            downed = tag.GetBool(this.GetType().Name + "Downed");
        }

        public virtual void OnDefeat()
        {

        }

        public void ReplaceMoon()
        {
            Main.moonTexture[Main.moonType] = moonTexture;
        }

        public static void ResetMoon()
        {
            Main.moonTexture[Main.moonType] = ModContent.GetTexture("Terraria/Moon_" + Main.moonType);
        }
    }
}
