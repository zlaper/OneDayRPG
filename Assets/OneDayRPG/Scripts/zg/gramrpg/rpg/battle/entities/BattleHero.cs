using UnityEngine.UI;
using zg.gramrpg.rpg.entities;

namespace zg.gramrpg.rpg.battle.entities
{
    public class BattleHero : BattleEntity
    {
        public Hero hero { get; protected set; }
        public Image selectionHighlight;

        public void SetHero(Hero hero)
        {
            this.hero = hero;
            SetEntity(hero);
        }

        public void EnableHighlight()
        {
            if(isDead) return;

            // Show highlight to notify that we should select hero
            selectionHighlight.gameObject.SetActive(true);
        }

        public void DisableHighlight()
        {
            if(isDead) return;

            // Hide highlight 
            selectionHighlight.gameObject.SetActive(false);
        }
    }
}
