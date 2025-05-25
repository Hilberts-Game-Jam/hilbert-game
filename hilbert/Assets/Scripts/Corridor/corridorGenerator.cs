using UnityEngine;
using Scripts.Player;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;

namespace Corridor
{
    public class corridorGenerator : MonoBehaviour
    {
        [SerializeField] public Tilemap corridorTilemap;
        [SerializeField] private TileBase _corridorTile;
        [SerializeField] private TileBase _lampUpper;
        [SerializeField] private TileBase _lampBottom;
        [SerializeField] private TileBase _wallUpper;
        [SerializeField] private TileBase _wallBottom;
        [SerializeField] private TileBase _doorUpper;
        [SerializeField] private TileBase _doorBottom;
        [SerializeField] private TileBase _elevatorUpper;
        [SerializeField] private TileBase _elevatorBottom;

        [SerializeField] private GameObject _doorInteractablePrefab;
        [SerializeField] private GameObject _elevatorInteractablePrefab;

        private TileBase[] _upperWallElements;
        private TileBase[] _bottomWallElements;
        private TileBase[] _upperDoorElements;
        private TileBase[] _bottomDoorElements;

        public void generateCorridor(int position_x)
        {

            for (int x = position_x - 10; x < position_x + 10; x++)
            {
                for (int y = -2; y < 3; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0); // z = 0 for 2D tilemaps
                    corridorTilemap.SetTile(position, _corridorTile);
                }
                if (x % 5 == 0)
                {
                    int rand1 = Random.Range(0, 100);
                    int rand2 = Random.Range(0, 100);

                    if (rand1 < 5) // 5% chance for Elevator
                    {
                        Vector3Int elevatorPosition = new Vector3Int(x , 3, 0);
                        corridorTilemap.SetTile(elevatorPosition, _elevatorUpper);
                        Vector3 worldPos = corridorTilemap.CellToWorld(elevatorPosition) + new Vector3(0.5f, 0.5f, 0);
                        Instantiate(_elevatorInteractablePrefab, worldPos, Quaternion.identity);
                    }
                    else // rest of the chances are Doors
                    {
                        Vector3Int doorPosition = new Vector3Int(x, 3, 0);
                        corridorTilemap.SetTile(doorPosition, _doorUpper);
                        Vector3 worldPos = corridorTilemap.CellToWorld(doorPosition) + new Vector3(0.5f, 0.5f, 0);
                        Instantiate(_doorInteractablePrefab, worldPos, Quaternion.identity);
                    }

                    if (rand2 < 5) // Im once again asking your for 5% elevator access 
                    {
                        Vector3Int elevatorPosition = new Vector3Int(x , -3, 0);
                        corridorTilemap.SetTile(elevatorPosition, _elevatorBottom);
                        Vector3 worldPos = corridorTilemap.CellToWorld(elevatorPosition) - new Vector3(0.5f, 0.5f, 0);
                        Instantiate(_elevatorInteractablePrefab, worldPos, Quaternion.identity);
                    }
                    else // rest of the chances are Doors
                    {
                        Vector3Int doorPosition = new Vector3Int(x, -3, 0);
                        corridorTilemap.SetTile(doorPosition, _doorBottom);
                        Vector3 worldPos = corridorTilemap.CellToWorld(doorPosition) - new Vector3(0.5f, 0.5f, 0);
                        Instantiate(_doorInteractablePrefab, worldPos, Quaternion.identity);
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
                        corridorTilemap.SetTile(upperWall, _lampUpper);
                    }
                    else
                    {
                        corridorTilemap.SetTile(upperWall, _wallUpper);
                    }

                    if (rand2 == 0)
                    {
                         corridorTilemap.SetTile(bottomWall, _lampBottom);
                    }
                    else
                    {
                        corridorTilemap.SetTile(bottomWall, _wallBottom);
                    }
                }
            }
        }

        public void destroyCorridor(int position_x)
        {
            for (int x = position_x - 10; x < position_x + 10; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    Vector3Int toBeRemoved = new Vector3Int(x, y, 0);
                    corridorTilemap.SetTile(toBeRemoved, null);
                    Debug.Log("Destroyed a Tile");
                }            
            }
        }


        void Awake()
        {
            _upperWallElements = new TileBase[] { _lampUpper, _wallUpper };
            _bottomWallElements = new TileBase[] { _lampBottom, _wallBottom };
            _upperDoorElements = new TileBase[] { _doorUpper, _elevatorUpper };
            _bottomDoorElements = new TileBase[] { _doorBottom, _elevatorBottom };
        }
    }
}

