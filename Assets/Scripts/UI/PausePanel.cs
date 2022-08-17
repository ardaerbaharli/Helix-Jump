using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        public void Resume()
        {
            SoundManager.instance.PlayUiClick();
            GameManager.instance.Resume();
            PageController.Instance.ShowPage(Pages.Game);
        }

        public void Settings()
        {
            SoundManager.instance.PlayUiClick();
            PageController.Instance.ShowPage(Pages.Settings);
        }

        public void Restart()
        {
            SoundManager.instance.PlayUiClick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}