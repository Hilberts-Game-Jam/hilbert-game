using System.Linq;
using Scripts.Managers;
using Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public static EndManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void BackToMenu()
    {
        foreach (var key in Player.PlayerKeyObjects.Keys.ToList())
        {
            Player.PlayerKeyObjects[key] = 0;
        }

        while (WeirdnessManager.WeirdnessLevel > 1)
        {
            WeirdnessManager.DecrementWeirdness();
        }

        MonologueManager._firstStartRoom = true;
        SceneManager.LoadScene("MenuScene");
    }
}
