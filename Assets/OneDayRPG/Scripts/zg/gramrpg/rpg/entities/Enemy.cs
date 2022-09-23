using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.entities
{
    public class Enemy : Entity, IFace<EnemyFaceType>
    {
        public EnemyFaceType face { get; private set; }

        protected Sprite enemyFace;

        public Enemy() : base()
        {
            // For loading
        }

        public Enemy(string name, int attackPower, int initialHealth, int experience, int level) : base(name, attackPower, initialHealth, experience, level)
        {
            // For new enemies
        }

        public void SetFace(EnemyFaceType face)
        {
            this.face = face;
            enemyFace = GraphicSprites.Instance.GetEnemySprite(face);
        }

        public override Sprite GetFace()
        {
            return enemyFace;
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
            SetFace((EnemyFaceType)Convert.ToInt32(data[SaveFields.FACE]));
        }
    }
}
