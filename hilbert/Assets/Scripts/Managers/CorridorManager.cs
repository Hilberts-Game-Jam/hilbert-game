using UnityEngine;
using Corridor;
using System.Collections.Generic;

namespace Scripts.Managers
{
    public class CorridorManager : MonoBehaviour
    {
        [SerializeField] private corridorGenerator corridorGen;
        [SerializeField] private int chunkWidth = 20; // Width of corridor Chunk
        private HashSet<int> generatedChunks = new HashSet<int>();
        private Transform playerTransform;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found!");
                return;
            }

            int playerPosition_x = Mathf.RoundToInt(player.transform.position.x);

            corridorGen.generateCorridor(playerPosition_x);
        }

        // Update is called once per frame
        void Update()
        {
            if (playerTransform == null) return;

            int currentChunk = Mathf.FloorToInt(playerTransform.position.x / chunkWidth);

            if (!generatedChunks.Contains(currentChunk))
            {
                corridorGen.generateCorridor(currentChunk);
                generatedChunks.Add(currentChunk);
            }
        }

        private void GenerateChunkAtPlayer()
        {
            if (playerTransform == null) return;
            int currentChunk = Mathf.FloorToInt(playerTransform.position.x / chunkWidth);
            if (!generatedChunks.Contains(currentChunk))
            {
                corridorGen.generateCorridor(currentChunk);
                generatedChunks.Add(currentChunk);
            }
        }
    }
}
