using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] CharacterClass characterClass;
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] Progression progression = null;
        [SerializeField] bool shouldUseModifiers = false;
 
        private void Update() {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(gameObject.name + ": " + GetLevel());
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
        }

        public int GetLevel()
        {
            Experience exp = GetComponent<Experience>();
            if (exp == null)
            {
                return startingLevel;
            }
            
            int level = 1;
            while (progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level) <= exp.GetExperience() && level < 100)
            {
                level++;
            }

            return level;
        }
    }
}