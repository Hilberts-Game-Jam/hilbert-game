using UnityEngine;
using static Scripts.Core.ClassesAndEnums;

namespace Scripts.Rooms
{
    public class RoomLayout
    {

        private int _width;
        private int _height;
        private TileType[,] _tileMap;

        public RoomLayout(int width, int height)
        {
            _width = width;
            _height = height;

            // Initialize TileMap and filling it with free tiles and walls
            _tileMap = new TileType[_width, _height];

            for (int i = 0; i < _tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < _tileMap.GetLength(1); j++)
                {
                    if (i == 0 || i == _width - 1 || j == 0 || j == _height - 1) _tileMap[i, j] = TileType.WALL;
                    else _tileMap[i, j] = TileType.FREE;
                }
            }
        }

        public TileType GetTileAt(int x, int y)
        {
            return _tileMap[x, y];
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public void SetTileAt(int x, int y, TileType tile)
        {
            _tileMap[x, y] = tile;
        }

        public void PrintMap()
        {
            string result = "";
            for (int i = 0; i < _tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < _tileMap.GetLength(1); j++)
                {
                    switch (_tileMap[i, j])
                    {
                        case TileType.BLOCKED:
                            result = result + "B";
                            break;
                        case TileType.FREE:
                            result = result + "F";
                            break;
                        default:
                            result = result + "E";
                            break;
                    }
                }
            }
        }

    }
}

