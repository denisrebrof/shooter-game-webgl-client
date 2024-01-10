using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.Platforms.CrazySDK.CrazyLogo.Assets.Scripts
{
    public class LoadNextScene : MonoBehaviour
    {
        [SerializeField] private float withDelay = 2;

        private void Start()
        {
            Invoke("loadNextScene", withDelay);
        }

        private void loadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}