using UnityEngine;
using zg.gramrpg.data;
using zg.gramrpg.utils;

namespace zg.gramrpg.assets
{
    public class GraphicSprites : Singleton<GraphicSprites>
    {
        [Header("Entity Sprites")]
        public Sprite[] heroes;
        public Sprite[] enemies;

        public Sprite GetHeroSprite(HeroFaceType face)
        {
            // Simple conversion
            int idx = Mathf.Clamp((int)face, 0, heroes.Length - 1);
            return heroes[idx];
        }

        public Sprite GetEnemySprite(EnemyFaceType face)
        {
            // Simple conversion
            int idx = Mathf.Clamp((int)face, 0, enemies.Length - 1);
            return enemies[idx];
        }
    }
}