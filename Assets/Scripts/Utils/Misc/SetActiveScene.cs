using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Misc
{
    public class SetActiveScene : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.SetActiveScene(gameObject.scene);
        }
    }
}