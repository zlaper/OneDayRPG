using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RecyclableListView
{
    public class GridRecyclableList : MonoBehaviour
    {
        #region HANDLER ItemLoaded
        public delegate void OnItemLoadedHandler(RecyclableListItemBase item);
        public OnItemLoadedHandler onItemLoaded;

        public void ItemLoaded(RecyclableListItemBase item, bool clear = false)
        {
            if (onItemLoaded != null)
            {
                onItemLoaded(item);

                if (clear)
                    onItemLoaded = null;
            }
        }
        #endregion

        #region HANDLER ItemSelected
        public delegate void OnItemSelectedHandler(RecyclableListItemBase item);
        public OnItemSelectedHandler onItemSelected;

        public void ItemSelected(RecyclableListItemBase item, bool clear = false)
        {
            if (onItemSelected != null)
            {
                onItemSelected(item);

                if (clear)
                    onItemSelected = null;
            }
        }
        #endregion

        private enum ScrollDirection
        {
            NEXT,
            PREVIOUS
        }

        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private RectTransform _viewport;

        [SerializeField]
        private RectTransform _content;

        [SerializeField]
        private float _spacing;

        [SerializeField]
        private float _changeItemDragFactor;

        public List<RecyclableListItemBase> itemsList;

        public int Columns;

        private float _itemSizeX, _itemSizeY;
        private float _lastPosition;

        private int _itemsInList;
        private int _itemsTotal;
        private int _itemsVisible;

        private int _itemsToRecycleBefore;
        private int _itemsToRecycleAfter;

        private int _currentItemIndex;
        private int _lastItemIndex;

        private Vector2 _dragInitialPosition;
        private IListItemPool _pool;

        private int rows;

        public void Create(int items, IListItemPool pool)
        {
            Clear();

            _pool = pool;
            RecyclableListItemBase listItemPrefab = _pool.CreateItem();

            _scrollRect.vertical = true;
            _scrollRect.horizontal = false;

            _content.anchorMin = new Vector2(0, 1);
            _content.anchorMax = new Vector2(1, 1);

            rows = Mathf.CeilToInt(items / Columns);

            _itemSizeX = listItemPrefab.rectTransform.sizeDelta.x;
            _itemSizeY = listItemPrefab.rectTransform.sizeDelta.y;

            _content.sizeDelta = new Vector2(_itemSizeX * Columns + _spacing * (Columns - 1), _itemSizeY * rows + _spacing * (rows - 1));

            _itemsVisible = Mathf.CeilToInt(_viewport.rect.height / _itemSizeY);

            int itemsToInstantiate = _itemsVisible * Columns;

            if (_itemsVisible == 1)
            {
                itemsToInstantiate = 5;
            }
            else if (itemsToInstantiate < items)
            {
                itemsToInstantiate *= 2;
            }

            if (itemsToInstantiate > items)
            {
                itemsToInstantiate = items;
            }

            itemsList = new List<RecyclableListItemBase>();

            for (int i = 0; i < itemsToInstantiate; i++)
            {
                RecyclableListItemBase item = CreateNewItem(listItemPrefab, i);
                itemsList.Add(item);
                ItemLoaded(item);
            }

            _itemsTotal = items;
            _itemsInList = itemsList.Count;
            _lastItemIndex = rows - 1;
            _itemsToRecycleAfter = rows - _itemsVisible;

            _scrollRect.onValueChanged.AddListener((Vector2 position) =>
           {
               Recycle();
           });
            _pool.StoreItem(listItemPrefab);
        }

        private RecyclableListItemBase CreateNewItem(RecyclableListItemBase prefab, int index)
        {
            RecyclableListItemBase instance = _pool.CreateItem();
            instance.Index = index;
            instance.transform.SetParent(_content.transform);
            instance.transform.localScale = Vector3.one;
            instance.gameObject.SetActive(true);

            int col = index % Columns;
            int row = Mathf.CeilToInt(index / Columns);

            Vector2 position = new Vector2(col * (_itemSizeX + _spacing) + _itemSizeX / 2, -row * (_itemSizeY + _spacing) + _itemSizeY / 2);

            instance.rectTransform.anchoredPosition = position;

            return instance;
        }

        private void Recycle()
        {
            if (_lastPosition == -1)
            {
                _lastPosition = _content.anchoredPosition.y;
                return;
            }

            int displacedRows = Mathf.FloorToInt(Mathf.Abs(_content.anchoredPosition.y - _lastPosition) / _itemSizeY);
            if (displacedRows == 0)
                return;

            ScrollDirection direction = GetScrollDirection();

            for (int i = 0; i < displacedRows; i++)
            {
                switch (direction)
                {
                    case ScrollDirection.NEXT:
                        for (int j = 0; j < Columns; j++)
                            NextItem();
                        break;

                    case ScrollDirection.PREVIOUS:
                        for (int j = 0; j < Columns; j++)
                            PreviousItem();
                        break;
                }

                if (direction == ScrollDirection.NEXT)
                {
                    _lastPosition += _itemSizeY + _spacing;
                }
                else
                {
                    _lastPosition -= _itemSizeY + _spacing;
                }
            }
        }

        private void NextItem()
        {
            if (_itemsToRecycleBefore >= (_itemsInList - _itemsVisible) / 2 && _lastItemIndex < _itemsTotal - 1)
            {
                _lastItemIndex++;
                RecycleItem(ScrollDirection.NEXT);
            }
            else
            {
                _itemsToRecycleBefore++;
                _itemsToRecycleAfter--;
            }
        }

        private void PreviousItem()
        {
            if (_itemsToRecycleAfter >= (_itemsInList - _itemsVisible) / 2 && _lastItemIndex > _itemsInList - 1)
            {
                RecycleItem(ScrollDirection.PREVIOUS);
                _lastItemIndex--;
            }
            else
            {
                _itemsToRecycleBefore--;
                _itemsToRecycleAfter++;
            }
        }

        private void RecycleItem(ScrollDirection direction)
        {
            RecyclableListItemBase firstItem = itemsList[0];
            RecyclableListItemBase lastItem = itemsList[_itemsInList - 1];

            float targetPosition = (_itemSizeY + _spacing);

            switch (direction)
            {
                case ScrollDirection.NEXT:
                    firstItem.Position = new Vector2(firstItem.Position.x, lastItem.Position.y - targetPosition);

                    firstItem.Index = _lastItemIndex;
                    firstItem.transform.SetAsLastSibling();

                    itemsList.RemoveAt(0);
                    itemsList.Add(firstItem);

                    ItemLoaded(firstItem);
                    break;

                case ScrollDirection.PREVIOUS:
                    lastItem.Position = new Vector2(lastItem.Position.x, firstItem.Position.y + targetPosition);

                    lastItem.Index = _lastItemIndex - _itemsInList;
                    lastItem.transform.SetAsFirstSibling();

                    itemsList.RemoveAt(itemsList.Count - 1);
                    itemsList.Insert(0, lastItem);

                    ItemLoaded(lastItem);
                    break;
            }
            Canvas.ForceUpdateCanvases();
        }

        public void Clear()
        {
            _scrollRect.horizontalNormalizedPosition = 0;
            _scrollRect.verticalNormalizedPosition = 1;

            if (itemsList != null)
            {
                if (_pool != null)
                {
                    for (int i = _itemsInList - 1; i >= 0; i--)
                        _pool.StoreItem(itemsList[i]);
                }
                else
                {
                    for (int i = _itemsInList - 1; i >= 0; i--)
                        Destroy(itemsList[i].gameObject);
                }

                itemsList.Clear();
                itemsList = null;
            }

            _lastPosition = -1;
        }

        private ScrollDirection GetScrollDirection()
        {
            return _lastPosition > _content.anchoredPosition.y ? ScrollDirection.PREVIOUS : ScrollDirection.NEXT;
        }
    }
}