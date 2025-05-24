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

        private TileBase[] upperWallElements;
        private TileBase[] bottomWallElements;
        private TileBase[] upperDoorElements;
        private TileBase[] bottomDoorElements;

        public void generateCorridor()
        {
            //private TileBase[] upperWall = new TileBase[] { lampUpper, wallUpper };

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
                if (x % 5 == 0)
                {
                    int rand1 = Random.Range(0, 100);
                    int rand2 = Random.Range(0, 100);

                    if (rand1 < 5) // 5% chance for Elevator
                    {
                        Vector3Int elevatorPosition = new Vector3Int(x , 3, 0);
                        corridorTilemap.SetTile(elevatorPosition, elevatorUpper);
                    }
                    else // rest of the chances are Doors
                    {
                        Vector3Int doorPosition = new Vector3Int(x, 3, 0);
                        corridorTilemap.SetTile(doorPosition, doorUpper);
                    }

                    if (rand2 < 5) // Im once again asking your for 5% elevator access 
                    {
                        Vector3Int elevatorPosition = new Vector3Int(x , -3, 0);
                        corridorTilemap.SetTile(elevatorPosition, elevatorBottom);
                    }
                    else // rest of the chances are Doors
                    {
                        Vector3Int doorPosition = new Vector3Int(x, -3, 0);
                        corridorTilemap.SetTile(doorPosition, doorBottom);
                    }
                }
                else
                {
                    Vector3Int upperWall = new Vector3Int(x, 3, 0);
                    Vector3Int bottomWall = new Vector3Int(x, -3, 0);
                    int rand1 = Random.Range(0, 4);
                    int rand2 = Random.Range(0, 4);
                    if (rand1 == 0)
                    {
                        corridorTilemap.SetTile(upperWall, lampUpper);
                    }
                    else
                    {
                        corridorTilemap.SetTile(upperWall, wallUpper);
                    }

                    if (rand2 == 0)
                    {
                         corridorTilemap.SetTile(bottomWall, lampBottom);
                    }
                    else
                    {
                        corridorTilemap.SetTile(bottomWall, wallBottom);
                    }
                }
            }
        }

        void Start()
        {
            generateCorridor();
        }

        void Awake()
        {
            upperWallElements = new TileBase[] { lampUpper, wallUpper };
            bottomWallElements = new TileBase[] { lampBottom, wallBottom };
            upperDoorElements = new TileBase[] { doorUpper, elevatorUpper };
            bottomDoorElements = new TileBase[] { doorBottom, elevatorBottom };
        }
    }
}

