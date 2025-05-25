using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SwitchScene : MonoBehaviour
    {
        public static void SwitchToScene(string sceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}