using UnityEngine;
using Scripts.Player;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;

namespace Corridor
{
    public class corridorGenerator : MonoBehaviour
    {
        [SerializeField] public Tilemap corridorTilemap;
        [SerializeField] private TileBase corridorTile;
        [SerializeField] private TileBase lampUpper;
        [SerializeField] private TileBase lampBottom;
        [SerializeField] private TileBase wallUpper;
        [SerializeField] private TileBase wallBottom;
        [SerializeField] private TileBase doorUpper;
        [SerializeField] private TileBase doorBottom;
        [SerializeField] private TileBase elevatorUpper;
        [SerializeField] private TileBase elevatorBottom;

        public void generateCorridor()
        {
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found!");
                return;
            }

            int playerPosition_x = Mathf.RoundToInt(player.transform.position.x);
            playerPosition_x = (int)GameObject.Find("Player").transform.position.x;

            for (int x = playerPosition_x - 10; x < playerPosition_x + 10; x++)
            {
                for (int y = -2; y < 3; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0); // z = 0 for 2D tilemaps
                    corridorTilemap.SetTile(position, corridorTile);
                }
            }
        }

        void Start()
        {
            generateCorridor();
        }
    }
}

