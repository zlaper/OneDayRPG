using System;
using UnityEngine;
using UnityEngine.UI;
using zg.gramrpg.data;
using zg.utils;

namespace zg.gramrpg.ui.components
{
    public class HeroSelect : MonoBehaviour
    {
        // Events
        public Action<TapType, HeroSelect> onTap;

        // Display
        public Image heroFace;
        public Image heroSelected;

        // Hero ID
        public int heroID { get; private set; }

        // Properties
        private bool _hasHero;
        private TapDetector _tapDetector;

        void Start()
        {
            _tapDetector = GetComponent<TapDetector>();
            _tapDetector.tapped += OnTapped;

            Clear();
        }

        private void OnTapped(TapType tapType)
        {
            if (!_hasHero) return;

            onTap?.Invoke(tapType, this);
        }

        public void SetHero(Sprite face, int heroID)
        {
            // Store face and id
            heroFace.sprite = face;
            heroFace.gameObject.SetActive(true);
            this.heroID = heroID;

            // Set slot properties
            _hasHero = true;
            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            heroSelected.gameObject.SetActive(selected);
        }

        public void Clear()
        {
            _hasHero = false;
            heroFace.gameObject.SetActive(false);
            SetSelected(false);
        }
    }
}