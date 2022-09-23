using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.data;
using zg.gramrpg.rpg.battle;
using zg.gramrpg.serialize;

namespace zg.gramrpg.game.controls
{
    public class BattleControl : MonoBehaviour, ISerializable
    {
        // Event propagation
        public Action<BattlePhaseType> phaseChanged;

        // Battle field
        public BattleFlow battleFlow;

        // Battles counter
        public int battlesCount { get; private set; }

        // In battle flag
        public bool inBattle => !battleFlow.battleComplete;

        private void Awake()
        {
            // Listen for events
            battleFlow.phaseChanged += OnPhaseComplete;
        }

        #region Control Functions

        public void ResetBattle()
        {
            battleFlow.SetPhase(BattlePhaseType.Start);
        }

        public void StartBattle()
        {
            battleFlow.StartBattle();
        }

        public void Clear()
        {
            battlesCount = 0;
        }

        private void OnPhaseComplete(BattlePhaseType battlePhase)
        {
            // Check for end of battle and increment battle counter
            if (battlePhase == BattlePhaseType.Complete)
                battlesCount++;

            // Notify main of event
            phaseChanged?.Invoke(battlePhase);
        }

        #endregion

        #region Save/Load

        public Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {SaveFields.BATTLES_COUNT, battlesCount}
            };

            // Check if we have an active battle
            if (inBattle)
                data[SaveFields.BATTLE] = battleFlow.Serialize();

            return data;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            // Load counter
            battlesCount = Convert.ToInt32(data[SaveFields.BATTLES_COUNT]);
            // If active battle, load saved phase
            if (data.TryGetValue(SaveFields.BATTLE, out object flowData))
                battleFlow.Deserialize(flowData as Dictionary<string, object>);
        }

        #endregion
    }
}