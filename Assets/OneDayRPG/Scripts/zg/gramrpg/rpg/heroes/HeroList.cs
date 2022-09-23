using System.Collections.Generic;
using zg.gramrpg.data;
using zg.gramrpg.rpg.entities;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.heroes
{
    public class HeroList : ISerializable
    {
        public List<Hero> heroes;
        protected Dictionary<int, Hero> heroesLookup;

        public HeroList()
        {
            heroes = new List<Hero>();
            heroesLookup = new Dictionary<int, Hero>();
        }

        public void AddHero(Hero hero)
        {
            // Very simple id assignment
            hero.id = GetNextHeroID();
            // Very simple face assignment
            hero.SetFace((HeroFaceType)hero.id);
            // Store hero
            heroes.Add(hero);
            heroesLookup[hero.id] = hero;
        }

        public int GetNextHeroID()
        {
            // Very simple id assignment
            return heroes.Count;
        }

        public Hero GetHeroFromID(int id)
        {
            if (heroesLookup.TryGetValue(id, out Hero hero))
                return hero;

            return null;
        }

        public void Clear()
        {
            heroes.Clear();
            heroesLookup.Clear();
        }

        public Dictionary<string, object> Serialize()
        {
            // Create hero list data
            List<object> heroesData = new List<object>();
            int len = heroes.Count;
            for (int i = 0; i < len; i++)
                heroesData.Add(heroes[i].Serialize());

            return new Dictionary<string, object>()
            {
                {SaveFields.HEROES, heroesData }
            };
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            List<object> heroesData = data[SaveFields.HEROES] as List<object>;
            int len = heroesData.Count;
            for (int i = 0; i < len; i++)
            {
                // Load hero
                Dictionary<string, object> heroData = heroesData[i] as Dictionary<string, object>;
                Hero hero = new Hero();
                hero.Deserialize(heroData);
                
                // Add
                AddHero(hero);
            }
        }
    }
}
