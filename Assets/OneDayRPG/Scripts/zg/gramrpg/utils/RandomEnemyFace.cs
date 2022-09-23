using UnityEngine;
using zg.gramrpg.data;

namespace zg.gramrpg.utils
{
    public class RandomEnemyFace
    {

        private static EnemyFaceType[] faces = {EnemyFaceType.Face1, EnemyFaceType.Face2, EnemyFaceType.Face3, EnemyFaceType.Face4, EnemyFaceType.Face5};

        public static EnemyFaceType RandomFace()
        {
            return faces[Random.Range(0, faces.Length)];
        }
    }
}