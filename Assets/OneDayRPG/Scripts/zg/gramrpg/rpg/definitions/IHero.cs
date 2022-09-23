using zg.gramrpg.data;

namespace zg.gramrpg.rpg.definitions
{
    public interface IHero : IEntity, IFace<HeroFaceType>
    {
        bool AddExperience(int xpGained);
    }
}
