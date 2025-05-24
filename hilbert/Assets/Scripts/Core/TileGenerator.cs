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
        [SerializeField] private Tile _wallTile;
        [SerializeField] private Tile _wallCornerTile;
        [SerializeField] private Tile _floorTile;
        [SerializeField] private Tile _doorTile;
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
                    
                    SetRotationOfTileAtPosition(i ,j);
                }
            }
            
        }

        private void SetRotationOfTileAtPosition(int i, int j)
        {
            int width = _layout.GetWidth();
            int height = _layout.GetHeight();
            int rotationAngle = 0;

            // Ecken
            if (i == 0 && j == 0) rotationAngle = 180; // unten links
            else if (i == 0 && j == height - 1) rotationAngle = 90; // unten rechts
            else if (i == width - 1 && j == 0) rotationAngle = 270; // oben links
            else if (i == width - 1 && j == height - 1) rotationAngle = 0; // oben rechts

            // Kanten (aber nicht Ecken)
            else if (i == 0) rotationAngle = 90; // untere Wand / Türe
            else if (i == width - 1) rotationAngle = 270; // untere Wand / Türe
            else if (j == 0) rotationAngle = 180; // untere Wand / Türe
            else if (j == height - 1) rotationAngle = 0; // untere Wand / Türe

            _tilemap.SetTransformMatrix(new Vector3Int(i, j, 0), Matrix4x4.Rotate(Quaternion.Euler(0, 0, rotationAngle)));
            _tilemap.RefreshTile(new Vector3Int(i, j, 0));
        }
            
        private Tile GetWallTile(int x, int y, bool isWall)
        {
            var isCorner = x == 0 && y == 0 ||
                           x == 0 && y == _layout.GetHeight() - 1 ||
                           x == _layout.GetWidth() - 1 && y == 0 ||
                           x == _layout.GetWidth() - 1 && y == _layout.GetHeight() - 1;

            if (!isWall)
                return _doorTile;
            if (isCorner)
                return _wallCornerTile;
            return _wallTile;
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
