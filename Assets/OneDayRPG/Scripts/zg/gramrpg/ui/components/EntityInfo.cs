using TMPro;
using UnityEngine;
using UnityEngine.UI;
using zg.gramrpg.rpg.definitions;

namespace zg.gramrpg.ui.components
{
    public class EntityInfo : MonoBehaviour
    {
        public GameObject newHero;
        public Image heroFace;

        public TextMeshProUGUI entityName;
        public TextMeshProUGUI attackPower;
        public TextMeshProUGUI health;
        public TextMeshProUGUI experience;
        public TextMeshProUGUI level;

        public Button close;

        private void Start()
        {
            close.onClick.AddListener(OnClose);
        }

        public void ShowInfo(IEntity entity)
        {
            // Show face
            heroFace.sprite = entity.GetFace();
            // Show data
            entityName.text = entity.name;
            attackPower.text = entity.attackPower.ToString();
            health.text = entity.health.ToString();
            experience.text = entity.experience.ToString();
            level.text = entity.level.ToString();
            //Show panel
            newHero.SetActive(false);
            this.gameObject.SetActive(true);
        }

        public void NewHero(IEntity entity)
        {
            ShowInfo(entity);
            // Show new hero text
            newHero.SetActive(true);
        }

        private void OnClose()
        {
            this.gameObject.SetActive(false);
        }

    }
}