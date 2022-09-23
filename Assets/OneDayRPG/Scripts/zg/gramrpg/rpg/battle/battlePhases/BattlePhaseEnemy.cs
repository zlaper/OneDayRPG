using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle.entities;

namespace zg.gramrpg.rpg.battle.battlePhases
{
    public class BattlePhaseEnemy : BattlePhase
    {
        public BattlePhaseEnemy(BattleField battle) : base(battle, BattlePhaseType.Enemy)
        {
        }

        public override IEnumerator ExecutePhase()
        {
            // Check if enemy is dead
            if (!battle.enemy.isDead)
            {
                // Get alive heroes
                List<BattleHero> heroes = GetHeroesAlive();
                int len = heroes.Count;
                // Check if any heroes are still alive
                if (len > 0)
                {
                    // Attack random alive hero
                    BattleHero hero = heroes[Random.Range(0, len - 1)];

                    // Attack in
                    yield return battle.enemy.transform.DOMove(hero.transform.position, Values.ANIMATION_ATTACK_IN_TIME)
                        .SetEase(Ease.InOutElastic)
                        .OnComplete(OnHit)
                        .WaitForCompletion();

                    // Apply damage to enemy
                    battle.DoDamage(battle.enemy, hero);

                    // Attack out
                    yield return battle.enemy.transform.DOMove(battle.enemy.startPosition, Values.ANIMATION_ATTACK_OUT_TIME)
                        .SetEase(Ease.OutFlash)
                        .WaitForCompletion();
                }
            }

            yield return null;
        }

        #region Util functions

        private void OnHit()
        {
            SoundControl.Instance.PlaySound(SoundType.MonsterAttack);
        }

        private List<BattleHero> GetHeroesAlive()
        {
            List<BattleHero> aliveHeroes = new List<BattleHero>();
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
            {
                BattleHero hero = battle.heroes[i];
                if (!hero.isDead)
                    aliveHeroes.Add(hero);
            }

            return aliveHeroes;
        }
        #endregion
    }
}
