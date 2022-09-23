using UnityEngine;
using zg.gramrpg.rpg.entities;

namespace zg.gramrpg.assets.entities
{
    [CreateAssetMenu(fileName = "Hero", menuName = "GramRPG/HeroData", order = 1)]
    public class HeroData : ScriptableObject
    {
        public string heroName;
        public int attackPower;
        public int initialHealth;
        public int experience;
        public int level;

        public Hero GetHero()
        {
            // Create a new hero class from predefined data
            return new Hero(heroName, attackPower, initialHealth, experience, level);
        }
    }
}
