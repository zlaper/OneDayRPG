using DG.Tweening;
using System.Collections;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.game;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle.entities;
using zg.gramrpg.rpg.entities;

namespace zg.gramrpg.rpg.battle.battlePhases
{
    public class BattlePhaseHero : BattlePhase
    {

        protected BattleHero selectedHero;
        protected WaitWhile waitForHeroSelection;

        public BattlePhaseHero(BattleField battle) : base(battle, BattlePhaseType.Hero)
        {
            waitForHeroSelection = new WaitWhile(NoHeroSelected);
        }

        public override IEnumerator ExecutePhase()
        {
            // Enable select hero
            selectedHero = null;
            EnableHeroSelection();

            // Wait for selection
            yield return waitForHeroSelection;

            // Stop selecting
            StopHeroSelection();

            // Attack in
            yield return selectedHero.transform.DOMove(battle.enemy.transform.position, Values.ANIMATION_ATTACK_IN_TIME)
                .SetEase(Ease.InOutElastic)
                .OnComplete(OnHit)
                .WaitForCompletion();

            // Apply damage to enemy
            battle.DoDamage(selectedHero, battle.enemy);

            // Attack out
            yield return selectedHero.transform.DOMove(selectedHero.startPosition, Values.ANIMATION_ATTACK_OUT_TIME)
                .SetEase(Ease.OutFlash)
                .WaitForCompletion();

            // Clear
            selectedHero = null;
            yield return null;
        }

        #region Util functions

        private void OnHit()
        {
            SoundControl.Instance.PlaySound(SoundType.Sword);
        }

        private void OnHeroSelect(EntityEvent eventType, BattleEntity hero)
        {
            // Assign selection
            if (eventType == EntityEvent.Select)
                selectedHero = hero as BattleHero;
        }

        private bool NoHeroSelected()
        {
            // Check if hero is selected
            return selectedHero == null;
        }

        private void EnableHeroSelection()
        {
            // Enable highlight and wait for selection event
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
            {
                BattleHero hero = battle.heroes[i];
                hero.EnableHighlight();
                hero.entityEvent += OnHeroSelect;
            }
        }

        private void StopHeroSelection()
        {
            // Disable highlight and selection event
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
            {
                BattleHero hero = battle.heroes[i];
                hero.DisableHighlight();
                hero.entityEvent -= OnHeroSelect;
            }
        }

        #endregion
    }
}
