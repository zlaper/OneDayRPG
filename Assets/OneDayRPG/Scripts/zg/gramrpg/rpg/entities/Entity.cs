using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.entities
{
    public abstract class Entity : IEntity
    {
        public int id { get; set; }
        public string name { get; protected set; }

        public int initialAttackPower { get; set; }
        public int initialHealth { get; set; }

        public int attackPower => Mathf.CeilToInt(initialAttackPower * levelScale);
        public int health => Mathf.CeilToInt(initialHealth * levelScale);

        public int experience { get; protected set; }
        public int level { get; protected set; }

        protected float levelScale => 1 + Values.LEVEL_SCALE * (level - 1);

        public Entity()
        {
            // For loading
        }

        public Entity(string name, int attackPower, int initialHealth, int experience, int level)
        {
            // For new entities
            this.name = name;

            // Store initial properties
            this.initialAttackPower = attackPower;
            this.initialHealth = initialHealth;

            this.experience = experience;
            this.level = level;
        }

        public abstract Sprite GetFace();

        public virtual Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>()
            {
                {SaveFields.NAME, name},
                {SaveFields.ATTACK_POWER, initialAttackPower},
                {SaveFields.INITIAL_HEALTH, initialHealth},
                {SaveFields.EXPERIENCE, experience},
                {SaveFields.LEVEL, level},
            };
        }

        public virtual void Deserialize(Dictionary<string, object> data)
        {
            name = Convert.ToString(data[SaveFields.NAME]);
            initialAttackPower = Convert.ToInt32(data[SaveFields.ATTACK_POWER]);
            initialHealth = Convert.ToInt32(data[SaveFields.INITIAL_HEALTH]);
            experience = Convert.ToInt32(data[SaveFields.EXPERIENCE]);
            level = Convert.ToInt32(data[SaveFields.LEVEL]);
        }
    }
}
