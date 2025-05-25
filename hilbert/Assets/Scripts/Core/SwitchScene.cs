using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SwitchScene : MonoBehaviour
    {
        [SerializeField] public string sceneName;
        public void SwitchToScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}