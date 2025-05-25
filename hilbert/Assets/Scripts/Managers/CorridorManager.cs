using UnityEngine;
using Corridor;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Scripts.Managers
{
    public class CorridorManager : MonoBehaviour
    {
        [SerializeField] private corridorGenerator _corridorGen;
        [SerializeField] private int _chunkWidth = 10; // Width of corridor Chunk

        private HashSet<int> _generatedChunks = new HashSet<int>();
        private Transform _playerTransform;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public static CorridorManager Instance;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            Debug.Log("generatedChunks hat" + _generatedChunks.Count + "Elements");
            _generatedChunks.Clear();
            Debug.Log("generatedChunks hat" + _generatedChunks.Count + "Elements nach clear");

            //GenerateInitialCorridor();

            
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

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (SceneManager.GetSceneByName("GameScene") == SceneManager.GetActiveScene())
            {
                if (_playerTransform == null)
                {
                    GameObject player = GameObject.Find("Player");
                    if (player != null)
                    {
                        _playerTransform = player.transform;
                        GenerateInitialCorridor();
                    }
                    else return;
                }

                int currentChunk = Mathf.FloorToInt(_playerTransform.position.x / _chunkWidth);

                for (int offset = -2; offset <= 2; offset++)
                {
                    int chunkToCheck = currentChunk + offset;
                    if (!_generatedChunks.Contains(chunkToCheck))
                    {
                        Debug.Log("Chunk generated");
                        _corridorGen.generateCorridor(chunkToCheck * _chunkWidth);
                        _generatedChunks.Add(chunkToCheck);
                        if (_generatedChunks.Contains(chunkToCheck - 5))
                        {
                            _corridorGen.destroyCorridor((chunkToCheck - 5) * _chunkWidth);
                            _generatedChunks.Remove(chunkToCheck - 5);
                        }
                        if (_generatedChunks.Contains(chunkToCheck + 5))
                        {
                            _corridorGen.destroyCorridor((chunkToCheck + 5) * _chunkWidth);
                            _generatedChunks.Remove(chunkToCheck + 5);
                        }
                    }
                }
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _corridorGen = FindObjectOfType<corridorGenerator>();
            _generatedChunks.Clear();
            _playerTransform = null;
        }

        public void GenerateInitialCorridor()
        {
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found!");
                return;
            }

            _playerTransform = player.transform;

            int playerPosition_x = Mathf.RoundToInt(player.transform.position.x);
            int startChunk = Mathf.FloorToInt(playerPosition_x / _chunkWidth);

            if (!_generatedChunks.Contains(startChunk))
            {
                _corridorGen.generateCorridor(startChunk * _chunkWidth);
                _generatedChunks.Add(startChunk);            
            }

        }

        public void DestroyEveryTile() {
            foreach (int chunk in _generatedChunks)
            {
                _corridorGen.destroyCorridor(chunk * _chunkWidth);
            }
            _generatedChunks.Clear();
        }
    }
}
