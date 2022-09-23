using TMPro;
using UnityEngine;
using UnityEngine.UI;
using zg.gramrpg.rpg.definitions;

namespace zg.gramrpg.ui.components
{
    public class HeroWinCell : MonoBehaviour
    {

        public Image heroFace;

        public TextMeshProUGUI status;
        public TextMeshProUGUI entityName;
        public TextMeshProUGUI attackPower;
        public TextMeshProUGUI health;
        public TextMeshProUGUI experience;
        public TextMeshProUGUI level;

        protected int _attackPower;
        protected int _health;
        protected int _experience;
        protected int _level;

        public void ShowEntity(IEntity entity, bool isDead)
        {
            // Set face
            heroFace.sprite = entity.GetFace();

            // Store entity properties
            _attackPower = entity.attackPower;
            _health = entity.health;
            _experience = entity.experience;
            _level = entity.level;

            // Display info
            entityName.text = entity.name;
            attackPower.text = $"{entity.attackPower}";
            health.text = $"{entity.health}";

            if (isDead)
            {
                // If dead, experience is the same
                experience.text = $"{entity.experience}";
            }
            else
            {
                // if alive we gain 1 experience
                experience.text = $"{entity.experience + 1} (+1)";
            }

            level.text = $"{entity.level}";

            // Update status
            status.text = isDead ? "Dead" : "Alive";
        }

        public void ShowLeveledEntity(IEntity entity)
        {
            // Update info based on the new level (new value - old value)
            attackPower.text = $"{entity.attackPower} (+{entity.attackPower - _attackPower})";
            health.text = $"{entity.health} (+{entity.health - _health})";
            experience.text = $"{entity.experience} (+{entity.experience - _experience})";
            level.text = $"{entity.level} (+{entity.level - _level})";
        }
    }
}