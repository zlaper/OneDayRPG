using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using zg.gramrpg.assets;
using zg.gramrpg.data;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle;
using zg.gramrpg.rpg.battle.entities;
using zg.gramrpg.ui.components;

namespace zg.gramrpg.ui
{
    public class BattleResultScreen : MonoBehaviour
    {
        public Action onHeroScreen;

        [Header("Monster Wins")]
        public GameObject monsterWins;
        public Image monsterIcon;

        [Header("Hero Win")]
        public GameObject heroesWins;
        public HeroWinCell[] heroCells;

        [Header("Buttons")]
        public Button heroScreen;

        void Start()
        {
            Assert.IsTrue(heroCells.Length == Values.BATTLE_HEROES, "Enough hero rows");
            heroScreen.onClick.AddListener(OnHeroList);
        }

        public void BattleResult(WinnerType winner, BattleField battle)
        {
            if (winner == WinnerType.Heroes)
            {
                // Show heroes win
                monsterWins.SetActive(false);
                heroesWins.SetActive(true);

                // Set initial values
                BattleHero[] heroes = battle.heroes;
                for (int i = 0; i < Values.BATTLE_HEROES; i++)
                {
                    BattleHero battleHero = heroes[i];
                    HeroWinCell cell = heroCells[i];
                    cell.ShowEntity(battleHero.entity, battleHero.isDead);

                    if (!battleHero.isDead)
                    {
                        // TEMPORARY - For ease of presentation, we do the level up here, so that we can present the new values to the player
                        // Usually there would be a separate class that handles leveling up, with effects etc.
                        if (battleHero.hero.AddExperience(1))
                            // We Gained a Level!
                            cell.ShowLeveledEntity(battleHero.entity);
                    }
                }
                SoundControl.Instance.PlaySound(SoundType.YouWin);
            }
            else
            {
                // Show enemy win
                monsterWins.SetActive(true);
                heroesWins.SetActive(false);

                monsterIcon.sprite = battle.enemy.entity.GetFace();
                SoundControl.Instance.PlaySound(SoundType.YouLose);
            }
            // Play music
            SoundControl.Instance.PlayMusic(SoundType.EndMusic);
        }

        private void OnHeroList()
        {
            onHeroScreen?.Invoke();
            SoundControl.Instance.PlaySound(SoundType.Click);
        }
    }
}