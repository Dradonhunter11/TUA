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
            MoonEventManagerWorld
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
                foreach (var enemy in enemyWave[waveCount])
                {
                    pool.Clear();
                    pool.Add(enemy.Item1, enemy.Item2);
                }
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (IsActive)
            {
                int NPCID = npc.type;
                if (enemyWave[waveCount].Any(i => i.Item1 == NPCID))
                {
                    score += enemyWave[waveCount].Single(i => i.Item1 == NPCID).Item3;
                }
            }
            return base.CheckDead(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (IsActive && score >= scoreTresholdLimitPerWave[waveCount])
            {
                Message(waveCount);

                score = 0;
                waveCount++;
                if (waveCount > MaxWave)
                {
                    OnDefeat();
                }
            }           
            base.PostAI(npc);
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
