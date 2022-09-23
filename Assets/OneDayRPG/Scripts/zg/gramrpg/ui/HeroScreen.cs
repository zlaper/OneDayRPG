using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.rpg.entities;
using zg.gramrpg.rpg.heroes;
using zg.gramrpg.ui.components;

namespace zg.gramrpg.ui
{
    public class HeroScreen : MonoBehaviour
    {
        // Events
        public Action<HeroScreenEventType> onScreenEvent;
        public Action<IEntity> onShowInfo;

        // Components
        public HeroSelect[] heroSlots;
        public Button newGame;
        public Button startGame;
        public TextMeshProUGUI battlesCounter;

        // Properties
        protected HeroList heroList;
        protected List<int> selectedHeroes;
        protected bool enoughHeroes => selectedHeroes.Count == Values.BATTLE_HEROES;

        void Start()
        {
            Assert.IsTrue(heroSlots.Length == Values.MAX_HEROES, "Enough hero slots");

            selectedHeroes = new List<int>();

            // Buttons
            newGame.onClick.AddListener(OnNewGame);
            startGame.onClick.AddListener(OnStartGame);

            // Listen for events
            int len = heroSlots.Length;
            for (int i = 0; i < len; i++)
                heroSlots[i].onTap += OnTapHero;

        }

        public void ShowScreen(int battles)
        {
            // Set heroes
            int len = heroList.heroes.Count;
            for (int i = 0; i < len; i++)
            {
                Hero hero = heroList.heroes[i];
                heroSlots[i].SetHero(hero.GetFace(), hero.id);
            }

            // Clear the rest of the slots
            len = heroSlots.Length;
            for (int i = heroList.heroes.Count; i < len; i++)
            {
                heroSlots[i].Clear();
            }

            // Clear selected
            selectedHeroes.Clear();

            // Update battle counter
            battlesCounter.text = battles.ToString();

            // Play music
            SoundControl.Instance.PlayMusic(SoundType.HeroMusic);

            // Check Start
            CheckStart();
        }

        public void SetHeroList(HeroList heroList)
        {
            this.heroList = heroList;
        }

        public Hero[] SelectedHeroes()
        {
            // Create hero array
            Hero[] heroes = new Hero[Values.BATTLE_HEROES];
            for (int i = 0; i < Values.BATTLE_HEROES; i++)
                heroes[i] = heroList.GetHeroFromID(selectedHeroes[i]);

            return heroes;
        }

        #region Select handlers

        private void OnTapHero(TapType tapType, HeroSelect heroSelect)
        {
            // Handle event
            switch (tapType)
            {
                case TapType.Tap:
                    SelectDeselect(heroSelect);
                    break;
                case TapType.LongTap:
                    Hero hero = heroList.GetHeroFromID(heroSelect.heroID);
                    onShowInfo?.Invoke(hero);
                    break;
            }
        }

        private void SelectDeselect(HeroSelect heroSelect)
        {
            // Check if already selected
            if (selectedHeroes.Contains(heroSelect.heroID))
            {
                // Already selected, remove from selected
                selectedHeroes.Remove(heroSelect.heroID);
                heroSelect.SetSelected(false);
                SoundControl.Instance.PlaySound(SoundType.DeselectHero);
            }
            else
            {
                // If we already have enough heroes, return
                if (enoughHeroes) return;

                // Add to selection
                selectedHeroes.Add(heroSelect.heroID);
                heroSelect.SetSelected(true);
                SoundControl.Instance.PlaySound(SoundType.SelectHero);
            }
            CheckStart();
        }

        #endregion

        #region Util Functions

        private void OnNewGame()
        {
            onScreenEvent?.Invoke(HeroScreenEventType.NewGame);
            SoundControl.Instance.PlaySound(SoundType.Click);
        }

        private void OnStartGame()
        {
            onScreenEvent?.Invoke(HeroScreenEventType.StartGame);
            SoundControl.Instance.PlaySound(SoundType.StartBattle);
        }

        private void CheckStart()
        {
            // Enable start when we have 3 heroes
            startGame.interactable = enoughHeroes;
        }

        #endregion
    }
}