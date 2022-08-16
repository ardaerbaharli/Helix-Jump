using System.Collections;
using Enums;
using UnityEngine;

namespace UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private GameObject loadingIcon;
        [SerializeField] private float rotatingSpeed;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.instance.IsInitialized);
            PageController.Instance.ShowPage(Pages.Main);
        }

        private void Update()
        {
            loadingIcon.transform.Rotate(0, 0, Time.deltaTime * rotatingSpeed);
        }
    }
}