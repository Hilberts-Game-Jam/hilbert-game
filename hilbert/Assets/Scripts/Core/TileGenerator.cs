using System;
using UnityEngine;
using Scripts.Managers;
using Scripts.Rooms;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine.Tilemaps;
using static Scripts.Core.ClassesAndEnums;

namespace Scripts.Core
{
    public class RoomTileGenerator : MonoBehaviour
    {

        private RoomLayout _layout;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private WallTile _wallTile;
        [SerializeField] private WallCornerTile _wallCornerTile;
        [SerializeField] private Tile _floorTile;
        [SerializeField] private DoorTile _doorTile;
        //[SerializeField] private Tile _blockedTile;
        private RoomType _type;
        private Vector2Int _spawnPoint;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            // Building the tiles
            for (int i = 0; i < _layout.GetWidth(); i++)
            {
                for (int j = 0; j < _layout.GetHeight(); j++)
                {

                    switch (_layout.GetTileAt(i, j))
                    {
                        case TileType.WALL: _tilemap.SetTile(new Vector3Int(i, j), GetWallTile(i, j, true));
                            break;
                        case TileType.DOOR: 
                            _tilemap.SetTile(new Vector3Int(i, j), GetWallTile(i,j, false));
                            
                            break;
                        //case TileType.BLOCKED: _tilemap.SetTile(new Vector3Int(i, j), _blockedTile);
                            //break;
                        case TileType.FREE: _tilemap.SetTile(new Vector3Int(i, j), _floorTile);
                            break;
                        default: _tilemap.SetTile(new Vector3Int(i, j), _floorTile);
                            break;
                    }
                    
                }
            }
            
        }

        private Tile GetWallTile(int x, int y, bool isWall)
        {
            if (x == 0 && y == 0)
            {
                return _wallCornerTile.lowerLeftCorner;
            } else if (x == 0 && y == _layout.GetHeight())
            {
                return _wallCornerTile.upperLeftCorner;
            } else if (x == _layout.GetWidth() && y == 0)
            {
                return _wallCornerTile.lowerRightCorner;

            } else if (x == _layout.GetWidth() && y == _layout.GetHeight())
            {
                return _wallCornerTile.upperRightCorner;
            } else if (x == 0)
            {
                return isWall ? _wallTile.leftWallTile : _doorTile.leftDoorTile;
            }
            else if (x == _layout.GetWidth())
            {
                return isWall ? _wallTile.rightWallTile : _doorTile.rightDoorTile;
            }
            else if (y == 0)
            {
                return isWall ? _wallTile.lowerWallTile : _doorTile.lowerDoorTile;
            }
            else
            {
                return isWall ? _wallTile.upperWallTile : _doorTile.upperDoorTile;
            }
        }

        private void Awake()
        {
            // Receiving room layout from RoomManager
            _layout = Managers.RoomManager.Instance.layout;
            _type = Managers.RoomManager.Instance.roomType;
        }

        [Serializable]

        private class WallTile
        {
            [SerializeField] public Tile upperWallTile;
            [SerializeField] public Tile lowerWallTile;
            [SerializeField] public Tile leftWallTile;
            [SerializeField] public Tile rightWallTile;
        }
        [Serializable]
        private class WallCornerTile
        {
            [SerializeField] public Tile upperLeftCorner;
            [SerializeField] public Tile lowerLeftCorner;
            [SerializeField] public Tile upperRightCorner;
            [SerializeField] public Tile lowerRightCorner;

        }

        [Serializable]
        private class DoorTile
        {
            [SerializeField] public Tile upperDoorTile;
            [SerializeField] public Tile lowerDoorTile; 
            [SerializeField] public Tile leftDoorTile;
            [SerializeField] public Tile rightDoorTile;

        }
    }
}
