using Enums;
using UnityEngine;

namespace UI
{
    public class MainPanel : MonoBehaviour
    {
        public void StartGame()
        {
            SoundManager.instance.PlayUiClick();
            GameManager.instance.StartGame();
            PageController.Instance.ShowPage(Pages.Game);
        }

        public void Settings()
        {
            SoundManager.instance.PlayUiClick();
            PageController.Instance.ShowPage(Pages.Settings);
        }
    }
}