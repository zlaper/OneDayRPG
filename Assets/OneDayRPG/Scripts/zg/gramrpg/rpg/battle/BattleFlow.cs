using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.data;
using zg.gramrpg.game.controls;
using zg.gramrpg.rpg.battle.battlePhases;
using zg.gramrpg.rpg.definitions;
using zg.gramrpg.serialize;

namespace zg.gramrpg.rpg.battle
{
    public class BattleFlow : MonoBehaviour, ISerializable
    {
        public Action<BattlePhaseType> phaseChanged;

        public BattleField battleField;

        public WinnerType winner { get; private set; }
        public bool battleComplete { get; private set; }

        protected BattlePhaseType currentPhase;
        protected Dictionary<BattlePhaseType, IBattlePhase> phases;

        private void Start()
        {
            battleComplete = true;
            CreateBattlePhases();
        }

        public void SetPhase(BattlePhaseType battlePhaseType)
        {
            currentPhase = battlePhaseType;
        }

        public void StartBattle()
        {
            // start battle
            battleComplete = false;
            // Play music
            SoundControl.Instance.PlayMusic(SoundType.BattleMusic);
            // Start battle flow
            StartCoroutine(Battle());
        }

        public void FinishBattle(WinnerType winner)
        {
            // Store winner
            this.winner = winner;

            // End battle and notify main
            battleComplete = true;
            phaseChanged?.Invoke(BattlePhaseType.Complete);
        }

        public IEnumerator Battle()
        {
            // While the battle is not complete, execute battle phases
            while (!battleComplete)
            {
                // Notify of phase start
                phaseChanged?.Invoke(currentPhase);

                // Execute current phase
                yield return phases[currentPhase].ExecutePhase();

                // Wait for next frame
                yield return null;

                // Move to next phase
                currentPhase = NextPhase();
            }

            yield return null;
        }

        #region Util Functions

        private void CreateBattlePhases()
        {
            // Create battle phases
            phases = new Dictionary<BattlePhaseType, IBattlePhase>
            {
                [BattlePhaseType.Start] = new BattlePhaseStart(battleField),
                [BattlePhaseType.Hero] = new BattlePhaseHero(battleField),
                [BattlePhaseType.Enemy] = new BattlePhaseEnemy(battleField),
                [BattlePhaseType.CheckEnd] = new BattlePhaseEndCheck(battleField, this)
            };
        }

        private BattlePhaseType NextPhase()
        {
            // Find next phase based on current phase
            switch (currentPhase)
            {
                // Start animation
                case BattlePhaseType.Start:
                    return BattlePhaseType.Hero;

                // Battle loop
                case BattlePhaseType.Hero:
                    return BattlePhaseType.Enemy;
                case BattlePhaseType.Enemy:
                    return BattlePhaseType.CheckEnd;
                case BattlePhaseType.CheckEnd:
                    return BattlePhaseType.Hero;
                // End battle loop
            }

            return BattlePhaseType.Complete;
        }

        #endregion

        #region Save/Load

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>()
            {
                {SaveFields.PHASE, (int) currentPhase}
            };
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            SetPhase((BattlePhaseType)Convert.ToInt32(data[SaveFields.PHASE]));
        }

        #endregion
    }
}