using DG.Tweening;
using TMPro;
using UnityEngine;
using zg.gramrpg.assets;

namespace zg.gramrpg.rpg.battle.entities
{
    public class BattleDamage : MonoBehaviour
    {
        public TextMeshPro damageText;

        public void ShowDamage(int amount, Transform target)
        {
            // Clear previous tweens
            DOTween.Kill(transform);

            // Assign damage amount
            damageText.text = amount.ToString();

            // Set position and scale
            transform.localScale = Vector3.one;
            transform.position = target.position;

            // Animate
            transform.DOMoveY(transform.position.y + 2, Values.ANIMATION_DAMAGE_TIME);
            transform.DOScale(Vector3.zero, Values.ANIMATION_DAMAGE_TIME * 0.5f)
                .SetDelay(Values.ANIMATION_DAMAGE_TIME * 0.5f);
        }

    }
}
