using Enums;
using UnityEngine;

namespace UI
{
    public class MainPanel : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.instance.StartGame();
            PageController.Instance.ShowPage(Pages.Game);
        }

        public void Settings()
        {
            PageController.Instance.ShowPage(Pages.Settings);
        }
    }
}