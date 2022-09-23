using zg.gramrpg.rpg.entities;

namespace zg.gramrpg.rpg.battle.entities
{
    public class BattleEnemy : BattleEntity
    {
        public Enemy enemy { get; protected set; }

        public void SetEnemy(Enemy enemy)
        {
            this.enemy = enemy;
            SetEntity(enemy);
        }
    }
}
