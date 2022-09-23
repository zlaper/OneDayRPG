using System;

namespace zg.gramrpg.rpg.definitions
{
    public interface IFace<T> where T : Enum
    {
        T face { get; }

        void SetFace(T face);
    }
}
