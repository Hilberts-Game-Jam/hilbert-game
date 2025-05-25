using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Core
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