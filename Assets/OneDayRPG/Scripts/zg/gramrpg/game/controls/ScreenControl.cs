using UnityEngine;
using zg.gramrpg.data;
using zg.gramrpg.ui;

namespace zg.gramrpg.game.controls
{
    public class ScreenControl : MonoBehaviour
    {
        
        public HeroScreen heroScreen;
        public BattleScreen battleScreen;
        public BattleResultScreen battleResultScreen;

        private GameObject[] _screens;

        void Start()
        {
            // Create array of screens for easy hide
            _screens = new[] { heroScreen.gameObject, battleScreen.gameObject, battleResultScreen.gameObject };
        }

        public void ShowScreen(ScreenType screen)
        {
            // Hide all screens
            HideScreens();
            // Show screen
            switch (screen)
            {
                case ScreenType.Hero:
                    heroScreen.gameObject.SetActive(true);
                    break;
                case ScreenType.Battle:
                    battleScreen.gameObject.SetActive(true);
                    break;
                case ScreenType.End:
                    battleResultScreen.gameObject.SetActive(true);
                    break;
            }
        }

        private void HideScreens()
        {
            for (int i = _screens.Length - 1; i >= 0; i--)
                _screens[i].SetActive(false);
        }
    }
}