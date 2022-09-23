using System.Collections.Generic;
using UnityEngine;

namespace zg.gramrpg.utils
{
    public class TypePool<T>
    {

        private GameObject _itemTemplate;
        private Transform _parent;
        private Stack<T> _pool = new Stack<T>();

        public void Setup(GameObject itemTemplate, Transform parent)
        {
            _itemTemplate = itemTemplate;
            _parent = parent;
        }

        public void CreatePoolItems(int num)
        {
            // Create some initial items
            for (int i = 0; i < num; i++)
                StoreItem(CreateComponent());
        }

        public T CreateItem(Transform parent)
        {
            T slot = default;
            // Check if stored
            if (_pool.Count > 0)
            {
                slot = _pool.Pop();
            }
            // If not in pool, create new
            else
            {
                slot = CreateComponent();
            }

            // Set parent
            (slot as MonoBehaviour).transform.SetParent(parent, false);
            return slot;
        }

        public void StoreItem(T item)
        {
            // Store in pool
            if (!_pool.Contains(item))
            {
                // Move transform parent
                (item as Component).gameObject.transform.SetParent(_parent, false);
                _pool.Push(item);
            }
        }

        private T CreateComponent()
        {
            GameObject _item = GameObject.Instantiate(_itemTemplate);
            return _item.GetComponent<T>();
        }
    }
}
