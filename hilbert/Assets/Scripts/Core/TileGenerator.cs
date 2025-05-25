using System;
using UnityEngine;
using Scripts.Managers;
using Scripts.Rooms;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using static Scripts.Core.ClassesAndEnums;

namespace Scripts.Core
{
    public class RoomTileGenerator : MonoBehaviour
    {

        private RoomLayout _layout;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tile _wallTile;
        [SerializeField] private Tile _floorTile;
        [SerializeField] private Tile _doorTile;
        [SerializeField] private Tile _blockedTile;
        private RoomType _type;
        
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
                        case TileType.WALL: _tilemap.SetTile(new Vector3Int(i, j), _wallTile);
                            break;
                        case TileType.DOOR: _tilemap.SetTile(new Vector3Int(i, j), _doorTile);
                            break;
                        case TileType.BLOCKED: _tilemap.SetTile(new Vector3Int(i, j), _blockedTile);
                            break;
                        case TileType.FREE: _tilemap.SetTile(new Vector3Int(i, j), _floorTile);
                            break;
                        default: _tilemap.SetTile(new Vector3Int(i, j), _floorTile);
                            break;
                    }
                    
                }
            }
            
        }

        private void Awake()
        {
            // Receiving room layout from RoomManager
            _layout = Managers.RoomManager.Instance.layout;
            _type = Managers.RoomManager.Instance.roomType;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
