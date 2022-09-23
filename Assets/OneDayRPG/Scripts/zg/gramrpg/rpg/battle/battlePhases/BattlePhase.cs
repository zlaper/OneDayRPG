using System.Collections;
using zg.gramrpg.data;
using zg.gramrpg.rpg.definitions;

namespace zg.gramrpg.rpg.battle.battlePhases
{
    public abstract class BattlePhase : IBattlePhase
    {
        public BattlePhaseType phase { get; protected set; }

        protected BattleField battle;

        public BattlePhase(BattleField battle, BattlePhaseType phase)
        {
            this.battle = battle;
            this.phase = phase;
        }

        public abstract IEnumerator ExecutePhase();
    }
}
