using System.Collections;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.battle.entities;

namespace zg.gramrpg.rpg.battle.battlePhases
{
    public class BattlePhaseEndCheck : BattlePhase
    {

        protected BattleFlow battleFlow;

        public BattlePhaseEndCheck(BattleField battle, BattleFlow battleFlow) : base(battle, BattlePhaseType.CheckEnd)
        {
            this.battleFlow = battleFlow;
        }

        public override IEnumerator ExecutePhase()
        {
            // Check if heroes are dead
            if (AllHeroesDead())
            {
                // All heroes dead, the enemy is the winner
                battleFlow.FinishBattle(WinnerType.Enemy);
            }
            else if (battle.enemy.isDead)
            {
                // Enemy is dead, the heroes are winners
                battleFlow.FinishBattle(WinnerType.Heroes);
            }

            yield return null;
        }

        #region Util Functions

        private bool AllHeroesDead()
        {
            bool allDead = true;
            // Check if any hero is still alive
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
            {
                BattleHero hero = battle.heroes[i];
                if (!hero.isDead)
                    allDead = false;
            }

            return allDead;
        }

        #endregion
    }
}
