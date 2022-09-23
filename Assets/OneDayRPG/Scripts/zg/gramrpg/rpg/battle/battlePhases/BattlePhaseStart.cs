using DG.Tweening;
using System.Collections;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle.entities;

namespace zg.gramrpg.rpg.battle.battlePhases
{
    public class BattlePhaseStart : BattlePhase
    {
        public BattlePhaseStart(BattleField battle) : base(battle, BattlePhaseType.Start)
        {
        }

        public override IEnumerator ExecutePhase()
        {
            // Offset enemy
            MoveOffset(battle.enemy.transform, 10);

            // Animate heroes in
            Sequence heroesIn = DOTween.Sequence();

            for (int i = 0; i < Values.BATTLE_HEROES; i++)
            {
                BattleHero hero = battle.heroes[i];

                // Offset to the left
                MoveOffset(hero.transform, -10);

                // Animate in
                heroesIn.Append(hero.transform.DOMoveX(hero.startPosition.x, Values.ANIMATION_IN_TIME)
                    .SetEase(Ease.OutBack)
                    .OnStart(PlayWhoosh));
            }

            // Wait for completion
            yield return heroesIn.WaitForCompletion();

            // Animate Enemy in and wait for completion
            yield return battle.enemy.transform.DOMoveX(battle.enemy.startPosition.x, Values.ANIMATION_IN_TIME)
                .SetEase(Ease.OutBack)
                .OnStart(OnMonsterAppear)   
                .WaitForCompletion();

            // Wait 1 sec
            yield return new WaitForSecondsRealtime(1.0f);
        }

        #region Util Functions

        private void PlayWhoosh()
        {
            SoundControl.Instance.PlaySound(SoundType.Whoosh);
        }

        private void OnMonsterAppear()
        {
            SoundControl.Instance.PlaySound(SoundType.MonsterAppear);
        }

        private void MoveOffset(Transform transform, float offsetX)
        {
            // Move transform on the X axis by offset
            Vector3 position = transform.position;
            position.x += offsetX;
            transform.position = position;
        }

        #endregion
    }
}
