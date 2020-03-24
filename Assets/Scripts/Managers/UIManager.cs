using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        private void InitSingleton()
        {
            if (instance != null && instance != this) Destroy(gameObject);
            else instance = this;
        }

        private void Awake()
        {
            InitSingleton();
            gameObject.SetActive(false);
        }

        public void ShowWinPopUp()
        {
            // Debug.Log("ShowWinPopUp");
            gameObject.SetActive(true);
        }

        public void OnTryAgainClick()
        {
            // Debug.Log("Reload Scene");
            BallManager.Instance.ResetGame();
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        public void OnExitGameClick()
        {
            // Debug.Log("Quit the Game");
            Application.Quit();
        }
    }
}
