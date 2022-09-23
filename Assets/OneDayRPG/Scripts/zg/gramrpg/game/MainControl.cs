using System.Collections;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.rpg.battle;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.rpg.entities;
using zg.gramrpg.rpg.heroes;
using zg.gramrpg.serialize;
using zg.gramrpg.ui.components;
using zg.gramrpg.utils;

namespace zg.gramrpg.game.controls
{

    public class MainControl : Singleton<MainControl>
    {

        public BattleField battleField;
        public BattleControl battleControl;
        public ScreenControl screenControl;
        public EntityGenerator entityGenerator;
        public EntityInfo entityInfo;
        public HeroList heroList;

        private SaveDataManager _saveManager;

        IEnumerator Start()
        {
            // Wait for everything to init
            yield return new WaitForEndOfFrame();

            // Initialize game
            Application.targetFrameRate = 60;

            // Create components
            heroList = new HeroList();
            _saveManager = new SaveDataManager();

            // Assign hero list to hero screen
            screenControl.heroScreen.SetHeroList(heroList);

            // Screen events
            screenControl.heroScreen.onScreenEvent += OnHeroScreenEvent;
            screenControl.heroScreen.onShowInfo += OnShowInfo;
            screenControl.battleResultScreen.onHeroScreen += CheckForNewHero;

            // Battle events
            battleControl.phaseChanged += OnBattlePhase;

            // Long tap info events
            battleField.onShowInfo += OnShowInfo;

            // Check for save game
            if (_saveManager.hasSaved)
            {
                // Has saved, restore progress
                _saveManager.Load(this);
            }
            else
            {
                // No save game, start new game
                NewGame();
            }
        }

        #region Main Functions

        public void NewGame()
        {
            // Clear previous heroes
            heroList.Clear();
            // Add starting heroes
            for (int i = 0; i < Values.STARTING_HEROES; i++)
            {
                Hero hero = entityGenerator.GenerateHero();
                heroList.AddHero(hero);
            }
            // Clear battle count
            battleControl.Clear();
            // Show hero screen
            ShowHeroScreen();
            // Save new game
            SaveGame();
        }

        public void SaveGame()
        {
            _saveManager.SaveGame(this);
        }

        private void StartBattle()
        {
            // Reset battle phase
            battleControl.ResetBattle();

            // Get heroes
            Hero[] selectedHeroes = screenControl.heroScreen.SelectedHeroes();
            // Create enemy
            Enemy enemy = entityGenerator.GenerateEnemy();
            // Random enemy face
            enemy.SetFace(RandomEnemyFace.RandomFace());
            // Assign
            battleField.SetHeroes(selectedHeroes);
            battleField.SetEnemy(enemy);

            // Show screen and start battle
            ShowBattleScreen();
            // Save game
            SaveGame();
        }

        private void OnBattlePhase(BattlePhaseType battlePhase)
        {
            switch (battlePhase)
            {
                case BattlePhaseType.Start:
                case BattlePhaseType.Hero:
                case BattlePhaseType.Enemy:
                    screenControl.battleScreen.ShowNotification(battlePhase);
                    break;

                case BattlePhaseType.Complete:
                    ShowEndScreen();
                    break;
            }
            SaveGame();
        }

        private void OnHeroScreenEvent(HeroScreenEventType eventType)
        {
            switch (eventType)
            {
                case HeroScreenEventType.StartGame:
                    StartBattle();
                    break;
                case HeroScreenEventType.NewGame:
                    NewGame();
                    break;
            }
        }

        #endregion

        #region Screens Flow

        public void ShowHeroScreen()
        {
            screenControl.ShowScreen(ScreenType.Hero);
            screenControl.heroScreen.ShowScreen(battleControl.battlesCount);
        }

        public void ShowBattleScreen()
        {
            screenControl.ShowScreen(ScreenType.Battle);
            battleControl.StartBattle();
        }

        public void ShowEndScreen()
        {
            screenControl.ShowScreen(ScreenType.End);
            screenControl.battleResultScreen.BattleResult(battleControl.battleFlow.winner, battleField);
        }

        private void CheckForNewHero()
        {
            // Check if we are at max
            int currentHeroes = heroList.heroes.Count;
            if (currentHeroes < Values.MAX_HEROES)
            {
                // Check if we earned a new hero
                int expectedHeroes = Values.STARTING_HEROES + Mathf.FloorToInt(battleControl.battlesCount / Values.NEW_HERO_BATTLES);
                if (expectedHeroes > currentHeroes)
                {
                    // We earned a new hero
                    // Create
                    Hero hero = entityGenerator.GenerateHero();
                    // Store
                    heroList.AddHero(hero);
                    // Show
                    entityInfo.NewHero(hero);
                }
            }

            // Go back to hero screen
            ShowHeroScreen();
            // Save game
            SaveGame();
        }

        #endregion

        #region Util Functions

        private void OnShowInfo(IEntity entity)
        {
            entityInfo.ShowInfo(entity);
        }

        #endregion

    }
}