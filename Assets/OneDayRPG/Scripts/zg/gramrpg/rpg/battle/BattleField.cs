using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.battle.entities;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.rpg.entities;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.battle
{
    public class BattleField : MonoBehaviour, ISerializable
    {
        // Events
        public Action<IEntity> onShowInfo;

        // Display
        public BattleHero[] heroes;
        public BattleEnemy enemy;
        public BattleDamage damageDisplay;

        private void Awake()
        {
            Assert.IsTrue(heroes.Length == Values.BATTLE_HEROES, "Enough battle heroes");

            // Assign long tap checks
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
                heroes[i].entityEvent += OnShowInfo;

            enemy.entityEvent += OnShowInfo;
        }

        #region Battle Functions

        public void SetHeroes(Hero[] heroEntities)
        {
            // Set heroes
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
                heroes[i].SetHero(heroEntities[i]);
        }

        public void SetEnemy(Enemy enemyEntity)
        {
            // Set enemy
            enemy.SetEnemy(enemyEntity);
        }

        #endregion

        #region Util Functions

        public void DoDamage(BattleEntity attacker, BattleEntity defender)
        {
            // Show damage text
            damageDisplay.ShowDamage(attacker.damage, defender.transform);

            // Apply damage
            defender.Damage(attacker.damage);
        }

        private void OnShowInfo(EntityEvent entityEvent, BattleEntity battleEntity)
        {
            // Check for long tap and show info
            if (entityEvent == EntityEvent.Info)
                onShowInfo?.Invoke(battleEntity.entity);
        }

        #endregion

        #region Save/Load

        public Dictionary<string, object> Serialize()
        {
            // Serialize heroes
            List<object> heroObjects = new List<object>();
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
                heroObjects.Add(heroes[i].Serialize());

            // Serialize enemy
            Dictionary<string, object> enemyData = enemy.Serialize();
            Dictionary<string, object> enemyEntityData = enemy.entity.Serialize();

            return new Dictionary<string, object>()
            {
                {SaveFields.HEROES, heroObjects},
                {SaveFields.ENEMY, enemyData},
                {SaveFields.ENEMY_ENTITY, enemyEntityData}
            };
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            // Create stored enemy entity
            Enemy enemyEntity = new Enemy();
            enemyEntity.Deserialize(data[SaveFields.ENEMY_ENTITY] as Dictionary<string, object>);

            // Set enemy entity
            enemy.SetEnemy(enemyEntity);

            // Load health
            enemy.Deserialize(data[SaveFields.ENEMY] as Dictionary<string, object>);

            // NOTE: Heroes are loaded in the save manager
        }

        #endregion
    }
}
