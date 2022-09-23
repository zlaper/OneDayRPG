using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.assets.entities;
using Random = UnityEngine.Random;

namespace zg.gramrpg.rpg.entities
{
    public class EntityGenerator : MonoBehaviour
    {
        [Header("Predefined Heroes")]
        public List<HeroData> predefinedHeroes;

        [Header("Random Data")]
        public RandomData randomHeroParams;
        public RandomData randomEnemyParams;

        public Hero GenerateHero()
        {
            // Check for predefined hero chance
            if (Random.value <= Values.PREDEFINED_HERO_CHANCE)
                return predefinedHeroes.Random().GetHero();

            // Create random hero
            string heroName = "H" + DateTime.Now.Ticks;

            int attackPower = randomHeroParams.attackPower.random;
            int initialHealth = randomHeroParams.initialHealth.random;
            int experience = randomHeroParams.experience.random;
            int level = randomHeroParams.level.random;

            return new Hero(heroName, attackPower, initialHealth, experience, level);
        }

        public Enemy GenerateEnemy()
        {
            // Create random enemy
            string enemyName = "H" + DateTime.Now.Ticks;

            int attackPower = randomEnemyParams.attackPower.random;
            int initialHealth = randomEnemyParams.initialHealth.random;
            int experience = randomEnemyParams.experience.random;
            int level = randomEnemyParams.level.random;

            return new Enemy(enemyName, attackPower, initialHealth, experience, level);
        }
    }
}
