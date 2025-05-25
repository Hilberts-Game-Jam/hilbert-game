using UnityEngine;

namespace Scripts.Core
{
    public static class ClassesAndEnums
    {
        public enum RoomType
        {
            RANDOM_ROOM,
            SCRIPTED_ROOM,
            CORRIDOR
        }

        public enum TileType
        {
            FREE,
            BLOCKED,
            DOOR,
            WALL
        }
        
        /// <summary>
        /// Used to specify the type of interaction that the player is performing.
        /// </summary>
        // todo: make this enum better
        public enum InteractionType
        {
            BROKEN_BED,
            BROKEN_CHAIR,
            BROKEN_SUITCASE,
            BROKEN_TABLE,
            
            BURNED_BED,
            BURNED_CHAIR,
            BURNED_SHELF,
            BURNED_SUITCASE,
            BURNED_TABLE,
            
            MOIST_BED,
            MOIST_CHAIR,
            MOIST_SHELF,
            MOIST_SUITCASE,
            MOIST_TABLE,
            
            NORMAL_BED,
            NORMAL_CHAIR,
            NORMAL_SHELF,
            NORMAL_SUITCASE,
            
            NONE,
            DOOR,
            ELEVATOR,
            START
        }
        
        public enum KeyObject
        {
            MIRROR_SHARD
        }

        public class FurnitureObjectInfo
        {

            private int _x_start;
            private int _y_start;
            private int _x_end;
            private int _y_end;
            private string _name;
            
            public FurnitureObjectInfo(int x_start, int x_end, int y_start, int y_end, string name)
            {
                _x_start = x_start;
                _y_start = y_start;
                _x_end = x_end;
                _y_end = y_end;
                _name = name;
            }

            public int GetXStart()
            {
                return _x_start;
            }
            
            public int GetYStart()
            {
                return _y_start;
            }
            
            public int GetXEnd()
            {
                return _x_end;
            }
            
            public int GetYEnd()
            {
                return _y_end;
            }

            public string GetName()
            {
                return _name;
            }
            
        }
        
        public static class D20
        {
            public static int Roll()
            {
                return Random.Range(1, 21);
            }
        }
    }
}