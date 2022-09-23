using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.entities
{
    public class Hero : Entity, IHero
    {
        public HeroFaceType face { get; private set; }

        protected Sprite heroFace;
        
        public Hero() : base()
        {
            // For loading
        }

        public Hero(string name, int attackPower, int initialHealth, int experience, int level) : base(name, attackPower, initialHealth, experience, level)
        {
            // For new heroes
        }

        public void SetFace(HeroFaceType face)
        {
            this.face = face;
            heroFace = GraphicSprites.Instance.GetHeroSprite(face);
        }

        public override Sprite GetFace()
        {
            return heroFace;
        }

        public bool AddExperience(int xpGained)
        {
            // Add experience
            experience += xpGained;
            // Store old level
            int oldLevel = level;
            // Calculate new level
            level = 1 + (experience / Values.XP_FOR_LEVEL);
            // Check if we gained a level
            return oldLevel != level;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> data = base.Serialize();
            data[SaveFields.FACE] = (int)face;
            return data;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            base.Deserialize(data);
            face = (HeroFaceType)Convert.ToInt32(data[SaveFields.FACE]);
        }
    }
}
