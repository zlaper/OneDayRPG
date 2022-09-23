using UnityEngine;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.definitions
{
    public interface IEntity : ISerializable
    {
        int id { get; set; }
        string name { get; }

        int initialAttackPower { get; set; }
        int initialHealth { get; set; }

        int attackPower { get; }
        int health { get; }

        int experience { get; }
        int level { get; }

        Sprite GetFace();
    }
}
