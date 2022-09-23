//
//  ListControllerItem.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using UnityEngine.UI;

namespace RecyclableListView
{
    public class RecyclableListItemBase : MonoBehaviour
    {
        #region HANDLER Selected

        public Button select;

        void Awake()
        {
            Init();
        }

        virtual public void Init()
        {
            if (select != null)
                select.onClick.AddListener(OnClick);
        }

        virtual protected void OnClick()
        {
        }
        #endregion

        [SerializeField]
        private RectTransform _rectTransform;

        [HideInInspector]
        public int Index;
        [HideInInspector]
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
            private set { }
        }

        public Vector2 Position
        {
            get { return rectTransform.anchoredPosition; }
            set { rectTransform.anchoredPosition = value; }
        }

        virtual public void Clear()
        {
            //gameObject.SetActive(false);
        }
    }
}