using DG.Tweening;
using TMPro;
using UnityEngine;
using zg.gramrpg.assets;
using zg.gramrpg.data;

namespace zg.gramrpg.ui
{
    public class BattleScreen : MonoBehaviour
    {

        public TextMeshProUGUI turnNotification;

        private void Awake()
        {
            turnNotification.gameObject.SetActive(false);
        }

        public void ShowNotification(BattlePhaseType phaseType)
        {
            switch (phaseType)
            {
                case BattlePhaseType.Start:
                    turnNotification.text = "Battle Start!";
                    break;
                case BattlePhaseType.Hero:
                    turnNotification.text = "Heroes turn!";
                    break;
                case BattlePhaseType.Enemy:
                    turnNotification.text = "Enemy Turn!";
                    break;
            }

            AnimateText();
        }

        private void AnimateText()
        {
            // Stop previous animations
            DOTween.Kill(turnNotification.transform);

            // Reset scale and show
            turnNotification.transform.localScale = Vector3.one;
            turnNotification.gameObject.SetActive(true);

            // Animate
            turnNotification.transform.DOScale(Vector3.zero, Values.ANIMATION_PHASE_TIME)
                .SetEase(Ease.InBounce)
                .OnComplete(() => turnNotification.gameObject.SetActive(true));
        }
    }
}