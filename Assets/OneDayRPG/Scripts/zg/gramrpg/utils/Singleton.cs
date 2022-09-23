using UnityEngine;

namespace zg.gramrpg.utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        public static T Instance => _instance;
        private static T _instance;

        private void Awake()
        {
            // Check if already exists
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Get singleton component
            _instance = GetComponent<T>();
        }
    }
}