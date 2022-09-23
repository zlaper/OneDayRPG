using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace zg.gramrpg.assets.entities
{
    [CreateAssetMenu(fileName = "RandomData", menuName = "GramRPG/RandomData", order = 2)]
    public class RandomData : ScriptableObject
    {
        [Header("Attack Power Range")]
        public MinMax attackPower;

        [Header("Initial Health Range")]
        public MinMax initialHealth;

        [Header("Initial Experience Range")]
        public MinMax experience;

        [Header("Initial Level Range")]
        public MinMax level;

        [Serializable]
        public struct MinMax
        {
            // For easy min max setting
            public int min;
            public int max;
            
            // helper function to get random value
            public int random => Random.Range(min, max);
        }
    }
}
