using UnityEngine;

namespace zg.gramrpg.rpg.definitions
{
    public interface IEntityBattle
    {
        IEntity entity { get; }
        int health { get; }
        bool isDead { get; }
        Vector2 startPosition { get; }

        void SetEntity(IEntity entity);
        void Damage(int amount);
    }
}
