using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        public void Resume()
        {
            GameManager.instance.Resume();
            PageController.Instance.ShowPage(Pages.Game);
        }

        public void Settings()
        {
            PageController.Instance.ShowPage(Pages.Settings);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}