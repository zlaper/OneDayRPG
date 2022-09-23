using System;
using System.Collections.Generic;
using Tiny;
using UnityEngine;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle.entities;
using zg.gramrpg.rpg.entities;

namespace zg.gramrpg.serialize
{
    public class SaveDataManager
    {
        public bool hasSaved { get { return PlayerPrefs.HasKey(SaveFields.GRAMRPG); } }

        public void SaveGame(MainControl mainControl)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            // Save heroes
            data[SaveFields.HERO_LIST] = mainControl.heroList.Serialize();
            // Save battle flow
            data[SaveFields.BATTLE] = mainControl.battleControl.Serialize();
            // Save battle status
            if (mainControl.battleControl.inBattle)
                data[SaveFields.BATTLES_FIELD] = mainControl.battleField.Serialize();

            PlayerPrefs.SetString(SaveFields.GRAMRPG, Json.Encode(data));
        }

        public void Load(MainControl mainControl)
        {
            string saveString = PlayerPrefs.GetString(SaveFields.GRAMRPG);

            try
            {
                Dictionary<string, object> data = JsonParser.ParseValue(saveString) as Dictionary<string, object>;

                // Load heroes
                mainControl.heroList.Deserialize(data[SaveFields.HERO_LIST] as Dictionary<string, object>);

                // Load battle counter
                Dictionary<string, object> battleData = data[SaveFields.BATTLE] as Dictionary<string, object>;
                mainControl.battleControl.Deserialize(battleData);

                // Do we have an active battle ?
                if (data.TryGetValue(SaveFields.BATTLES_FIELD, out object activeBattle))
                {
                    Dictionary<string, object> battle = activeBattle as Dictionary<string, object>;

                    // Load phase and enemy
                    mainControl.battleField.Deserialize(battle);

                    // Load heroes and health
                    List<object> battleHeroes = battle[SaveFields.HEROES] as List<object>;
                    int len = battleHeroes.Count;
                    for (int i = 0; i < len; i++)
                    {
                        Dictionary<string, object> heroData = battleHeroes[i] as Dictionary<string, object>;
                        // Get hero from id
                        int heroID = Convert.ToInt32(heroData[SaveFields.ENTITY]);
                        Hero hero = mainControl.heroList.GetHeroFromID(heroID);

                        // Get battle hero
                        BattleHero battleHero = mainControl.battleField.heroes[i];
                        // Set hero entity
                        battleHero.SetHero(hero);
                        // Load health
                        battleHero.Deserialize(heroData);
                    }

                    // Continue battle
                    mainControl.ShowBattleScreen();
                }
                else
                {
                    // No battle in progress
                    mainControl.ShowHeroScreen();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Invalid save string: \n" + saveString);
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
        }
    }
}