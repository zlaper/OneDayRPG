using System.Collections;
using zg.gramrpg.data;

namespace zg.gramrpg.rpg.definitions
{
    public interface IBattlePhase
    {
        BattlePhaseType phase { get; }
        IEnumerator ExecutePhase();
    }
}
