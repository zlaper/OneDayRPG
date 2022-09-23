using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zg.gramrpg.data;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.serialize;
using zg.utils;

namespace zg.gramrpg.rpg.battle.entities
{
    [RequireComponent(typeof(TapDetector))]
    public class BattleEntity : MonoBehaviour, IEntityBattle
    {
        // Event
        public Action<EntityEvent, BattleEntity> entityEvent;

        // Display
        public SpriteRenderer entityFace;
        public Image healthBar;

        // Data
        public IEntity entity { get; private set; }
        public int health { get; private set; }
        public int damage { get; private set; }
        public bool isDead { get; private set; }

        // Start position
        public Vector2 startPosition { get; private set; }

        // Tap detector
        private TapDetector tap;

        private void Awake()
        {
            // Get tap and listen for events
            tap = GetComponent<TapDetector>();
            tap.tapped += OnTapped;

            // Store starting position
            startPosition = this.transform.position;
        }

        public void SetEntity(IEntity entity)
        {
            this.entity = entity;

            // Set entity values
            SetHealth(entity.health);
            damage = entity.attackPower;

            // Set graphics
            entityFace.sprite = entity.GetFace();
            gameObject.SetActive(true);
        }

        public void Damage(int amount)
        {
            // Remove health
            SetHealth(health - amount);
        }

        protected void SetHealth(int value)
        {
            // Subtract health with check that we are always above 0
            health = Mathf.Max(0, value);
            // Update health bar
            healthBar.fillAmount = Mathf.Clamp01((float)health / (float)entity.health);
            // Check if dead
            CheckIfDead();
        }

        private void CheckIfDead()
        {
            // Check if health is 0
            isDead = health == 0;

            // If we are dead, hide the gameobject
            if (isDead)
                gameObject.SetActive(false);
        }

        private void OnTapped(TapType tapType)
        {
            // Don't fire events if we are dead!
            if (isDead)
                return;

            switch (tapType)
            {
                case TapType.Tap:
                    entityEvent?.Invoke(EntityEvent.Select, this);
                    break;
                case TapType.LongTap:
                    entityEvent?.Invoke(EntityEvent.Info, this);
                    break;
            }
        }

        public Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data[SaveFields.ENTITY] = entity.id;
            data[SaveFields.HEALTH] = health;
            return data;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            int healthValue = Convert.ToInt32(data[SaveFields.HEALTH]);
            SetHealth(healthValue);
        }
    }
}
